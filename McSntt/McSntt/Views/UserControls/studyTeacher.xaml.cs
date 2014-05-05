﻿using System;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using McSntt.DataAbstractionLayer;
using McSntt.Models;
using McSntt.Views.Windows;

namespace McSntt.Views.UserControls
{
    /// <summary>
    /// Interaction logic for StudyTeacher.xaml
    /// </summary>
    public partial class StudyTeacher : UserControl, INotifyPropertyChanged
    {
        #region Til test
        public Team Team15 = new Team{Name = "Hold 15", Level = Team.ClassLevel.First, TeamMembers = new List<StudentMember>(), Lectures = new List<Lecture>()};
        public Team Team16 = new Team {Name = "Hold 16", TeamMembers = new List<StudentMember>(), Lectures = new List<Lecture>()};
        public IList<Team> TeamList = new List<Team>();
        public StudentMember Member1 = new StudentMember {FirstName = "Knold", LastName = "Jensen", SailClubMemberId = 2000};
        public StudentMember Member2 = new StudentMember { FirstName = "Tot", LastName = "Jensen", SailClubMemberId = 2001 };
        public Lecture Lecture = new Lecture{DateOfLecture = new DateTime(2014, 04, 5)};
        #endregion

        public ICollection<StudentMember> MembersList = new Collection<StudentMember>();

        public StudyTeacher()
        {
            InitializeComponent();
            var teamDal = new TeamEfDal();
            var memberDal = new StudentMemberEfDal();
           
            #region Til test
            Team15.TeamMembers.Add(Member1);
            Team15.TeamMembers.Add(Member2);
            TeamList.Add(Team15);
            TeamList.Add(Team16);
            Team15.Lectures.Add(Lecture);

            teamDropdown.ItemsSource = TeamList;
            lectureDropdown.ItemsSource = Team15.Lectures;
            #endregion

            // To be uncommented when database works
            //teamDropdown.ItemsSource = teamDal.GetAll();

            teamDropdown.DisplayMemberPath = "Name";
            teamDropdown.SelectedValuePath = "TeamId";

            lectureDropdown.DisplayMemberPath = "DateOfLecture";
            lectureDropdown.SelectedValuePath = "LectureId";
            
            
            DataGridCollection = CollectionViewSource.GetDefaultView(memberDal.GetAll());
            DataGridCollection.Filter = new Predicate<object>(Filter);
            
            editTeamGrid.IsEnabled = false;
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
            ClearFields();
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

                #region Til test
                TeamList.Remove(team);
                teamDropdown.ItemsSource = null;
                teamDropdown.ItemsSource = TeamList;
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

        private void StudentCheckBoxNameReset()
        {
            if ((teamDropdown.SelectedItem == null || ((Team) teamDropdown.SelectedItem).TeamMembers.Count != 0) &&
                teamDropdown.SelectedItem != null) return;
            studentOne.Content = "";
            studentOne.IsEnabled = false;
            studentTwo.Content = "";
            studentTwo.IsEnabled = false;
            studentThree.Content = "";
            studentThree.IsEnabled = false;
            studentFour.Content = "";
            studentFour.IsEnabled = false;
            studentFive.Content = "";
            studentFive.IsEnabled = false;
            studentSix.Content = "";
            studentSix.IsEnabled = false;
        }
        
        private void teamDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                studentDropdown.ItemsSource = ((Team)teamDropdown.SelectedItem).TeamMembers;
                //Messagebox for test purpose, list seemingly does not exist in database
                //MessageBox.Show("" + ((Team)teamDropdown.SelectedItem).TeamMembers.Count);
                this.MembersList = ((Team) teamDropdown.SelectedItem).TeamMembers;
                CurrentMemberDataGrid.ItemsSource = CollectionViewSource.GetDefaultView(this.MembersList);
                teamName.Text = ((Team) teamDropdown.SelectedItem).Name;
                
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

            StudentCheckBoxNameReset();
            StudentCheckBoxNameChange();

        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            var currentMember = (StudentMember) MemberDataGrid.SelectedItem;
            if (!this.MembersList.Contains(currentMember))
            {
                this.MembersList.Add(currentMember);
                studentDropdown.ItemsSource = null;
                studentDropdown.ItemsSource = ((Team)teamDropdown.SelectedItem).TeamMembers;
                RefreshDatagrid(CurrentMemberDataGrid, this.MembersList);
            }
            
        }

