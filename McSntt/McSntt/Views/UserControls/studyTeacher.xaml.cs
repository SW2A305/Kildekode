﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
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
using TextBox = System.Windows.Controls.TextBox;
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
            promoteTeam.IsEnabled = false;
            newLecture1.IsEnabled = false;

        }

        #region Methods
        private void RefreshDatagrid(DataGrid grid, ICollection<StudentMember> list)
        {
            grid.ItemsSource = null;
            grid.ItemsSource = list;
        }

        private void ClearFields()
        {
            CurrentMemberDataGrid.ItemsSource = null;
            MembersList.Clear();
            lectureDropdown.ItemsSource = null;
            promoteTeam.IsEnabled = false;
            newLecture1.IsEnabled = false;
            LectureDataClear();
            StudentCheckBoxNameChange();
            TeacherName.Width = 92;
            TeacherName.Text = "";
            teamName.Text = string.Empty;
            memberSearch.Text = string.Empty;
            Level1RadioButton.IsChecked = false;
            Level2RadioButton.IsChecked = false;
            LectureDataClear();
        }

        private void PromoteTeam(StudentMember member)
        {
            var upgradedMember = new SailClubMember();
            upgradedMember = member;
            upgradedMember.Position = SailClubMember.Positions.Member;
            upgradedMember.BoatDriver = true;
            DalLocator.SailClubMemberDal.Create(upgradedMember);
        }

        #region UpdateLecture
        private void SaveLectureInfo()
        {
            if (lectureDropdown.SelectedItem == null) { return; }
            var indexCount = 0;
            ((Lecture)lectureDropdown.SelectedItem).RopeWorksLecture = (RopeWorksCheckBox.IsChecked == true);
            ((Lecture)lectureDropdown.SelectedItem).Motor = (MotorCheckBox.IsChecked == true);
            ((Lecture)lectureDropdown.SelectedItem).Navigation = (NavigationCheckBox.IsChecked == true);
            ((Lecture)lectureDropdown.SelectedItem).Night = (NightCheckBox.IsChecked == true);
            ((Lecture)lectureDropdown.SelectedItem).Gaffelrigger = (GaffelriggerCheckBox.IsChecked == true);
            ((Lecture)lectureDropdown.SelectedItem).Drabant = (DrabantCheckBox.IsChecked == true);
            if (studentOne.IsChecked == true)
            {
                ((Lecture)lectureDropdown.SelectedItem).PresentMembers.Add(
                    ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(indexCount));
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
                if (((Lecture)lectureDropdown.SelectedItem).Navigation == true) { member.Navigation = true; }
                if (((Lecture)lectureDropdown.SelectedItem).Motor == true) { member.Motor = true; }
                if (((Lecture)lectureDropdown.SelectedItem).RopeWorksLecture == true) { member.RopeWorks = true; }
                if (((Lecture)lectureDropdown.SelectedItem).Night == true) { member.Night = true; }
                if (((Lecture)lectureDropdown.SelectedItem).Gaffelrigger == true) { member.Gaffelrigger = true; }
                if (((Lecture)lectureDropdown.SelectedItem).Drabant == true) { member.Drabant = true; }
                if (((Lecture)lectureDropdown.SelectedItem).Navigation == true) { member.Navigation = true; }
            }
        }

        private void StudentCheckBoxNameChange()
        {
            int i = 0;
            if (teamDropdown.SelectedItem == null) return;

            if (i < ((Team)teamDropdown.SelectedItem).TeamMembers.Count)
            {
                studentOne.IsEnabled = StudentChecBoxIsEnable();
                studentOne.Content = ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
            if (i < ((Team)teamDropdown.SelectedItem).TeamMembers.Count)
            {
                studentTwo.IsEnabled = StudentChecBoxIsEnable();
                studentTwo.Content = ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
            if (i < ((Team)teamDropdown.SelectedItem).TeamMembers.Count)
            {
                studentThree.IsEnabled = StudentChecBoxIsEnable();
                studentThree.Content = ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
            if (i < ((Team)teamDropdown.SelectedItem).TeamMembers.Count)
            {
                studentFour.IsEnabled = StudentChecBoxIsEnable();
                studentFour.Content = ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
            if (i < ((Team)teamDropdown.SelectedItem).TeamMembers.Count)
            {
                studentFive.IsEnabled = StudentChecBoxIsEnable();
                studentFive.Content = ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
            if (i < ((Team)teamDropdown.SelectedItem).TeamMembers.Count)
            {
                studentSix.IsEnabled = StudentChecBoxIsEnable();
                studentSix.Content = ((Team)teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
        }

        private bool StudentChecBoxIsEnable()
        {
            if (lectureDropdown.SelectedItem != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void LectureDataClear()
        {
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
        #endregion
        #endregion

        #region Events
        private void teamDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearFields();
            if (teamDropdown.SelectedItem != null)
            {
                foreach (var member in ((Team)teamDropdown.SelectedItem).TeamMembers)
                {
                    MembersList.Add(member);
                }
                CurrentMemberDataGrid.ItemsSource = CollectionViewSource.GetDefaultView(MembersList);
                teamName.Text = ((Team)teamDropdown.SelectedItem).Name;
                lectureDropdown.ItemsSource = ((Team)teamDropdown.SelectedItem).Lectures.OrderBy(lect => lect.DateOfLecture);
                promoteTeam.IsEnabled = ((Team)teamDropdown.SelectedItem).Level == Team.ClassLevel.Second;
                newLecture1.IsEnabled = true;
                if (((Team)teamDropdown.SelectedItem).Teacher != null)
                {
                    TeacherName.Width = Double.NaN;
                    TeacherName.Text = ((Team)teamDropdown.SelectedItem).Teacher.FullName;
                }
                switch (((Team)teamDropdown.SelectedItem).Level)
                {
                    case 0:
                        Level1RadioButton.IsChecked = false;
                        Level2RadioButton.IsChecked = false;
                        break;
                    case Team.ClassLevel.First:
                        Level1RadioButton.IsChecked = true;
                        break;
                    case Team.ClassLevel.Second:
                        Level2RadioButton.IsChecked = true;
                        break;
                }
            }
        }

        #region Editing of team
        private void editTeam_Checked(object sender, RoutedEventArgs e)
        {
            editTeamGrid.IsEnabled = true;
        }

        private void editTeam_Unchecked(object sender, RoutedEventArgs e)
        {
            editTeamGrid.IsEnabled = false;
        }

        private void newTeam_Click(object sender, RoutedEventArgs e)
        {
            teamDropdown.SelectedIndex = -1;
            // TODO: delete StudyMockData.TeamListGlobal and add database reference
            int currentTeamDropdownCount = StudyMockData.TeamListGlobal.Count;
            var newTeamWindow = new NewTeamWindow();
            newTeamWindow.ShowDialog();
            teamDropdown.ItemsSource = null;
            teamDropdown.ItemsSource = StudyMockData.TeamListGlobal;

            if (currentTeamDropdownCount != StudyMockData.TeamListGlobal.Count)
            {
                teamDropdown.SelectedIndex = StudyMockData.TeamListGlobal.Count - 1;
            }
        }

        private void deleteTeam_Click(object sender, RoutedEventArgs e)
        {
            if (teamDropdown.SelectedItem != null)
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
                    ((Team)teamDropdown.SelectedItem).TeamMembers.Remove(member);
                }
                else
                {
                    MessageBox.Show("" + member.FullName + " er endnu ikke klar til at få sit duelighedsbevis\nTovværksarbejde " +
                                    member.RopeWorks + "\nNavigation " + member.Navigation + "\nMotor- og brandlære " + member.Motor + "\nNatsejlads " +
                                    member.Night + "\nDrabantsejling " + member.Drabant + "\nGaffelriggersejling " + member.Gaffelrigger);
                }
            }
            // TODO: Team should probably be deleted if all members have been upgraded
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            if (MemberDataGrid.SelectedItem == null) { return; }
            var currentMember = (StudentMember) MemberDataGrid.SelectedItem;
            if (!MembersList.Contains(currentMember) && StudyMockData.TeamListGlobal.All(x => !x.TeamMembers.Contains(currentMember)))
            {
                MembersList.Add(currentMember);
                RefreshDatagrid(CurrentMemberDataGrid, MembersList);
            }
            else
            {
                MessageBox.Show("Eleven er allerede på et hold!");
            }
        }

        private void RemoveStudent_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentMemberDataGrid.SelectedItem == null) { return; }
            var currentMember = (StudentMember) CurrentMemberDataGrid.SelectedItem;
            MembersList.Remove(currentMember);
            RefreshDatagrid(CurrentMemberDataGrid, MembersList);
        }

        private void saveChanges_Click(object sender, RoutedEventArgs e)
        {
            int currentTeamDropdownIndex = teamDropdown.SelectedIndex;

            if (Level1RadioButton.IsChecked == true)
            {
                ((Team)teamDropdown.SelectedItem).Level = Team.ClassLevel.First;
            }
            else if (Level2RadioButton.IsChecked == true)
            {
                ((Team)teamDropdown.SelectedItem).Level = Team.ClassLevel.Second;
            }

            if (((Team) teamDropdown.SelectedItem).Name != teamName.Text)
            {
                // TODO: Change reference to database
                if (StudyMockData.TeamListGlobal.All(x => x.Name != teamName.Text))
                {
                    ((Team) teamDropdown.SelectedItem).Name = teamName.Text;
                }
                else
                {
                    MessageBox.Show("Et hold med dette navn eksisterer allerede!");
                }
            }

            ((Team)teamDropdown.SelectedItem).TeamMembers.Clear();
            foreach (var member in MembersList)
            {
                member.AssociatedTeam = ((Team)teamDropdown.SelectedItem);
                ((Team)teamDropdown.SelectedItem).TeamMembers.Add(member);
            }

            teamDropdown.ItemsSource = null;
            // TODO: Itemsource should be changed to database
            teamDropdown.ItemsSource = StudyMockData.TeamListGlobal;
            teamDropdown.SelectedIndex = currentTeamDropdownIndex;
        }
        #endregion

        #region Lecture
        private void lectureDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LectureDataClear();
            StudentCheckBoxNameChange();
            if (lectureDropdown.SelectedItem == null) { return; }
            if (lectureDropdown.SelectedIndex != -1)
            {
                lectureGrid.IsEnabled = true;
                NavigationCheckBox.IsChecked = ((Lecture) lectureDropdown.SelectedItem).Navigation;
                MotorCheckBox.IsChecked = ((Lecture) lectureDropdown.SelectedItem).Motor;
                GaffelriggerCheckBox.IsChecked = ((Lecture) lectureDropdown.SelectedItem).Gaffelrigger;
                NightCheckBox.IsChecked = ((Lecture) lectureDropdown.SelectedItem).Night;
                DrabantCheckBox.IsChecked = ((Lecture) lectureDropdown.SelectedItem).Drabant;
                RopeWorksCheckBox.IsChecked = ((Lecture) lectureDropdown.SelectedItem).RopeWorksLecture;
            }
            if (((Team)teamDropdown.SelectedItem).Level == Team.ClassLevel.First)
            {
                NavigationCheckBox.IsEnabled = false;
                MotorCheckBox.IsEnabled = false;
                GaffelriggerCheckBox.IsEnabled = false;
                NightCheckBox.IsEnabled = true;
                DrabantCheckBox.IsEnabled = true;
                RopeWorksCheckBox.IsEnabled = true;
            }
            else
            {
                NavigationCheckBox.IsEnabled = true;
                MotorCheckBox.IsEnabled = true;
                GaffelriggerCheckBox.IsEnabled = true;
                NightCheckBox.IsEnabled = false;
                DrabantCheckBox.IsEnabled = false;
                RopeWorksCheckBox.IsEnabled = false;
            }
        }

        private void updateLecture_Click(object sender, RoutedEventArgs e)
        {
            SaveLectureInfo();
            AssignToughtLectureItemsToMember();
        }
        
        private void newLecture1_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewLecture(teamDropdown.SelectedItem);
            window.ShowDialog();
        }

        private void DeleteLecture_Click(object sender, RoutedEventArgs e)
        {
            var lecture = ((Lecture) lectureDropdown.SelectedItem);
            LectureDataClear();
            // TODO: delete mockdata reference
            ((Team) teamDropdown.SelectedItem).Lectures.Remove(lecture);
            //DalLocator.LectureDal.Delete(lecture);
            lectureDropdown.ItemsSource = null;
            lectureDropdown.ItemsSource = ((Team)teamDropdown.SelectedItem).Lectures.OrderBy(lect => lect.DateOfLecture);
        }
        #endregion
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

        
    }
}