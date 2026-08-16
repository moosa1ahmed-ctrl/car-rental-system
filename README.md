# Car Rental System

CP317 – Software Engineering | Fall 2025 | Desktop Application, Group Project

## Table of Contents
- [Project Overview](#project-overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Team Members](#team-members)
- [Repository Structure](#repository-structure)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Database Setup](#database-setup)
  - [Running the Application](#running-the-application)
- [Environment Configuration](#environment-configuration)
- [Database Schema Overview](#database-schema-overview)
- [Application Flow](#application-flow)
- [Testing](#testing)
- [Milestone & Sprint Deliverables](#milestone--sprint-deliverables)
- [Known Issues / Limitations](#known-issues--limitations)
- [Project Management](#project-management)
- [License](#license)

## Project Overview
Car Rental System is a WPF desktop application that lets customers create an account, browse a catalog of listed vehicles, and manage bookings, while the underlying data model tracks car availability against existing reservations. It was built over a full academic term by a 7-person Agile team, following a milestone → sprint → final-delivery structure.

**In Scope:**
- Account registration and login
- First-time profile setup (name, address, phone, payment card on file)
- Car listing browsing (year, make, model, city/province, price, image)
- Data model for bookings, linked to accounts and cars

**Out of Scope:**
- Payment processing
- Booking creation UI (data model exists; not yet wired to the interface — see [Known Issues](#known-issues--limitations))
- Admin/provider-side listing management
- Web or mobile clients (Windows desktop only)

## Features

**Must Have**
- User registration and login
- First-time account setup flow (profile details collected before full app access)
- Car listing browsing with image, price, and location
- Data model enforcing referential integrity between users, cars, and bookings

**Should Have**
- Booking creation wired into the UI (modeled in the database, not yet exposed in-app)
- Search/filter of car listings by city, price, or make

**Could Have (Stretch Goals)**
- Payment processing integration
- Admin dashboard for managing the car inventory
- Booking history and cancellation flow

## Technology Stack
| Layer | Technology |
|---|---|
| UI | WPF (Windows Presentation Foundation), XAML |
| Language | C# / .NET 9 (net9.0-windows) |
| ORM | Entity Framework Core |
| Database | PostgreSQL (via Npgsql) |
| IDE | Visual Studio |
| Version Control | Git, GitHub |

## Team Members
| Name | Role |
|---|---|
| SabilUllah Hussaini | Product Owner |
| Sahil Sandhu | Product Owner & Developer |
| Moosa Ahmed | Scrum Master & Developer |
| Mahad Farrukh | Developer |
| Ayesha Raheel | Developer |
| Kritika Kamath | Developer |
| Shalin Panjwani | Developer |

## Repository Structure
```
car-rental-system/
├── README.md
├── .gitignore
├── src/                     ← Application source
│   ├── CarRental-CP317.sln
│   ├── App.xaml(.cs)
│   ├── LoginWindow.xaml(.cs)
│   ├── MainWindow.xaml(.cs)
│   ├── UserSetup.xaml(.cs)
│   ├── UserDataContext.cs   ← EF Core DB context
│   ├── User.cs              ← Users, UserInformations entities
│   ├── CarEntry.cs          ← CarEntries entity
│   ├── Booking.cs           ← Bookings entity
│   ├── GlobalVariables.cs
│   ├── Migrations/          ← EF Core migration history
│   └── DEV_GUIDE.md         ← Local setup + file-by-file guide
└── docs/                    ← Project deliverables
    ├── Milestone-01.pdf
    ├── Milestone-02.pdf
    ├── Sprint-01.pdf
    ├── Sprint-02.pdf
    ├── Sprint-03.pdf
    ├── Final-Report.pdf
    ├── Presentation.pdf
    └── Product-Backlog.xlsx
```

## Getting Started

### Prerequisites
Ensure the following are installed before proceeding:
- [.NET SDK](https://dotnet.microsoft.com/download) (net9.0-windows)
- [PostgreSQL](https://www.postgresql.org/download/)
- [pgAdmin4](https://www.pgadmin.org/download/) (optional, for visual DB management)
- Visual Studio (with WPF workload)

### Database Setup
1. Install PostgreSQL and create a local database (e.g. name it `CarRentalDB`).
2. Set the `CARRENTAL_DB_CONNECTION` environment variable (see [Environment Configuration](#environment-configuration)).
3. On first run, Entity Framework Core applies the migrations in `src/Migrations/` automatically — no manual schema step needed.

### Running the Application
```bash
git clone https://github.com/moosa1ahmed-ctrl/car-rental-system.git
cd car-rental-system/src
```
1. Open `CarRental-CP317.sln` in Visual Studio.
2. Ensure `CARRENTAL_DB_CONNECTION` is set in your environment.
3. Build and run (F5).

See [`src/DEV_GUIDE.md`](src/DEV_GUIDE.md) for a file-by-file breakdown of the program flow.

## Environment Configuration
The database connection string is read from an environment variable — **never hardcoded in source**.

```
CARRENTAL_DB_CONNECTION=Host=localhost;Port=5432;Database=CarRentalDB;Username=postgres;Password=your_password_here
```

| Variable | Description |
|---|---|
| `CARRENTAL_DB_CONNECTION` | Full PostgreSQL (Npgsql) connection string. If unset, falls back to a local placeholder that will not authenticate — set your own before running. |

## Database Schema Overview
Four EF Core entities, migrated to PostgreSQL via [`src/Migrations/`](src/Migrations).

| Table | Description |
|---|---|
| `Users` | Login credentials — `AccountID` (key), `Email`, `Password` |
| `UserInformations` | Profile details linked by `AccountID` — `FirstName`, `LastName`, `CardNumber`, `Address`, `City`, `Province`, `PhoneNumber` |
| `CarEntries` | Car listings — `CarID` (key), `CarYear`, `CarMake`, `CarModel`, `City`, `Province`, `Price`, `ImageUrl`, and a `BookingIDs` array used to check availability against existing bookings |
| `Bookings` | Rental records — `BookingID` (key), `AccountID`, `CarID`, `TotalPrice`, `StartDate`, `EndDate` |

**Key Relationships**

| From | To | Relationship |
|---|---|---|
| `Users` | `UserInformations` | One user has one profile (shared `AccountID`) |
| `Users` | `Bookings` | One user has many bookings |
| `CarEntries` | `Bookings` | One car has many bookings, tracked via `BookingIDs` |

## Application Flow
```
LoginWindow
  ├── no UserInformations row for this account → UserSetup → MainWindow
  └── UserInformations exists → MainWindow
```

## Testing
No automated test suite is configured for this project. Testing during development was manual, performed sprint-by-sprint against the acceptance criteria in the sprint reports (see [`docs/`](docs)):

| Area | Example Test Cases |
|---|---|
| Account setup | Register a new account, log in with existing credentials, complete first-time profile setup |
| Car listings | Browse listings, confirm image/price/location display correctly |
| Data integrity | Confirm foreign key constraints reject orphaned bookings |

## Milestone & Sprint Deliverables
| Deliverable | Location |
|---|---|
| Milestone 01 — Project description & objectives | [`docs/Milestone-01.pdf`](docs/Milestone-01.pdf) |
| Milestone 02 — Requirements & backlog expansion | [`docs/Milestone-02.pdf`](docs/Milestone-02.pdf) |
| Sprint 01 — Implementation & initial prototype | [`docs/Sprint-01.pdf`](docs/Sprint-01.pdf) |
| Sprint 02 — Feature integration | [`docs/Sprint-02.pdf`](docs/Sprint-02.pdf) |
| Sprint 03 — System integration & delivery planning | [`docs/Sprint-03.pdf`](docs/Sprint-03.pdf) |
| Final Report | [`docs/Final-Report.pdf`](docs/Final-Report.pdf) |
| Presentation | [`docs/Presentation.pdf`](docs/Presentation.pdf) |
| Product Backlog | [`docs/Product-Backlog.xlsx`](docs/Product-Backlog.xlsx) |

## Known Issues / Limitations
- The `Bookings` entity and its relationships are fully modeled, but booking *creation* is not yet wired into the UI — this was the natural next milestone.
- No payment processing; `CardNumber` is stored as a profile field but not validated or charged against.
- No automated test suite is configured.

## Project Management
GitHub Kanban Board: *not yet set up*
Wiki: *not yet set up*

## License
This project was developed as part of the CP317 – Software Engineering course at Wilfrid Laurier University (WLU), Fall 2025. It is intended solely for academic evaluation purposes.
