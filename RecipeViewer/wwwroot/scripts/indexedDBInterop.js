export function mountAndInitializeSqliteFile() {
    window.Module.FS.mkdir('/database');
    window.Module.FS.mount(window.Module.FS.filesystems.IDBFS, {}, '/database');
    syncDbFileToIndexedDb(true);
}
export function syncDbFileToIndexedDb(populate) {
    return new Promise((resolve, reject) => {
        window.Module.FS.syncfs(populate, (err) => {
            if (err) {
                console.log('syncfs failed. Error:', err);
                reject(err);
            }
            else {
                console.log('synced successfully.');
                resolve();
            }
        });
    });
}
