using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_CP317
{
    public class UserDataContext:DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Connection string is read from an environment variable so credentials are never committed to source control.
            // Set CARRENTAL_DB_CONNECTION locally, e.g.:
            // Host=localhost;Port=5432;Database=CarRentalDB;Username=postgres;Password=your_password_here
            var connectionString = Environment.GetEnvironmentVariable("CARRENTAL_DB_CONNECTION")
                ?? "Host=localhost;Port=5432;Database=CarRentalDB;Username=postgres;Password=CHANGE_ME";
            optionsBuilder.UseNpgsql(connectionString); // PostgreSQL connection string
        }

        public DbSet<User> Users { get; set; } // initialized in User.cs
        public DbSet<UserInformation> UserInformations { get; set; } // initialized in User.cs
        public DbSet<CarEntry> CarEntries { get; set; } // initialized in CarEntry.cs
        public DbSet<Booking> Bookings { get; set; } // initialized in Booking.cs
    }
}