        private void RemoveStudent_Click(object sender, RoutedEventArgs e)
        {
            var currentMember = (StudentMember) CurrentMemberDataGrid.SelectedItem;
            this.MembersList.Remove(currentMember);
            studentDropdown.ItemsSource = null;
            studentDropdown.ItemsSource = ((Team)teamDropdown.SelectedItem).TeamMembers;
            RefreshDatagrid(CurrentMemberDataGrid, this.MembersList);
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
        /*
        #region Check/Uncheck

        private void RobeWorksCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (studentDropdown.SelectedItem != null)
            {
                ((StudentMember) studentDropdown.SelectedItem).RobeWorks = true;
            }
            else
            {
                RobeWorksCheckBox.IsChecked = false;
            }
        }

        private void RobeWorksCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (studentDropdown.SelectedItem != null)
            {
                ((StudentMember) studentDropdown.SelectedItem).RobeWorks = false;
            }
        }

        private void NavigationCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (studentDropdown.SelectedItem != null)
            {
                ((StudentMember)studentDropdown.SelectedItem).Navigation = true;
            }
            else
            {
                NavigationCheckBox.IsChecked = false;
            }
        }

        private void NavigationCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (studentDropdown.SelectedItem != null)
            {
                ((StudentMember)studentDropdown.SelectedItem).Navigation = false;
            }
        }

        private void MotorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (studentDropdown.SelectedItem != null)
            {
                ((StudentMember)studentDropdown.SelectedItem).Motor = true;
            }
            else
            {
                MotorCheckBox.IsChecked = false;
            }
        }

        private void MotorCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (studentDropdown.SelectedItem != null)
            {
                ((StudentMember)studentDropdown.SelectedItem).Motor = false;
            }
        }

        private void DrabantCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (studentDropdown.SelectedItem != null)
            {
                ((StudentMember)studentDropdown.SelectedItem).Drabant = true;
            }
            else
            {
                DrabantCheckBox.IsChecked = false;
            }
        }

        private void DrabantCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (studentDropdown.SelectedItem != null)
            {
                ((StudentMember)studentDropdown.SelectedItem).Drabant = false;
            }
        }

        private void GaffelriggerCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (studentDropdown.SelectedItem != null)
            {
                ((StudentMember)studentDropdown.SelectedItem).Gaffelrigger = true;
            }
            else
            {
                GaffelriggerCheckBox.IsChecked = false;
            }
        }

        private void GaffelriggerCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (studentDropdown.SelectedItem != null)
            {
                ((StudentMember)studentDropdown.SelectedItem).Gaffelrigger = false;
            }
        }

        private void NightCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (studentDropdown.SelectedItem != null)
            {
                ((StudentMember)studentDropdown.SelectedItem).Night = true;
            }
            else
            {
                NightCheckBox.IsChecked = false;
            }
        }

#endregion
         */ // Har udkommenteret dette da det ikke virker så godt med lektioner men kan eventuelt bruges til at se individuel progress, vi snakker om det mandag

        private void SaveLectureInfo(object sender, RoutedEventArgs e)
        {
            var indexCount = 0;
            ((Lecture) lectureDropdown.SelectedItem).RopeWorksLecture = (RopeWorksCheckBox.IsChecked == true);
            ((Lecture)lectureDropdown.SelectedItem).Motor = (MotorCheckBox.IsChecked == true);
            ((Lecture)lectureDropdown.SelectedItem).Navigation = (NavigationCheckBox.IsChecked == true);
            ((Lecture)lectureDropdown.SelectedItem).Night = (NightCheckBox.IsChecked == true);
            ((Lecture)lectureDropdown.SelectedItem).Gaffelrigger = (GaffelriggerCheckBox.IsChecked == true);
            ((Lecture)lectureDropdown.SelectedItem).Drabant = (DrabantCheckBox.IsChecked == true);
            if ((string) this.studentOne.Content != "Elev 1" && studentOne.IsChecked == true)
            {
                ((Lecture) lectureDropdown.SelectedItem).PresentMembers.Add(
                    ((Team) teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount));

                if (NavigationCheckBox.IsChecked == true)
                {
                    ((Team) teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Navigation = true;
                }
                if (RopeWorksCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).RopeWorks = true;
                }
                if (MotorCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Motor = true;
                }
                if (DrabantCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Drabant = true;
                }
                if (GaffelriggerCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Drabant = true;
                }
                if (NightCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Night = true;
                }
            }
            ++indexCount;
            if ((string) this.studentTwo.Content != "Elev 2" && studentTwo.IsChecked == true)
            {
                ((Lecture)lectureDropdown.SelectedItem).PresentMembers.Add(
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount));

                if (NavigationCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Navigation = true;
                }
                if (RopeWorksCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).RopeWorks = true;
                }
                if (MotorCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Motor = true;
                }
                if (DrabantCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Drabant = true;
                }
                if (GaffelriggerCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Drabant = true;
                }
                if (NightCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Night = true;
                }
            }
            ++indexCount;
            if ((string) this.studentThree.Content != "Elev 3" && studentThree.IsChecked == true)
            {
                ((Lecture)lectureDropdown.SelectedItem).PresentMembers.Add(
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount));

