using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using McSntt.DataAbstractionLayer;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.Views.UserControls
{
    /// <summary>
    ///     Interaction logic for Members.xaml
    /// </summary>ry>
    public partial class Members : UserControl, INotifyPropertyChanged
    {
        private ISailClubMemberDal sailClubMemberDal = DalLocator.SailClubMemberDal;

        public Members()
        {
            InitializeComponent();

            DataGridCollection = CollectionViewSource.GetDefaultView(sailClubMemberDal.GetAll());
            DataGridCollection.Filter = new Predicate<object>(Filter);
        }   

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

                    if ((data.Gender.Equals(Gender.Male) ? "mand" : string.Empty).Contains(lower))
                        return true;

                    if ((data.Gender.Equals(Gender.Female) ? "kvinde" : string.Empty).Contains(lower))
                        return true;

                    if (data.SailClubMemberId.ToString().Contains(lower))
                        return true;

                    // If none succeeds return false
                    return false;
                }
                return true;
            }
            return false;
        }

        #endregion
    }

}
