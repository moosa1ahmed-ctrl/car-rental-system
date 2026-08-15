using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTextBox.Text;
            var password = PasswordTextBox.Password;

            using (UserDataContext context = new UserDataContext())
            {
               var userFound = context.Users.FirstOrDefault(user => user.Email == email && user.Password == password); // searches if a User with the entered email and password exists
                if (userFound != null)
                {
                    GlobalVariables.accountID = userFound.AccountID;
                    var userInfo = context.UserInformations.Find(GlobalVariables.accountID);
                    // checks if selected User has UserInformation row. if not, opens the UserSetup window.
                    if (userInfo == null)
                    {
                        OpenSetupWindow();
                    }
                    else
                    {
                        GlobalVariables.loggedIn = true;
                        OpenMainWindow();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid email or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }    
            }
        }

        public void OpenMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        public void OpenSetupWindow()
        {
            UserSetup setupWindow = new UserSetup();
            setupWindow.Show();
            this.Close();
        }

        private void NoAccountButton_Click(object sender, RoutedEventArgs e)
        {
            LoginCanvas.Visibility = Visibility.Hidden;
            SignupCanvas.Visibility = Visibility.Visible;
        }

        private void BackButtonSignup_Click(object sender, RoutedEventArgs e)
        {
            LoginCanvas.Visibility = Visibility.Visible;
            SignupCanvas.Visibility = Visibility.Hidden;
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            if(EmailTextBoxSignup.Text.Contains("@") && PasswordTextBoxSignup.Password == PasswordConfirmTextBoxSignup.Password && PasswordTextBoxSignup.Password.Length > 0)
            {
                using (UserDataContext context = new UserDataContext())
                {
                    var newUser = new User // creates new User row
                    {
                        Email = EmailTextBoxSignup.Text,
                        Password = PasswordTextBoxSignup.Password
                    };
                    context.Users.Add(newUser); // adds new User row to Users table
                    context.SaveChanges();
                }
                MessageBox.Show("Account created successfully! You can now log in.", "Signup Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                LoginCanvas.Visibility = Visibility.Visible;
                SignupCanvas.Visibility = Visibility.Hidden;
            }
            else if (PasswordTextBoxSignup.Password.Length == 0)
            {
                MessageBox.Show("You must enter a password.", "Signup Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if(PasswordTextBoxSignup.Password != PasswordConfirmTextBoxSignup.Password)
            {
                MessageBox.Show("The entered passwords do not match.", "Signup Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (EmailTextBoxSignup.Text.Contains("@") == false)
            {
                MessageBox.Show("The entered email is not valid.", "Signup Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