                if (NavigationCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Navigation = true;
                }
                if (RopeWorksCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).RopeWorks = true;
                }
                if (MotorCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Motor = true;
                }
                if (DrabantCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Drabant = true;
                }
                if (GaffelriggerCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Drabant = true;
                }
                if (NightCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Night = true;
                }
            }
            ++indexCount;
            if ((string) this.studentFour.Content != "Elev 4" && studentFour.IsChecked == true)
            {
                ((Lecture)lectureDropdown.SelectedItem).PresentMembers.Add(
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount));

                if (NavigationCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Navigation = true;
                }
                if (RopeWorksCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).RopeWorks = true;
                }
                if (MotorCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Motor = true;
                }
                if (DrabantCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Drabant = true;
                }
                if (GaffelriggerCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Drabant = true;
                }
                if (NightCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Night = true;
                }
            }
            ++indexCount;
            if ((string) this.studentFive.Content != "Elev 5" && studentFive.IsChecked == true)
            {
                ((Lecture)lectureDropdown.SelectedItem).PresentMembers.Add(
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount));

                if (NavigationCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Navigation = true;
                }
                if (RopeWorksCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).RopeWorks = true;
                }
                if (MotorCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Motor = true;
                }
                if (DrabantCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Drabant = true;
                }
                if (GaffelriggerCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Drabant = true;
                }
                if (NightCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Night = true;
                }
            }
            ++indexCount;
            if ((string) this.studentSix.Content != "Elev 6" && studentSix.IsChecked == true)
            {
                ((Lecture)lectureDropdown.SelectedItem).PresentMembers.Add(
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount));

                if (NavigationCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Navigation = true;
                }
                if (RopeWorksCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).RopeWorks = true;
                }
                if (MotorCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Motor = true;
                }
                if (DrabantCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Drabant = true;
                }
                if (GaffelriggerCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Drabant = true;
                }
                if (NightCheckBox.IsChecked == true)
                {
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount).Night = true;
                }
            }
            ++indexCount;
        }

        private void updateLecture_Click(object sender, RoutedEventArgs e)
        {
            SaveLectureInfo(sender, e);
        }

        private void newLecture1_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewLecture();
            window.ShowDialog();
        }

        

        

        





    }
}
