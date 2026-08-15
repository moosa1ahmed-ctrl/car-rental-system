# Car Rental System — CP317 Group Project

A desktop car rental management application built for CP317 (Software Engineering) at Wilfrid Laurier University. Users can sign up, log in, and manage car listings and bookings through a WPF desktop interface backed by a PostgreSQL database.

**Team 28**
- SabilUllah Hussaini (Product Owner)
- Sahil Sandhu (Product Owner & Developer)
- Moosa Ahmed (Scrum Master & Developer)
- Mahad Farrukh (Developer)
- Ayesha Raheel (Developer)
- Kritika Kamath (Developer)
- Shalin Panjwani (Developer)

## Stack
- C# / .NET (net9.0-windows)
- WPF (Windows Presentation Foundation) for the UI
- Entity Framework Core + Npgsql (PostgreSQL)

## Repo structure
```
├── src/                    # Application source code
│   ├── LoginWindow.xaml(.cs)
│   ├── MainWindow.xaml(.cs)
│   ├── UserSetup.xaml(.cs)
│   ├── UserDataContext.cs  # EF Core DB context
│   ├── User.cs / CarEntry.cs / Booking.cs
│   ├── Migrations/
│   └── DEV_GUIDE.md        # local setup instructions
├── docs/                   # Project deliverables (milestones, sprints, final report)
│   ├── Milestone-01.pdf
│   ├── Milestone-02.pdf
│   ├── Sprint-01.pdf
│   ├── Sprint-02.pdf
│   ├── Sprint-03.pdf
│   ├── Final-Report.pdf
│   ├── Presentation.pdf
│   └── Product-Backlog.xlsx
└── .gitignore
```

## Running locally
1. Install PostgreSQL and .NET SDK (net9.0-windows).
2. See [`src/DEV_GUIDE.md`](src/DEV_GUIDE.md) for database setup — the connection string is read from an environment variable (`CARRENTAL_DB_CONNECTION`), not hardcoded.
3. Open `src/CarRental-CP317.sln` in Visual Studio and run.

## Project docs
Full development history — from initial milestone through final delivery — is in [`docs/`](docs), including sprint reports, the final project writeup, and the team presentation.
