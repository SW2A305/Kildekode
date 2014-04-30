
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using McSntt.DataAbstractionLayer;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            // This is to ensure that the contens of the database is ready for when the login is performed.
            var sailClubMembers = new SailClubMemberEfDal().GetAll();
        }

        #region Login
        private void DoLogin(object sender, RoutedEventArgs e)
        {
            // If usernamebox is empty display an error message and change cursor focus.
            if (UsernameBox.Text == "")
            {
                UsernameBox.Focusable = true;
                FocusManager.SetFocusedElement(LoginBox, UsernameBox);
                StatusTextBlock.Text = "Indtast et brugernavn";
                StatusTextBlock.Foreground = new SolidColorBrush(Colors.Red);

                return;
            }

            // If passwordbox is empty display an error message and change cursor focus.
            if (PasswordBox.Password == "")
            {
                PasswordBox.Focusable = true;
                FocusManager.SetFocusedElement(LoginBox, PasswordBox);
                StatusTextBlock.Text = "Indtast et kodeord";
                StatusTextBlock.Foreground = new SolidColorBrush(Colors.Red);

                return;
            }

            var sailClubMembers = new SailClubMemberEfDal().GetAll();

            if (sailClubMembers != null)
            {
                SailClubMember usr =
                    sailClubMembers.FirstOrDefault(
                        x => String.Equals(x.Username, UsernameBox.Text, StringComparison.CurrentCultureIgnoreCase));

                // Check if user exists (Case insensitive)
                if (usr != null &&
                    String.Equals(usr.Username, UsernameBox.Text, StringComparison.CurrentCultureIgnoreCase))
                {
                    // Check if the password is correct (Case sensitive)
                    if (usr.PasswordHash == Sha256(PasswordBox.Password))
                    {
                        StatusTextBlock.Text = "Velkommen, " + usr.FirstName + "!";
                        StatusTextBlock.Foreground = new SolidColorBrush(Colors.Green);

                        LoginCompleted(usr.Position);
                    }
                    else
                    {
                        StatusTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                        StatusTextBlock.Text = "Forkert kodeord";
                    }
                }
                else
                {
                    StatusTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                    StatusTextBlock.Text = "Forkert Brugernavn";
                }
            }
        }


        // This method will return a Sha256 hash as a string given a string as input. 
        // http://stackoverflow.com/questions/12416249/hashing-a-string-with-sha256
        static string Sha256(string password)
        {
            var crypt = new SHA256Managed();
            var hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
            return crypto.Aggregate(hash, (current, bit) => current + bit.ToString("x2"));
        }

        private void LoginCompleted(SailClubMember.Positions p)
        {
            switch (p)
            {
                case SailClubMember.Positions.Admin:
                    var adminWindow = new AdminMainWindow();
                    adminWindow.Show();
                    adminWindow.Owner = this.Owner;
                    break;
                case SailClubMember.Positions.Member:
                    var memberWindow = new MemberMainWindow();
                    memberWindow.Show();
                    memberWindow.Owner = this.Owner;
                    break;
                case SailClubMember.Positions.Student:
                    break;
                case SailClubMember.Positions.Teacher:
                    break;
                case SailClubMember.Positions.SupportMember:
                    break;
            }
            this.Hide();

        }

        // If you press the enter key while in one of the textboxes, do login.
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                DoLogin(sender, e);
            }
        }
        #endregion
    }

}
