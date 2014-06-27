WebSharper Chat
===============

A simple chat application in F# using WebSharper with WebSockets.

How to run
===============
- Colne this repository
- Open project using Visual Studio (preferrably 2013 but should work in older versions)
- Enable NuGet package restore in the options then try to build the project
- If it won't build try restarting Visual Studio and unloading/reloading the project
- Setup your MSSQL database and change the connection string in the SQLConnection module.
This line:
```fsharp
    type internal DbSchema = SqlDataConnection<"Data Source=(localdb)\Projects;Initial Catalog=WebSharperChat;Integrated Security=True;\
                                                Connect Timeout=30;Encrypt=False;TrustServerCertificate=False">
```

To setup a database for the project you can use the following script:
```sql
    CREATE TABLE [dbo].[Users] (
        [Id]        INT        IDENTITY (1, 1) NOT NULL,
        [username]  NCHAR (64) NOT NULL,
        [password]  NCHAR (64) NOT NULL,
        [lasttoken] NCHAR (64) NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );
```

Live preview
===============
[http://websharper-chat.apphb.com/](http://websharper-chat.apphb.com/)
You can login with: `qwe2` `jelszo` and `masik` `jelszo`



