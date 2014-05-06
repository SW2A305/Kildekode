
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using McSntt.DataAbstractionLayer;
using McSntt.Helpers;
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
            UsernameBox.Focusable = true;
            FocusManager.SetFocusedElement(LoginBox, UsernameBox);
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
                    if (usr.PasswordHash == EncryptionHelper.Sha256(PasswordBox.Password))
                    {
                        LoginCompleted(usr);
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

        private void LoginCompleted(SailClubMember p)
        {
            GlobalInformation.CurrentUser = p;

            switch (p.Position)
            {
                case SailClubMember.Positions.Admin:
                    var adminWindow = new AdminMainWindow(this);
                    adminWindow.Show();
                    adminWindow.Owner = this.Owner;
                    break;
                case SailClubMember.Positions.Member:
                    var memberWindow = new MemberMainWindow(this);
                    memberWindow.Show();
                    memberWindow.Owner = this.Owner;
                    break;
                case SailClubMember.Positions.Student:
                    var studentWindow = new StudentMainWindow(this);
                    studentWindow.Show();
                    studentWindow.Owner = this.Owner;
                    break;
                case SailClubMember.Positions.Teacher:
                    var teacherWindow = new AdminMainWindow(this);
                    teacherWindow.Show();
                    teacherWindow.Owner = this.Owner;
                    break;
                case SailClubMember.Positions.SupportMember:
                    var supportWindow = new GuestMainWindow(this);
                    supportWindow.Show();
                    supportWindow.Owner = this.Owner;
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

        private void GuestLogin_OnClick(object sender, RoutedEventArgs e)
        {

            GlobalInformation.CurrentUser = new SailClubMember()
            {
                FirstName = "Gæst"
            };

            var GuestWindow = new GuestMainWindow(this);
            GuestWindow.Show();
            GuestWindow.Owner = this.Owner;
            this.Hide();
        }
    }
}
