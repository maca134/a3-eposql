# EpoSql - [Downloads](https://github.com/maca134/a3-eposql/releases)
A server mod for A3 Epoch to allow use of MySQL instead of Redis. Once data is retrevied from the database it is held in memory and only updates when the server send data to the extension. This means you CAN NOT edit player data if they have logged in during the current server session (This behavour will be changed if people request it), to get around this, you can disable caching globally but this will slow things down.

Worst case, if you do edit the database while the server is running, the changes will be lost. Nothing will explode =P

Redis MAY still be required, as is the current `EpochServer.dll` but all player/building/etc data will be redirected to MySQL/Sqlite.

##### Performance
EpoSql: 0.0126999/0.00810089 ms — with/without full logging

Offical Epoch: 0.0524994 ms – Offical epoch dll

#### THIS EXTENSION IS COMPLETELY WIP. ENSURE YOU BACKUP EVERYTHING!

##### IM LOOKING FOR PEOPLE TO HELP OUT WITH IT.

### Installation
- Download the latest [release](https://github.com/maca134/a3-eposql/releases)
- Extract `@eposql` on your server (you need a seperate copy per server instance)
- Add `@eposql` to the `-servermod` start-up param
- If you are modifing the server's CfgFunctions `preinit`, you will need to add: `call ESQ_fnc_Init;` so your server files BEFORE any Epoch code starts to load.
- Now open `EpoSql.ini` and edit to settings to suit your setup. 
    - `ShowConsole`: Displays a console window showing logging.
    - `LogRequests`: Logs everything but also slows stuff down.
    - `CacheData`: Setting this to false means every request hits database.
    - `Driver`: Either MySQL or Sqlite. MySQL is recommended for live servers.
    - `MySQL Host`: MySQL hostname/ip
    - `MySQL Port`: MySQL port
    - `MySQL User`: MySQL user
    - `MySQL Password`: MySQL password
    - `MySQL Database`: MySQL database name
    - `Sqlite DBFile`: Sqlite filename
- Create a new database.
- Once all the config is setup, run `CreateDatabase.exe` to create the database schema.
- Now you can start the server.

### Importing From Redis
The mod includes an exe to import from Redis.
- Setup the mod as stated above.
- Run `Import.exe` with the correct params:
    - `--host`: Redis hostname
    - `--port`: Redis post
    - `--password`: Redis password
    - `--db`: Redis DBID
    - `--instance`: Epoch instance id
- Once complete, all Redis data should be in the database.

### License
This work is licensed under a [Creative Commons Attribution-NonCommercial-NoDerivatives 4.0](http://creativecommons.org/licenses/by-nc-nd/4.0/) International License.
