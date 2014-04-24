using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Globalization;
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
using McSntt.Migrations;
using McSntt.Models;

namespace McSntt.Views.UserControls
{
    /// <summary>
    ///     Interaction logic for Members.xaml
    /// </summary>ry>
    public partial class Members : UserControl, INotifyPropertyChanged
    {
        public Members()
        {
            InitializeComponent();
            //DataContext = this;

            using (var db = new McSntttContext())
            {
                #region SearchUI

                db.SailClubMembers.Load();
                DataGridCollection = CollectionViewSource.GetDefaultView(db.SailClubMembers.Local);
                DataGridCollection.Filter = new Predicate<object>(Filter);

                #endregion
            }
        }

        #region Search
        // In order to seach these must exist.
        private ICollectionView _dataGridCollection;
        private string _filterString;
        private ObservableCollection<SailClubMember> _dataGridCollection2;

        public ICollectionView DataGridCollection
        {
            get { return _dataGridCollection; }
            set { _dataGridCollection = value; NotifyPropertyChanged("DataGridCollection"); }
        }

        public ObservableCollection<SailClubMember> DataGridCollection2
        {
            get { return _dataGridCollection2; }
            set { _dataGridCollection2 = value; NotifyPropertyChanged("DataGridCollection2"); }
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

                    if ((data.Gender.Equals(Gender.Male) ? "mand" : string.Empty).Contains(lower))
                        return true;

                    if ((data.Gender.Equals(Gender.Female) ? "kvinde" : string.Empty).Contains(lower))
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

        #endregion

        private void MembersUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new McSntttContext())
            {
                db.SailClubMembers.Load();
                DataGridCollection2 = db.SailClubMembers.Local;
            }
        }
    }

}
