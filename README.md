# IDVault - Server

## Getting Set Up

1. Install .NET SDK 8.0 (or newer; exlcuding version 9)
2. Install a .NET compatible IDE (e.g. Visual Studio, Jetbrains Rider, Neovim)
3. Clone the repository
4. Open the project in your IDE
5. Ensure database is running (See `Database` section)
6. Run the project (Use IDE run button or `dotnet run`)
7. Ensure webserver is functional by navigating to `localhost:<port>` (check console output for the port number; visual studio will open the page automatically)in your web browser. You should see some response for the `/` route.


## Database - PostgreSQL
1. Install and run PostgreSQL version 16.2 or newer.
2. Ensure: 
    a. The `postgres` user exists
    b. the password for the `postgres` user is `idvault_server`
3. Verify database is functional using the command `dotnet ef database update` in the root of this project. This command will create all the required tables and apply all migrations.

