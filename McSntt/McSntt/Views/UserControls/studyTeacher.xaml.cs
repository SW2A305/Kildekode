using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using McSntt.DataAbstractionLayer;
using McSntt.Helpers;
using McSntt.Models;
using McSntt.Views.Windows;
using DataGrid = System.Windows.Controls.DataGrid;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace McSntt.Views.UserControls
{
    /// <summary>
    /// Interaction logic for StudyTeacher.xaml
    /// </summary>
    public partial class StudyTeacher : UserControl, INotifyPropertyChanged
    {
        #region Mock data
        public Team Team15 = new Team{Name = "Hold 15", Level = Team.ClassLevel.First, TeamMembers = new List<StudentMember>(), Lectures = new List<Lecture>()};
        public Team Team16 = new Team {Name = "Hold 16", TeamMembers = new List<StudentMember>(), Lectures = new List<Lecture>()};
        public IList<Team> TeamList = new List<Team>();
        public StudentMember Member1 = new StudentMember {FirstName = "Knold", LastName = "Jensen", SailClubMemberId = 2000};
        public StudentMember Member2 = new StudentMember { FirstName = "Tot", LastName = "Jensen", SailClubMemberId = 2001 };
        public Lecture Lecture = new Lecture{DateOfLecture = new DateTime(2014, 04, 5), PresentMembers = new List<StudentMember>()};
        #endregion

        private ILectureDal lectureDal = DalLocator.LectureDal;
        public ICollection<StudentMember> MembersList = new Collection<StudentMember>();

        public StudyTeacher()
        {
            InitializeComponent();
            var teamDal = new TeamEfDal();
            var memberDal = new StudentMemberEfDal();
           
            #region Mock data
            Team15.TeamMembers.Add(Member1);
            Team15.TeamMembers.Add(Member2);
            Team15.Lectures.Add(Lecture);
            StudyMockData.TeamListGlobal.Add(Team15);
            StudyMockData.TeamListGlobal.Add(Team16);
            teamDropdown.ItemsSource = StudyMockData.TeamListGlobal;
            #endregion

            // To be uncommented when database works
            //teamDropdown.ItemsSource = teamDal.GetAll();

            teamDropdown.DisplayMemberPath = "Name";
            teamDropdown.SelectedValuePath = "TeamId";

            lectureDropdown.DisplayMemberPath = "DateOfLecture";
            lectureDropdown.SelectedValuePath = "LectureId";

            LectureDataClear();
            
            DataGridCollection = CollectionViewSource.GetDefaultView(memberDal.GetAll());
            DataGridCollection.Filter = new Predicate<object>(Filter);
            
            editTeamGrid.IsEnabled = false;
            lectureGrid.IsEnabled = (lectureDropdown.SelectedIndex != -1);

        }

        private void RefreshDatagrid(DataGrid grid, ICollection<StudentMember> list)
        {
            grid.ItemsSource = null;
            grid.ItemsSource = list;
        }

        private void ClearFields()
        {
            teamName.Text = string.Empty;
            memberSearch.Text = string.Empty;
            Level1RadioButton.IsChecked = false;
            Level2RadioButton.IsChecked = false;
            CurrentMemberDataGrid.ItemsSource = null;
            teamDropdown.SelectedIndex = -1;
            studentDropdown.SelectedIndex = -1;
            studentDropdown.ItemsSource = null;
            DrabantCheckBox.IsChecked = false;
            GaffelriggerCheckBox.IsChecked = false;
            NavigationCheckBox.IsChecked = false;
            MotorCheckBox.IsChecked = false;
            RopeWorksCheckBox.IsChecked = false;
            NightCheckBox.IsChecked = false;
            LectureDataClear();
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
            
      //      var team = new Team { Name = teamName.Text };
            if (Level1RadioButton.IsChecked == true)
            {
                ((Team)teamDropdown.SelectedItem).Level = Team.ClassLevel.First;
            }
            else if (Level2RadioButton.IsChecked == true)
            {
                ((Team)teamDropdown.SelectedItem).Level = Team.ClassLevel.Second;
            }
            
            /* Database
            ITeamDal teamDal = new TeamEfDal();
            teamDal.Create(team);
            teamDropdown.ItemsSource = teamDal.GetAll();
                */

            #region Mock data
            /*
            StudyMockData.TeamListGlobal.Add(team);
            teamDropdown.ItemsSource = null;
            teamDropdown.ItemsSource = StudyMockData.TeamListGlobal; */

            #endregion
        }

        private void newTeam_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            var newTeamWindow = new NewTeamWindow();
            newTeamWindow.ShowDialog();
            teamDropdown.ItemsSource = null;
            teamDropdown.ItemsSource = StudyMockData.TeamListGlobal;
        }

        private void deleteTeam_Click(object sender, RoutedEventArgs e)
        {
            // The try block prevents the program from crashing if
            // you try to delete a team without having selected one
            try
            {
                var team = (Team)teamDropdown.SelectedItem;

                /*
                ITeamDal teamDal = new TeamEfDal();
                teamDal.Delete(team);
                teamDropdown.ItemsSource = teamDal.GetAll();
                 */

                #region Mock data
                StudyMockData.TeamListGlobal.Remove(team);
                teamDropdown.ItemsSource = null;
                teamDropdown.ItemsSource = StudyMockData.TeamListGlobal;
                #endregion

                ClearFields();
            }
            catch (Exception)
            {
            }
        }

        private void StudentCheckBoxNameChange()
        {
            int i = 0;
            if (teamDropdown.SelectedItem == null) return;

            if (i < ((Team)teamDropdown.SelectedItem).TeamMembers.Count)
            {
                studentOne.IsEnabled = true;
                studentOne.Content = ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
            if (i < ((Team)teamDropdown.SelectedItem).TeamMembers.Count)
            {
                studentTwo.IsEnabled = true;
                studentTwo.Content = ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
            if (i < ((Team)teamDropdown.SelectedItem).TeamMembers.Count)
            {
                studentThree.IsEnabled = true;
                studentThree.Content = ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
            if (i < ((Team)teamDropdown.SelectedItem).TeamMembers.Count)
            {
                studentFour.IsEnabled = true;
                studentFour.Content = ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
            if (i < ((Team)teamDropdown.SelectedItem).TeamMembers.Count)
            {
                studentFive.IsEnabled = true;
                studentFive.Content = ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
            if (i < ((Team)teamDropdown.SelectedItem).TeamMembers.Count)
            {
                studentSix.IsEnabled = true;
                studentSix.Content = ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
        }

        private void LectureDataClear()
        {
        /*    if ((teamDropdown.SelectedItem == null || ((Team) teamDropdown.SelectedItem).TeamMembers.Count != 0) &&
                teamDropdown.SelectedItem != null) return; */
            studentOne.Content = "";
            studentOne.IsEnabled = false;
            studentOne.IsChecked = false;
            studentTwo.Content = "";
            studentTwo.IsEnabled = false;
            studentTwo.IsChecked = false;
            studentThree.Content = "";
            studentThree.IsEnabled = false;
            studentThree.IsChecked = false;
            studentFour.Content = "";
            studentFour.IsEnabled = false;
            studentFour.IsChecked = false;
            studentFive.Content = "";
            studentFive.IsEnabled = false;
            studentFive.IsChecked = false;
            studentSix.Content = "";
            studentSix.IsEnabled = false;
            studentSix.IsChecked = false;
            DrabantCheckBox.IsChecked = false;
            DrabantCheckBox.IsEnabled = false;
            GaffelriggerCheckBox.IsChecked = false;
            GaffelriggerCheckBox.IsEnabled = false;
            NavigationCheckBox.IsChecked = false;
            NavigationCheckBox.IsEnabled = false;
            MotorCheckBox.IsChecked = false;
            MotorCheckBox.IsEnabled = false;
            RopeWorksCheckBox.IsChecked = false;
            RopeWorksCheckBox.IsEnabled = false;
            NightCheckBox.IsChecked = false;
            NightCheckBox.IsEnabled = false;
        }

        
        private void teamDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                
                studentDropdown.ItemsSource = ((Team)teamDropdown.SelectedItem).TeamMembers;
                CurrentMemberDataGrid.ItemsSource = CollectionViewSource.GetDefaultView(((Team) teamDropdown.SelectedItem).TeamMembers);
                teamName.Text = ((Team) teamDropdown.SelectedItem).Name;
                lectureDropdown.ItemsSource = ((Team) teamDropdown.SelectedItem).Lectures;

                switch (((Team)teamDropdown.SelectedItem).Level)
                {
                    case 0:
                        Level1RadioButton.IsChecked = false;
                        break;
                    case Team.ClassLevel.First:
                        Level1RadioButton.IsChecked = true;
                        break;
                    case Team.ClassLevel.Second:
                        Level2RadioButton.IsChecked = true;
                        break;
                }
                
            }
            catch (NullReferenceException)
            {
                // TODO Is this ignoring intentional? It's bad code design.
            }
            studentDropdown.DisplayMemberPath = "FirstName";
            studentDropdown.SelectedValuePath = "MemberId";

            LectureDataClear();
            StudentCheckBoxNameChange();

        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            var currentMember = (StudentMember) MemberDataGrid.SelectedItem;
            if (!((Team)teamDropdown.SelectedItem).TeamMembers.Contains(currentMember))
            {
                ((Team)teamDropdown.SelectedItem).TeamMembers.Add(currentMember);
                studentDropdown.ItemsSource = null;
                studentDropdown.ItemsSource = ((Team)teamDropdown.SelectedItem).TeamMembers;
                RefreshDatagrid(CurrentMemberDataGrid, ((Team)teamDropdown.SelectedItem).TeamMembers);
            }
            
        }

        private void RemoveStudent_Click(object sender, RoutedEventArgs e)
        {
            var currentMember = (StudentMember) CurrentMemberDataGrid.SelectedItem;
            ((Team)teamDropdown.SelectedItem).TeamMembers.Remove(currentMember);
            studentDropdown.ItemsSource = null;
            studentDropdown.ItemsSource = ((Team)teamDropdown.SelectedItem).TeamMembers;
            RefreshDatagrid(CurrentMemberDataGrid, ((Team)teamDropdown.SelectedItem).TeamMembers);
        }

        private void Level1RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (teamDropdown.SelectedItem != null)
            {
                ((Team)teamDropdown.SelectedItem).Level = Team.ClassLevel.First;
            }
        }

        private void Level2RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (teamDropdown.SelectedItem != null)
            {
                ((Team)teamDropdown.SelectedItem).Level = Team.ClassLevel.Second;
            }
        }
        /* benyttes ikke
        private void studentDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (studentDropdown.SelectedItem != null)
            {
                RopeWorksCheckBox.IsChecked = ((StudentMember) studentDropdown.SelectedItem).RopeWorks;
                NavigationCheckBox.IsChecked = ((StudentMember)studentDropdown.SelectedItem).Navigation;
                MotorCheckBox.IsChecked = ((StudentMember)studentDropdown.SelectedItem).Motor;
                DrabantCheckBox.IsChecked = ((StudentMember)studentDropdown.SelectedItem).Drabant;
                GaffelriggerCheckBox.IsChecked = ((StudentMember)studentDropdown.SelectedItem).Gaffelrigger;
                NightCheckBox.IsChecked = ((StudentMember)studentDropdown.SelectedItem).Night;
                
            }
            else
            {
                RopeWorksCheckBox.IsChecked = false;
                NavigationCheckBox.IsChecked = false;
                MotorCheckBox.IsChecked = false;
                DrabantCheckBox.IsChecked = false;
                GaffelriggerCheckBox.IsChecked = false;
                NightCheckBox.IsChecked = false;
            }
        }
        */

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

        #region updateLecture
        private void SaveLectureInfo()
        {
            if(lectureDropdown.SelectedItem == null){ return; }
            var indexCount = 0;
            ((Lecture) lectureDropdown.SelectedItem).RopeWorksLecture = (RopeWorksCheckBox.IsChecked == true);
            ((Lecture)lectureDropdown.SelectedItem).Motor = (MotorCheckBox.IsChecked == true);
            ((Lecture)lectureDropdown.SelectedItem).Navigation = (NavigationCheckBox.IsChecked == true);
            ((Lecture)lectureDropdown.SelectedItem).Night = (NightCheckBox.IsChecked == true);
            ((Lecture)lectureDropdown.SelectedItem).Gaffelrigger = (GaffelriggerCheckBox.IsChecked == true);
            ((Lecture)lectureDropdown.SelectedItem).Drabant = (DrabantCheckBox.IsChecked == true);
            if (studentOne.IsChecked == true)
            {
                ((Lecture) lectureDropdown.SelectedItem).PresentMembers.Add(
                    ((Team) teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount));
            }
            ++indexCount;
            if (studentTwo.IsChecked == true)
            {
                ((Lecture)lectureDropdown.SelectedItem).PresentMembers.Add(
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount));
            }
            ++indexCount;
            if (studentThree.IsChecked == true)
            {
                ((Lecture)lectureDropdown.SelectedItem).PresentMembers.Add(
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount));
            }
            ++indexCount;
            if (studentFour.IsChecked == true)
            {
                ((Lecture)lectureDropdown.SelectedItem).PresentMembers.Add(
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount));
            }
            ++indexCount;
            if (studentFive.IsChecked == true)
            {
                ((Lecture)lectureDropdown.SelectedItem).PresentMembers.Add(
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount));
            }
            ++indexCount;
            if (studentSix.IsChecked == true)
            {
                ((Lecture)lectureDropdown.SelectedItem).PresentMembers.Add(
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount));
            }
            ++indexCount;
        }

        private void AssignToughtLectureItemsToMember()
        {
            foreach (var member in ((Lecture)lectureDropdown.SelectedItem).PresentMembers)
            {
                if (((Lecture)lectureDropdown.SelectedItem).Navigation == true){ member.Navigation = true; }
                if (((Lecture)lectureDropdown.SelectedItem).Motor == true) { member.Motor = true; }
                if (((Lecture)lectureDropdown.SelectedItem).RopeWorksLecture == true) { member.RopeWorks = true; }
                if (((Lecture)lectureDropdown.SelectedItem).Night == true) { member.Night = true; }
                if (((Lecture)lectureDropdown.SelectedItem).Gaffelrigger == true) { member.Gaffelrigger = true; }
                if (((Lecture)lectureDropdown.SelectedItem).Drabant == true) { member.Drabant = true; }
                if (((Lecture)lectureDropdown.SelectedItem).Navigation == true) { member.Navigation = true; }
            }
        }

        private void updateLecture_Click(object sender, RoutedEventArgs e)
        {
            SaveLectureInfo();
            AssignToughtLectureItemsToMember();
        }
        #endregion

        private void newLecture1_Click(object sender, RoutedEventArgs e)
        {
            
            var window = new NewLecture(teamDropdown.SelectedItem);
            window.ShowDialog();
        }

        private void lectureDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LectureDataClear();
            StudentCheckBoxNameChange();
            if (lectureDropdown.SelectedItem == null) {return;}
            if(lectureDropdown.SelectedIndex != -1)
            {
                lectureGrid.IsEnabled = true;
            }
            if (((Team) teamDropdown.SelectedItem).Level == Team.ClassLevel.First)
            {
                NavigationCheckBox.IsEnabled = false;
                MotorCheckBox.IsEnabled = false;
                GaffelriggerCheckBox.IsEnabled = false;
                RopeWorksCheckBox.IsEnabled = true;
                DrabantCheckBox.IsEnabled = true;
                NightCheckBox.IsEnabled = true;
            }
            else
            {
                NavigationCheckBox.IsEnabled = true;
                MotorCheckBox.IsEnabled = true;
                GaffelriggerCheckBox.IsEnabled = true;
                NightCheckBox.IsEnabled = true;
                DrabantCheckBox.IsEnabled = false;
                RopeWorksCheckBox.IsEnabled = false;
            }
        }

        private void promoteTeam_Click(object sender, RoutedEventArgs e)
        {
            foreach (var member in (((Team)teamDropdown.SelectedItem).TeamMembers))
            {
                if (member.Night == true && member.Navigation == true &&
                    member.Motor == true && member.RopeWorks == true &&
                    member.Drabant == true && member.Gaffelrigger == true)
                {
                    PromoteTeam(member);
                }
                else
                {
                    MessageBox.Show("" + member.FullName + " er endnu ikke klar til at få sit duelighedsbevis\nTovværksarbejde " +
                                    member.RopeWorks + "\nNavigation " + member.Navigation + "\nMotor- og brandlære " + member.Motor + "\nNatsejlads " +
                                    member.Night + "\nDrabantsejling " + member.Drabant + "\nGaffelriggersejling " + member.Gaffelrigger);
                }
            }
        }

        private void PromoteTeam(StudentMember member)
        {
            var upgradedMember = new SailClubMember();
            upgradedMember = member;
            DalLocator.SailClubMemberDal.Create(upgradedMember);
        }

        

        

        





    }
}
