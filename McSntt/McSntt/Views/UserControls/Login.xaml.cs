using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
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

            // Clear all error messages.
            StatusTextBlock.Text = "Forsøger at logge dig ind.";
            StatusTextBlock.Foreground = new SolidColorBrush(Colors.Green);

            using (var db = new McSntttContext())
            {
                db.SailClubMembers.Load();

                try
                {
                    if (db.SailClubMembers != null)
                    {
                        SailClubMember usr = db.SailClubMembers.Local.FirstOrDefault(x => String.Equals(x.Username, UsernameBox.Text, StringComparison.CurrentCultureIgnoreCase));

                        // Check if user exists (Case insensitive)
                        if (usr != null && String.Equals(usr.Username, UsernameBox.Text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            // Check if the password is correct (Case sensitive)
                            if (usr.PasswordHash == EncryptionHelper.Sha256(PasswordBox.Password))
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
                catch (NullReferenceException exception)
                {
                    StatusTextBlock.Text = "Bruger ikke fundet" + exception;
                }
            }
        }

        private void LoginCompleted(SailClubMember.Positions p)
        {
            switch (p)
            {
                case SailClubMember.Positions.Admin:
                    break;
                case SailClubMember.Positions.Member:
                    break;
                case SailClubMember.Positions.Student:
                    break;
                case SailClubMember.Positions.Teacher:
                    break;
                case SailClubMember.Positions.SupportMember:
                    break;
            }
        }

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
