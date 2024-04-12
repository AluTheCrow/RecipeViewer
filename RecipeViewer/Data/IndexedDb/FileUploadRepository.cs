using BlazorDexie.JsModule;
using BlazorDexie.ObjUrl;
using Microsoft.IO;
using System.Net.Mime;

namespace RecipeViewer.Data.IndexedDb;

public interface IFileUploadRepository
{
    ValueTask<string> UploadFileAsync(Guid recipeId, Stream stream, CancellationToken cancellationToken = default);
    Task<Stream> GetBlobAsStreamAsync(string url, CancellationToken cancellationToken = default);
    ValueTask<bool> DeleteBlobAsync(Guid id, string url, CancellationToken cancellationToken = default);
}

internal sealed class FileUploadRepository(IModuleFactory jsModuleFactory, ILogger<FileUploadRepository> logger, RecyclableMemoryStreamManager recyclableMemoryStreamManager, ObjectUrlService objectUrlService) : IFileUploadRepository
{
    private readonly RecipeImageDb _recipeImageDb = new(jsModuleFactory);

    public async ValueTask<string> UploadFileAsync(Guid recipeId, Stream stream, CancellationToken cancellationToken = default)
    {
        if (stream?.CanRead != true)
        {
            logger.LogError("Stream is null or cannot be read");
            return string.Empty;
        }

        var blobData = await DecomposeStreamAsync(stream, cancellationToken);
        var blobUrl = await objectUrlService.Create(blobData, MediaTypeNames.Application.Octet, cancellationToken);
        await _recipeImageDb.RecipeImages.PutObjectUrl(blobUrl, recipeId, cancellationToken);
        return blobUrl;
    }

    public async Task<Stream> GetBlobAsStreamAsync(string url, CancellationToken cancellationToken = default)
    {
        if (String.IsNullOrWhiteSpace(url))
        {
            logger.LogError("Url is null or empty");
            return Stream.Null;
        }

        var blobData = await objectUrlService.FetchData(url, cancellationToken);
        await objectUrlService.Revoke(url, cancellationToken);
        return recyclableMemoryStreamManager.GetStream(blobData);
    }

    public async ValueTask<bool> DeleteBlobAsync(Guid id, string url, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty || String.IsNullOrWhiteSpace(url))
        {
            logger.LogError("Url is null or empty");
            return false;
        }

        try
        {
            await _recipeImageDb.RecipeImages.Delete(id, cancellationToken);

            await objectUrlService.Revoke(url, cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting blob: {Message}", ex);
            return false;
        }
    }

    private async ValueTask<byte[]> DecomposeStreamAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (stream is MemoryStream memoryStream)
        {
            return memoryStream.ToArray();
        }

        logger.LogInformation("Supplied stream was not a MemoryStream, copying to MemoryStream");

        await using var ms = recyclableMemoryStreamManager.GetStream("FileUploadService");
        await stream.CopyToAsync(ms, cancellationToken);
        return ms.ToArray();
    }
}