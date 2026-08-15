using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace CarRental_CP317
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e) // runs when the application starts up
        {
            using var udc = new UserDataContext();
            udc.Database.Migrate();
        }
    }

}
