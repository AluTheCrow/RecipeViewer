# RecipeViewer
A simple Blazor WASM Application, built for demonstration purposes
## About this App

- ### Summary
    - This is a simple recipe viewer application. It was created by Andrew Beers for demo purposes. The application allows you to view recipes and their ingredients. You can also create new recipes and store them in the database.
    - The application was created using Blazor WebAssembly and [Blazorise components.](https://blazorise.com) 
    - We also utilize [Bootstrap 5](https://getbootstrap.com) for styling and layout, and [Font Awesome Icons](https://fontawesome.com/) for icons.
- ### How To Build
  - #### Prerequisites
    - .NET 8 SDK
    - It's recommended to install the wasm-tools workload
    - <details>
      <summary>Instal workload</summary>
      <code>dotnet workload install wasm-tools</code>
      </details>
  - Cloning & Building 
  ```git
    $ git clone https://github.com/AluTheCrow/RecipeViewer
    $ cd RecipeViewer
    $ dotnet build --configuration Release
    $ dotnet run --configuration Release
    ```

- ### Debugging
    - Open the ```RecipeViewer.sln``` file in Visual Studio 2022
    - You can press F5, or click the green "play" button to open a browser and load wasm.
  
- ### About the Database             
    - The application uses a [SQLite database](https://www.sqlite.org/index.html) to store recipes. SQLite is a lightweight, serverless database engine that is easy to use and configure. The database is created and managed using [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) a powerful and flexible ORM for .NET.
    - We also use [Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/) to provide the necessary tools and libraries to interact with the SQLite database.
    - While normally SQLite is not available client-side, Blazor WebAssembly allows us to use SQLite in the browser, and we also take advantage of IndexedDB to store the database locally. This allows the application to function offline and without a server.
    - In the future, we may consider moving to a more robust database solution, such as SQL Server or PostgreSQL, for production applications.
    - While SQLite is great for small applications and demos, it may not be suitable for large-scale applications with high traffic and complex data requirements.
    - To allow SQLite usage in the browser, we needed to adjust some items within the application's configuration.
       ```xml
            <PropertyGroup>
                <WasmBuildNative>true</WasmBuildNative>
		        <PublishedTrimmed>true</PublishedTrimmed>
		        <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
		        <EmccExtraLDFlags>-s WARN_ON_UNDEFINED_SYMBOLS=0 -lidbfs.js</EmccExtraLDFlags>
		        <RunAOTCompilation>true</RunAOTCompilation>
		        <WasmStripILAfterAOT>false</WasmStripILAfterAOT>
            </PropertyGroup> 
      ```
    - IndexedDB is great for storing data locally, but it is not a full-featured database and has limitations compared to traditional databases. We utilize a library called [Blazor Dexie](https://github.com/simon-kuster/BlazorDexie) to interact with IndexedDB and provide a more robust and flexible API for working with the database - within this we gain LINQ support and a more familiar API for working with the database. This allows us to store and retrieve images as well as text data.
    - With IndexedDb storing images, we can then further leverage the browser's cache to store images and other static assets, reducing the need to download them each time the application is loaded, and provide a semi-permaent storage solution for the application.
    - The SQLite instance stores references to the images (as urls) stored in IndexedDB, allowing us to retrieve the images when needed.
- ### About the Javascript
    - While Blazor WebAssembly allows us to write C# coderuns in the browser, there are times when we neinteract with JavaScript. This is especially trueworking with the browser's APIs, such as IndexedDB, orwe need to interact with JavaScript libraries or frameworks.
    - We use [JQuery](https://jquery.com) to interact with the DOM and provide a more robust and flexible APworking with the browser's APIs. This allows us to and retrieve images as well as text data. We use JQuery as a way to enlarge the logo when the user hovers over it, and to dynamically tell the DOM that code/syntax will need highlighting for the code snippets on the about page.
    - We also use [HighlightJs](https://highlightjs.org) to provide code/syntax highlighting for the code snippets in the about page. Highlight.js is a popular and flexible syntax highlighter that supports a wide range of languages and styles.
    - Of course, we also use [DexieJs](https://dexie.org/) to interact with IndexedDB, and we use the BlazorDexie library to provide a more robust and flexible API for working with the database. This allows us to store and retrieve images as well as text data.