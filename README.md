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

## Table of Contents
- [Team](#team-28)
- [Stack](#stack)
- [Repo Structure](#repo-structure)
- [Environment Configuration](#environment-configuration)
- [Database Schema Overview](#database-schema-overview)
- [App Flow](#app-flow)
- [Running Locally](#running-locally)
- [Project Docs](#project-docs)

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

## Environment Configuration

The database connection string is read from an environment variable — never hardcoded in source. Set it locally before running:

```
CARRENTAL_DB_CONNECTION=Host=localhost;Port=5432;Database=CarRentalDB;Username=postgres;Password=your_password_here
```

| Variable | Description |
|---|---|
| `CARRENTAL_DB_CONNECTION` | Full PostgreSQL (Npgsql) connection string. If unset, falls back to a local placeholder that will not authenticate — set your own before running. |

## Database Schema Overview

Four EF Core entities, migrated to PostgreSQL via `src/Migrations/`.

| Table | Description |
|---|---|
| `Users` | Login credentials — `AccountID` (key), `Email`, `Password` |
| `UserInformations` | Profile details linked by `AccountID` — name, card number, address, city, province, phone |
| `CarEntries` | Car listings — `CarID` (key), year, make, model, city, province, price, image URL, and a `BookingIDs` array used to check availability against existing bookings |
| `Bookings` | Rental records — `BookingID` (key), `AccountID`, `CarID`, `TotalPrice`, `StartDate`, `EndDate`. *(Model is defined; booking-creation flow is not yet wired into the UI.)* |

**Key relationships**

| From | To | Relationship |
|---|---|---|
| Users | UserInformations | One user has one profile (shared `AccountID`) |
| Users | Bookings | One user has many bookings |
| CarEntries | Bookings | One car has many bookings, tracked via `BookingIDs` |

## App flow
`LoginWindow` → if no `UserInformations` row exists for the account → `UserSetup` → `MainWindow`
`LoginWindow` → else → `MainWindow` directly

## Running locally
1. Install PostgreSQL and .NET SDK (net9.0-windows).
2. See [`src/DEV_GUIDE.md`](src/DEV_GUIDE.md) for database setup — the connection string is read from an environment variable (`CARRENTAL_DB_CONNECTION`), not hardcoded.
3. Open `src/CarRental-CP317.sln` in Visual Studio and run.

## Project docs
Full development history — from initial milestone through final delivery — is in [`docs/`](docs), including sprint reports, the final project writeup, and the team presentation.
