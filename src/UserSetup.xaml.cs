using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarRental_CP317
{
    /// <summary>
    /// Interaction logic for UserSetup.xaml
    /// </summary>
    public partial class UserSetup : Window
    {
        public UserSetup()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (FirstNameTextBox.Text != null && LastNameTextBox.Text != null && PhoneNumTextBox.Text != null && CityTextBox.Text != null && AddressTextBox.Text != null && ProvinceComboBox.SelectedIndex != -1)
            {
                using (UserDataContext context = new UserDataContext())
                {
                    var newUserInfo = new UserInformation
                    {
                        AccountID = GlobalVariables.accountID,
                        FirstName = FirstNameTextBox.Text,
                        LastName = LastNameTextBox.Text,
                        PhoneNumber = PhoneNumTextBox.Text,
                        City = CityTextBox.Text,
                        Address = AddressTextBox.Text,
                        Province = ProvinceComboBox.SelectedValue as string ?? ProvinceComboBox.Text,
                        CardNumber = !string.IsNullOrWhiteSpace(CreditCardNumTextBox.Text) ? CreditCardNumTextBox.Text : null
                    };
                    GlobalVariables.loggedIn = true;
                    context.UserInformations.Add(newUserInfo);
                    context.SaveChanges();
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
            }
        }
    }
}
