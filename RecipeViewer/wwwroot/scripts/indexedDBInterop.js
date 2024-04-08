export function syncDbFileToIndexedDb(dbName) {
    return new Promise((resolve, reject) => {
        const db = window.indexedDB.open(dbName, 1);
        db.onupgradeneeded = () => {
            db.result.createObjectStore('Files', { keyPath: 'id' });
        };
        db.onerror = (event) => {
            reject(event);
        };
        db.onsuccess = () => {
            const req = db.result.transaction('Files', 'readwrite').objectStore('Files').get('file');
            req.onsuccess = () => {
                Module.FS_createDataFile('/', 'file', req.result, true, true, true);
                resolve();
            };
        };
        let lastModifiedAt = new Date();

        setInterval(() => {
            const path = `indexeddb://${dbName}`;
            if (FS.analyzePath(path).exists) {
                const mtime = FS.stat(path).mtime;
                if (mtime.valueOf() !== lastModifiedAt.valueOf()) {
                    lastModifiedAt = mtime;
                    const dbFile = FS.readFile(path);
                    db.result.transaction('Files', 'readwrite').objectStore('Files').put(dbFile, 'file');
                }
            }
        },
            1000);
    });
}