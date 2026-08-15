using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CarRental_CP317
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            using (UserDataContext context = new UserDataContext())
            {
                var userInfo = context.UserInformations.Find(GlobalVariables.accountID);
                if (userInfo != null)
                {
                    TopBarAccountName.Content = $"{userInfo.FirstName} {userInfo.LastName}";
                }
            }
        }

        private double aspectRatio = 1.77777778;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            aspectRatio = this.ActualWidth / this.ActualHeight;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            if (sizeInfo.WidthChanged)
            {
                this.Width = sizeInfo.NewSize.Height * aspectRatio;
            }
            else
            {
                this.Height = sizeInfo.NewSize.Width * aspectRatio;
            }
        }

        private void TopBarLogoutButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.loggedIn = false;
            GlobalVariables.accountID = -1;
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        public static List<CarEntry> GetAllCarEntries()
        {
            using (var db = new UserDataContext())
            {
                if(db.CarEntries != null)
                {
                    return db.CarEntries.ToList();
                }
                return null;
            }
        }

        // opens the default search when the rentals button is clicked
        private void TopBarRentalsButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("BrowseRentals");
            RentalBrowserCanvas.Visibility = Visibility.Visible;

            //List<CarEntry> carEntries = GetAllCarEntries();

            ProvinceComboBox.Text = string.Empty;
            CityTextBox.Text = string.Empty;
            MakeTextBox.Text = string.Empty;
            ModelTextBox.Text = string.Empty;
            YearTextBox.Text = string.Empty;
            StartDate.SelectedDate = null;
            EndDate.SelectedDate = null;

            RentalsListPanel.Children.Clear();

            /*
            if (carEntries == null)
            {
                System.Diagnostics.Debug.WriteLine("Error: CarEntries table is null!");
                return;
            }
            else
            {
                RentalsListPanel.Children.Clear();
                if (carEntries.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("No cars found in the database!");
                }
            }
            */
        }

        // creates the canvas box that is used in the search results list
        private Canvas CreateRentalCanvas(string carName, string price, int carID)
        {
            var canvas = new Canvas
            {
                Width = 1100,
                Height = 150,
                Background = new SolidColorBrush(Color.FromRgb(26, 26, 26)), // #FF1A1A1A
                Margin = new Thickness(0, 20, 0, 0),
                Tag = carID
            };

            // White image placeholder rectangle
            var rect = new Rectangle
            {
                Width = 219,
                Height = 118,
                Fill = Brushes.White
            };
            Canvas.SetLeft(rect, 16);
            Canvas.SetTop(rect, 16);
            canvas.Children.Add(rect);

            // Car name label
            var nameLabel = new Label
            {
                Content = carName,
                FontSize = 38,
                FontWeight = FontWeights.SemiBold,
                Foreground = Brushes.White,
                Width = 580,
                Height = 71,
                Background = Brushes.Transparent,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            Canvas.SetLeft(nameLabel, 258);
            Canvas.SetTop(nameLabel, 13);
            canvas.Children.Add(nameLabel);

            // Price label
            var priceLabel = new Label
            {
                Content = price,
                FontSize = 38,
                FontWeight = FontWeights.SemiBold,
                Foreground = Brushes.White,
                Width = 164,
                Height = 71,
                Background = Brushes.Transparent,
                HorizontalContentAlignment = HorizontalAlignment.Right,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            Canvas.SetLeft(priceLabel, 906);
            Canvas.SetTop(priceLabel, 13);
            canvas.Children.Add(priceLabel);

            // Transparent overlay button (to capture clicks)
            var overlay = new Button
            {
                Width = 1100,
                Height = 150,
                Background = Brushes.Transparent,
                BorderBrush = null,
                Opacity = 0,
                Cursor = Cursors.Hand,
                ToolTip = $"View {carName}"
            };
            Canvas.SetLeft(overlay, 0);
            Canvas.SetTop(overlay, 0);
            overlay.Click += (s, e) =>
            {
                MessageBox.Show($"Clicked: {carName} - {price}", "Rental Click");
            };
            canvas.Children.Add(overlay);

            return canvas;
        }

        // validates that only numbers are entered in the year textbox
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private bool IsCarEntryBooked(CarEntry entry, DateTime StartDate, DateTime EndDate) // checks if a car entry is booked for the given date range
        {
            System.Diagnostics.Debug.WriteLine($"CarID: {entry.CarID} Booking Count: {entry.BookingIDs.Length.ToString()}");

                int j = 0;
                bool isBooked = false;
                while (isBooked == false && j < entry.BookingIDs.Length)
                {
                    using (var db = new UserDataContext())
                    {
                        var booking = db.Bookings.AsNoTracking().FirstOrDefault(b => b.BookingID == entry.BookingIDs[j]);
                        if (booking != null)
                        {
                            DateTime bookingStart = booking.StartDate;
                            DateTime bookingEnd = booking.EndDate;
                            if ((DateTime.Compare(StartDate, bookingEnd) > 0) || (DateTime.Compare(EndDate, bookingStart) < 0)) // if the startdate is after the booking end date or the enddate is before the booking start date
                            {
                                // no date overlap, car is available for this booking
                                System.Diagnostics.Debug.WriteLine($"CarID: {entry.CarID} BookingID: {booking.BookingID} No Overlap");
                            }
                            else
                            {
                                // date overlap, car is booked for this booking
                                System.Diagnostics.Debug.WriteLine($"CarID: {entry.CarID} BookingID: {booking.BookingID} Overlap");
                                isBooked = true;
                            }
                        }
                    }
                    j++;
                }
                return isBooked;
        }


        // search logic
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartDate.SelectedDate != null && EndDate.SelectedDate != null)
            {
                if (DateTime.Compare(StartDate.SelectedDate.Value, EndDate.SelectedDate.Value) <= 0) // check if start date is before or same as end date
                {
                    DateTime UserStartDate = StartDate.SelectedDate.Value;
                    DateTime UserEndDate = EndDate.SelectedDate.Value;

                    List<CarEntry> carEntries = GetAllCarEntries();

                    RentalsListPanel.Children.Clear();

                    if (carEntries == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Error: CarEntries table is null!");
                        return;
                    }
                    else if ((ProvinceComboBox.SelectedIndex <= 0) && string.IsNullOrWhiteSpace(CityTextBox.Text) && string.IsNullOrWhiteSpace(MakeTextBox.Text) && string.IsNullOrWhiteSpace(ModelTextBox.Text) && string.IsNullOrWhiteSpace(YearTextBox.Text)) // if all search filters are empty
                    {
                        for (int i = 0; i < carEntries.Count; i++)
                        {
                            CarEntry entry = carEntries[i];
                            var canvas = CreateRentalCanvas($"{entry.CarYear} {entry.CarMake} {entry.CarModel}", $"${entry.Price}", entry.CarID);

                            if (entry.BookingIDs.Length > 0) // if there are bookings for this car
                            {
                                bool isBooked = IsCarEntryBooked(entry, UserStartDate, UserEndDate); // checks if car is booked for the given date range
                                if (isBooked == false)
                                {
                                    RentalsListPanel.Children.Add(canvas);
                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"CarID: {entry.CarID}, Make: {entry.CarMake}, Model: {entry.CarModel}, Year: {entry.CarYear}, Price: {entry.Price}");
                                RentalsListPanel.Children.Add(canvas);
                            }
                            if (carEntries.Count == 0)
                            {
                                System.Diagnostics.Debug.WriteLine("No cars found in the database!");
                            }
                        }
                    }
                    else // if at least one search filter is set
                    {
                        using (var db = new UserDataContext()) // use every filter to narrow down the search
                        {

                            IQueryable<CarEntry> query = db.CarEntries.AsNoTracking();

                            if (!string.IsNullOrWhiteSpace(ProvinceComboBox.Text))
                                query = query.Where(c => c.Province == ProvinceComboBox.Text);

                            if (!string.IsNullOrWhiteSpace(CityTextBox.Text))
                                query = query.Where(c => c.City == CityTextBox.Text);

                            if (!string.IsNullOrWhiteSpace(MakeTextBox.Text))
                                query = query.Where(c => c.CarMake == MakeTextBox.Text);

                            if (!string.IsNullOrWhiteSpace(ModelTextBox.Text))
                                query = query.Where(c => c.CarModel == ModelTextBox.Text);

                            if (int.TryParse(YearTextBox.Text, out int yearInt))
                                query = query.Where(c => c.CarYear == yearInt);

                            carEntries = query.OrderBy(c => c.CarID).ToList();
                        }

                        for (int i = 0; i < carEntries.Count; i++)
                        {
                            CarEntry entry = carEntries[i];
                            var canvas = CreateRentalCanvas($"{entry.CarYear} {entry.CarMake} {entry.CarModel}", $"${entry.Price}", entry.CarID);
                            System.Diagnostics.Debug.WriteLine($"CarID: {entry.CarID} Booking Count: {entry.BookingIDs.ToString()}");

                            if (entry.BookingIDs.Length > 0) // if there are bookings for this car
                            {
                                bool isBooked = IsCarEntryBooked(entry, UserStartDate, UserEndDate); // checks if car is booked for the given date range
                                if (isBooked == false)
                                {
                                    RentalsListPanel.Children.Add(canvas);
                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"CarID: {entry.CarID}, Make: {entry.CarMake}, Model: {entry.CarModel}, Year: {entry.CarYear}, Price: {entry.Price}");
                                RentalsListPanel.Children.Add(canvas);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Start Date must be before or the same as End Date.", "Search Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Start Time or End Time is not set.", "Search Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
        }
    }
}