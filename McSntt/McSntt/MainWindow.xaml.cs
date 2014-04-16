using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using McSntt.Models;

namespace McSntt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            // Set the list as the current DataContext
            InitializeComponent();

            using (var db = new McSntttContext())
            {
                #region SearchInMain

                db.SailClubMembers.Load();
                DataGridCollection = CollectionViewSource.GetDefaultView( db.SailClubMembers.Local);
                DataGridCollection.Filter = new Predicate<object>(Filter);
                #endregion
            }
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
                        SailClubMember usr = db.SailClubMembers.Local.FirstOrDefault(x => x.Username.ToLower() == UsernameBox.Text.ToLower());

                        // Check if user exists (Case insensitive)
                        if (usr != null && usr.Username == UsernameBox.Text)
                        {
                            // Check if the password is correct (Case sensitive)
                            if (usr.PasswordHash == Sha256(PasswordBox.Password))
                            {
                                StatusTextBlock.Text = "Velkommen, " + usr.FirstName + "!";
                                StatusTextBlock.Foreground = new SolidColorBrush(Colors.Green);

                                LoginCompleted();
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
                    StatusTextBlock.Text = "Bruger ikke fundet" + exception.ToString();
                }
            }
        }

        public void LoginCompleted()
        {
            
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                DoLogin(sender, e);
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

        #endregion

        #region Search
        // In order to seach these must exist.
        private ICollectionView _dataGridCollection;
        private string _filterString;

        public ICollectionView DataGridCollection
        {
            get { return _dataGridCollection; }
            set { _dataGridCollection = value; NotifyPropertyChanged("DataGridCollection"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public string FilterString
        {
            get { return _filterString; }
            set
            {
                _filterString = value;
                NotifyPropertyChanged("FilterString");
                FilterCollection();
            }
        }

        private void FilterCollection()
        {
            if (_dataGridCollection != null)
            {
                _dataGridCollection.Refresh();
            }
        }

        public bool Filter(object obj)
        {
            var data = obj as SailClubMember;
            if (data != null)
            {
                if (!string.IsNullOrEmpty(_filterString))
                {
                    // Sanitise input to lower
                    var lower = _filterString.ToLower();

                    // Check if either of the data points for the members match the filterstring
                    if (data.FirstName != null)
                        if (data.FirstName.ToLower().Contains(lower))
                            return true;

                    if (data.LastName != null)
                        if (data.LastName.ToLower().Contains(lower))
                            return true;

                    if (data.Postcode != null)
                        if (data.Postcode.Contains(lower))
                            return true;

                    if (data.Username != null)
                        if (data.Username.ToLower().Contains(lower))
                            return true;

                    if (data.Cityname != null)
                        if (data.Cityname.ToLower().Contains(lower))
                            return true;

                    if (data.Email != null)
                        if (data.Email.ToLower().Contains(lower))
                            return true;

                    if (data.PhoneNumber != null)
                        if (data.PhoneNumber.Contains(lower))
                            return true;

                    if ((data.Gender.Equals(Gender.Male) ? "male" : string.Empty).Contains(lower))
                        return true;

                    if ((data.Gender.Equals(Gender.Female) ? "female" : string.Empty).Contains(lower))
                        return true;

                    if (data.MemberId.ToString().Contains(lower))
                        return true;

                    // If none succeeds return false
                    return false;
                }
                return true;
            }
            return false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        #endregion
    }
}
