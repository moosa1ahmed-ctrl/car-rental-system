# Developer Setup Guide

## Prerequisites
- PostgreSQL with pgAdmin4 — https://www.enterprisedb.com/downloads/postgres-postgresql-downloads
- .NET SDK (net9.0-windows)

## Database setup
1. Create a local PostgreSQL server (e.g. name it `CarRental_CP317`), username `postgres`, and choose your own password.
2. Set the connection string as an environment variable named `CARRENTAL_DB_CONNECTION`, e.g.:
   ```
   Host=localhost;Port=5432;Database=CarRentalDB;Username=postgres;Password=your_password_here
   ```
3. Entity Framework Core will migrate the tables automatically on first run.

## Things to know
The SQL database is interfaced with Entity Framework Core (EF Core).

## File explanations
- `UserDataContext.cs` — connects to the SQL database and defines its schema
- `User.cs` — defines the `Users` and `UserInformations` tables
- `CarEntry.cs` — defines the `CarEntries` table
- `GlobalVariables.cs` — shared app-wide variables
- `App.xaml` / `App.xaml.cs` — application entry point, no need to touch

## Program flow
`LoginWindow.xaml` → if no `UserInformations` row exists for the user → `UserSetup.xaml` → `MainWindow.xaml`
`LoginWindow.xaml` → else → `MainWindow.xaml`

The GUI is defined in `.xaml` files; the logic is in the matching `.xaml.cs` files.
