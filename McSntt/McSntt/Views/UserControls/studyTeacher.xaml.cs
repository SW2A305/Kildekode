using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using McSntt.DataAbstractionLayer;
using McSntt.Models;

namespace McSntt.Views.UserControls
{
    /// <summary>
    /// Interaction logic for StudyTeacher.xaml
    /// </summary>
    public partial class StudyTeacher : UserControl, INotifyPropertyChanged
    {
        public StudyTeacher()
        {
            InitializeComponent();
            var teamDal = new TeamEfDal();
            var memberDal = new SailClubMemberEfDal();
            
            teamDropdown.ItemsSource = teamDal.GetAll();
            teamDropdown.DisplayMemberPath = "Name";
            teamDropdown.SelectedValuePath = "TeamId";
            
            using (var db = new McSntttContext())
            {
                #region SearchUI

                db.SailClubMembers.Load();
                DataGridCollection = CollectionViewSource.GetDefaultView(db.SailClubMembers.Local);
                DataGridCollection.Filter = new Predicate<object>(Filter);

                #endregion
            }

           editTeamGrid.IsEnabled = false;
        }
        
        #region Events

        private void editTeam_Checked(object sender, RoutedEventArgs e)
        {
            editTeamGrid.IsEnabled = true;
        }

        private void editTeam_Unchecked(object sender, RoutedEventArgs e)
        {
            editTeamGrid.IsEnabled = false;
        }

        private void saveChanges_Click(object sender, RoutedEventArgs e)
        {
            var team = new Team { Name = teamName.Text };
            ITeamDal teamDal = new TeamEfDal();
            teamDal.Create(team);
            teamDropdown.ItemsSource = teamDal.GetAll();
        }

        private void newTeam_Click(object sender, RoutedEventArgs e)
        {
            teamName.Text = string.Empty;
          //  memberSearch.Text = string.Empty;
            level1.IsChecked = false;
            level2.IsChecked = false;
        }

        private void deleteTeam_Click(object sender, RoutedEventArgs e)
        {
            var team = (Team)teamDropdown.SelectionBoxItem;
            ITeamDal teamDal = new TeamEfDal();
            teamDal.Delete(team);
            teamDropdown.ItemsSource = teamDal.GetAll();
        }

        private void teamDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                studentDropdown.ItemsSource = ((Team)teamDropdown.SelectedItem).TeamMembers;
                //Messagebox for test purpose, list seemingly does not exist in database
                MessageBox.Show("" + ((Team)teamDropdown.SelectedItem).TeamMembers.Count);
            }
            catch (NullReferenceException ex)
            {
            }
            studentDropdown.DisplayMemberPath = "FirstName";
            studentDropdown.SelectedValuePath = "FirstName";
        }

        #endregion
        

        #region Search
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
            get
            {
                return _filterString;
            }
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

            if (data != null && data.Position == SailClubMember.Positions.Student)
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
    }
}
