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
    /// </summary>
    /// ry>
    public partial class Members : UserControl, INotifyPropertyChanged
    {
        private readonly ISailClubMemberDal sailClubMemberDal = DalLocator.SailClubMemberDal;

        public Members()
        {
            this.InitializeComponent();

            this.DataGridCollection = CollectionViewSource.GetDefaultView(this.sailClubMemberDal.GetAll());
            this.DataGridCollection.Filter = this.Filter;
        }

        #region Search
        // In order to seach these must exist.
        private ICollectionView _dataGridCollection;
        private string _filterString;

        public ICollectionView DataGridCollection
        {
            get { return this._dataGridCollection; }
            set
            {
                this._dataGridCollection = value;
                this.NotifyPropertyChanged("DataGridCollection");
            }
        }

        public string FilterString
        {
            get { return this._filterString; }
            set
            {
                this._filterString = value;
                this.NotifyPropertyChanged("FilterString");
                this.FilterCollection();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (this.PropertyChanged != null) { this.PropertyChanged(this, new PropertyChangedEventArgs(property)); }
        }

        private void FilterCollection()
        {
            if (this._dataGridCollection != null) { this._dataGridCollection.Refresh(); }
        }

        public bool Filter(object obj)
        {
            var data = obj as SailClubMember;
            if (data != null)
            {
                if (!string.IsNullOrEmpty(this._filterString))
                {
                    // Sanitise input to lower
                    string lower = this._filterString.ToLower();

                    // Check if either of the data points for the members match the filterstring
                    if (data.FirstName != null) { if (data.FirstName.ToLower().Contains(lower)) { return true; } }

                    if (data.LastName != null) { if (data.LastName.ToLower().Contains(lower)) { return true; } }

                    if (data.Postcode != null) { if (data.Postcode.Contains(lower)) { return true; } }

                    if (data.Username != null) { if (data.Username.ToLower().Contains(lower)) { return true; } }

                    if (data.Cityname != null) { if (data.Cityname.ToLower().Contains(lower)) { return true; } }

                    if (data.Email != null) { if (data.Email.ToLower().Contains(lower)) { return true; } }

                    if (data.PhoneNumber != null) { if (data.PhoneNumber.Contains(lower)) { return true; } }

                    if ((data.Gender.Equals(Gender.Male) ? "mand" : string.Empty).Contains(lower)) { return true; }

                    if ((data.Gender.Equals(Gender.Female) ? "kvinde" : string.Empty).Contains(lower)) { return true; }

                    if (data.SailClubMemberId.ToString().Contains(lower)) { return true; }

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
