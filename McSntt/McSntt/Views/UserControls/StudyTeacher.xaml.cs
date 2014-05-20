using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using McSntt.DataAbstractionLayer;
using McSntt.Helpers;
using McSntt.Models;
using McSntt.Views.Windows;

namespace McSntt.Views.UserControls
{
    /// <summary>
    ///     Interaction logic for StudyTeacher.xaml
    /// </summary>
    public partial class StudyTeacher : UserControl, INotifyPropertyChanged
    {
        public ICollection<StudentMember> MembersList = new Collection<StudentMember>();

        public StudyTeacher()
        {
            this.InitializeComponent();

            ITeamDal teamDal = DalLocator.TeamDal;
            IStudentMemberDal studentDal = DalLocator.StudentMemberDal;

            IEnumerable<Team> teams = teamDal.GetAll();
            IEnumerable<StudentMember> students = studentDal.GetAll();

            // Load extra data
            Team[] teamsArray = teams as Team[] ?? teams.ToArray();
            teamDal.LoadData(teamsArray);
            studentDal.LoadData(students);

            this.teamDropdown.ItemsSource = teamsArray;
            this.teamDropdown.DisplayMemberPath = "Name";
            this.teamDropdown.SelectedValuePath = "TeamId";

            this.lectureDropdown.DisplayMemberPath = "DateOfLectureString";
            this.lectureDropdown.SelectedValuePath = "LectureId";

            this.DataGridCollection = CollectionViewSource.GetDefaultView(students);
            this.DataGridCollection.Filter = this.Filter;

            this.editTeamGrid.IsEnabled = false;
            this.lectureGrid.IsEnabled = (this.lectureDropdown.SelectedIndex != -1);
            this.promoteTeam.IsEnabled = false;
            this.newLecture1.IsEnabled = false;
            this.DeleteLecture.IsEnabled = false;
        }

        #region Methods
        private void RefreshDatagrid(DataGrid grid, IEnumerable<StudentMember> list)
        {
            grid.ItemsSource = null;
            grid.ItemsSource = list;
        }

        private void ClearFields()
        {
            this.CurrentMemberDataGrid.ItemsSource = null;
            this.MembersList.Clear();
            this.lectureDropdown.ItemsSource = null;
            this.lectureDropdown.IsEnabled = false;
            this.promoteTeam.IsEnabled = false;
            this.newLecture1.IsEnabled = false;
            this.DeleteLecture.IsEnabled = false;
            this.StudentsProgress.IsEnabled = false;
            this.StudentCheckBoxNameChange();
            this.TeacherName.Width = 92;
            this.TeacherName.Text = "";
            this.teamName.Text = string.Empty;
            this.memberSearch.Text = string.Empty;
            this.Level1RadioButton.IsChecked = false;
            this.Level2RadioButton.IsChecked = false;
            this.LectureDataClear();
        }

        private void PromoteMember(StudentMember member)
        {
            /*
            SailClubMember upgradedMember = member.AsSailClubMember();
            upgradedMember.Position = SailClubMember.Positions.Member;
            upgradedMember.BoatDriver = true;
            DalLocator.SailClubMemberDal.Create(upgradedMember);
             */
            DalLocator.StudentMemberDal.PromoteToMember(member);
        }

        private IEnumerable GetTeams()
        {
            IEnumerable<Team> teams = DalLocator.TeamDal.GetAll();
            DalLocator.TeamDal.LoadData(teams);
            return teams;
        }

        private IEnumerable GetStudents()
        {
            IEnumerable<StudentMember> students = DalLocator.StudentMemberDal.GetAll();
            DalLocator.StudentMemberDal.LoadData(students);
            return students;
        }

        #region UpdateLecture
        private void SaveLectureInfo()
        {
            if (this.lectureDropdown.SelectedItem == null) { return; }
            int indexCount = 0;
            ((Lecture) this.lectureDropdown.SelectedItem).RopeWorksLecture = (this.RopeWorksCheckBox.IsChecked == true);
            ((Lecture) this.lectureDropdown.SelectedItem).Motor = (this.MotorCheckBox.IsChecked == true);
            ((Lecture) this.lectureDropdown.SelectedItem).Navigation = (this.NavigationCheckBox.IsChecked == true);
            ((Lecture) this.lectureDropdown.SelectedItem).Night = (this.NightCheckBox.IsChecked == true);
            ((Lecture) this.lectureDropdown.SelectedItem).Gaffelrigger = (this.GaffelriggerCheckBox.IsChecked == true);
            ((Lecture) this.lectureDropdown.SelectedItem).Drabant = (this.DrabantCheckBox.IsChecked == true);
            if (this.studentOne.IsChecked == true)
            {
                ((Lecture) this.lectureDropdown.SelectedItem).PresentMembers.Add(
                                                                                 ((Team) this.teamDropdown.SelectedItem)
                                                                                     .TeamMembers.ElementAt(indexCount));
            }
            ++indexCount;
            if (this.studentTwo.IsChecked == true)
            {
                ((Lecture) this.lectureDropdown.SelectedItem).PresentMembers.Add(
                                                                                 ((Team) this.teamDropdown.SelectedItem)
                                                                                     .TeamMembers.ElementAt(indexCount));
            }
            ++indexCount;
            if (this.studentThree.IsChecked == true)
            {
                ((Lecture) this.lectureDropdown.SelectedItem).PresentMembers.Add(
                                                                                 ((Team) this.teamDropdown.SelectedItem)
                                                                                     .TeamMembers.ElementAt(indexCount));
            }
            ++indexCount;
            if (this.studentFour.IsChecked == true)
            {
                ((Lecture) this.lectureDropdown.SelectedItem).PresentMembers.Add(
                                                                                 ((Team) this.teamDropdown.SelectedItem)
                                                                                     .TeamMembers.ElementAt(indexCount));
            }
            ++indexCount;
            if (this.studentFive.IsChecked == true)
            {
                ((Lecture) this.lectureDropdown.SelectedItem).PresentMembers.Add(
                                                                                 ((Team) this.teamDropdown.SelectedItem)
                                                                                     .TeamMembers.ElementAt(indexCount));
            }
            ++indexCount;
            if (this.studentSix.IsChecked == true)
            {
                ((Lecture) this.lectureDropdown.SelectedItem).PresentMembers.Add(
                                                                                 ((Team) this.teamDropdown.SelectedItem)
                                                                                     .TeamMembers.ElementAt(indexCount));
            }
            ++indexCount;
            DalLocator.LectureDal.Update((Lecture) this.lectureDropdown.SelectedItem);
        }

        private void AssignToughtLectureItemsToMember()
        {
            if (((Lecture) this.lectureDropdown.SelectedItem).PresentMembers == null) { return; }
            foreach (StudentMember member in ((Lecture) this.lectureDropdown.SelectedItem).PresentMembers)
            {
                if (((Lecture) this.lectureDropdown.SelectedItem).Navigation) { member.Navigation = true; }
                if (((Lecture) this.lectureDropdown.SelectedItem).Motor) { member.Motor = true; }
                if (((Lecture) this.lectureDropdown.SelectedItem).RopeWorksLecture) { member.RopeWorks = true; }
                if (((Lecture) this.lectureDropdown.SelectedItem).Night) { member.Night = true; }
                if (((Lecture) this.lectureDropdown.SelectedItem).Gaffelrigger) { member.Gaffelrigger = true; }
                if (((Lecture) this.lectureDropdown.SelectedItem).Drabant) { member.Drabant = true; }
                DalLocator.StudentMemberDal.Update(member);
            }
        }

        private void StudentCheckBoxNameChange()
        {
            int i = 0;
            if (this.teamDropdown.SelectedItem == null) { return; }
            if (((Team) this.teamDropdown.SelectedItem).TeamMembers == null) { return; }

            if (i < ((Team) this.teamDropdown.SelectedItem).TeamMembers.Count)
            {
                this.studentOne.IsEnabled = this.StudentChecBoxIsEnable();
                this.studentOne.Content = ((Team) this.teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
            if (i < ((Team) this.teamDropdown.SelectedItem).TeamMembers.Count)
            {
                this.studentTwo.IsEnabled = this.StudentChecBoxIsEnable();
                this.studentTwo.Content = ((Team) this.teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
            if (i < ((Team) this.teamDropdown.SelectedItem).TeamMembers.Count)
            {
                this.studentThree.IsEnabled = this.StudentChecBoxIsEnable();
                this.studentThree.Content = ((Team) this.teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
            if (i < ((Team) this.teamDropdown.SelectedItem).TeamMembers.Count)
            {
                this.studentFour.IsEnabled = this.StudentChecBoxIsEnable();
                this.studentFour.Content = ((Team) this.teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
            if (i < ((Team) this.teamDropdown.SelectedItem).TeamMembers.Count)
            {
                this.studentFive.IsEnabled = this.StudentChecBoxIsEnable();
                this.studentFive.Content = ((Team) this.teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
            if (i < ((Team) this.teamDropdown.SelectedItem).TeamMembers.Count)
            {
                this.studentSix.IsEnabled = this.StudentChecBoxIsEnable();
                this.studentSix.Content = ((Team) this.teamDropdown.SelectedItem).TeamMembers.ElementAt(i).FullName;
                i++;
            }
        }

        private bool StudentChecBoxIsEnable()
        {
            if (this.lectureDropdown.SelectedItem != null) { return true; }
            return false;
        }

        private void LectureDataClear()
        {
            this.studentOne.Content = "";
            this.studentOne.IsEnabled = false;
            this.studentOne.IsChecked = false;
            this.studentTwo.Content = "";
            this.studentTwo.IsEnabled = false;
            this.studentTwo.IsChecked = false;
            this.studentThree.Content = "";
            this.studentThree.IsEnabled = false;
            this.studentThree.IsChecked = false;
            this.studentFour.Content = "";
            this.studentFour.IsEnabled = false;
            this.studentFour.IsChecked = false;
            this.studentFive.Content = "";
            this.studentFive.IsEnabled = false;
            this.studentFive.IsChecked = false;
            this.studentSix.Content = "";
            this.studentSix.IsEnabled = false;
            this.studentSix.IsChecked = false;
            this.DrabantCheckBox.IsChecked = false;
            this.DrabantCheckBox.IsEnabled = false;
            this.GaffelriggerCheckBox.IsChecked = false;
            this.GaffelriggerCheckBox.IsEnabled = false;
            this.NavigationCheckBox.IsChecked = false;
            this.NavigationCheckBox.IsEnabled = false;
            this.MotorCheckBox.IsChecked = false;
            this.MotorCheckBox.IsEnabled = false;
            this.RopeWorksCheckBox.IsChecked = false;
            this.RopeWorksCheckBox.IsEnabled = false;
            this.NightCheckBox.IsChecked = false;
            this.NightCheckBox.IsEnabled = false;
        }
        #endregion

        #endregion

        #region Events
        private void teamDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ClearFields();
            this.lectureDropdown.ItemsSource = null;
            if (this.teamDropdown.SelectedItem != null)
            {
                this.editTeam.IsChecked = true;
                foreach (StudentMember member in ((Team) this.teamDropdown.SelectedItem).TeamMembers) {
                    this.MembersList.Add(member);
                }
                this.CurrentMemberDataGrid.ItemsSource = CollectionViewSource.GetDefaultView(this.MembersList);
                this.teamName.Text = ((Team) this.teamDropdown.SelectedItem).Name;
                this.lectureDropdown.ItemsSource =
                    ((Team) this.teamDropdown.SelectedItem).Lectures.OrderBy(lect => lect.DateOfLecture);
                this.promoteTeam.IsEnabled = ((Team) this.teamDropdown.SelectedItem).Level == Team.ClassLevel.Second;
                this.newLecture1.IsEnabled = true;
                this.StudentsProgress.IsEnabled = true;
                this.lectureDropdown.IsEnabled = true;
                if (((Team) this.teamDropdown.SelectedItem).Teacher != null)
                {
                    this.TeacherName.Width = Double.NaN;
                    this.TeacherName.Text = ((Team) this.teamDropdown.SelectedItem).Teacher.FullName;
                }
                switch (((Team) this.teamDropdown.SelectedItem).Level)
                {
                    case 0:
                        this.Level1RadioButton.IsChecked = false;
                        this.Level2RadioButton.IsChecked = false;
                        break;
                    case Team.ClassLevel.First:
                        this.Level1RadioButton.IsChecked = true;
                        break;
                    case Team.ClassLevel.Second:
                        this.Level2RadioButton.IsChecked = true;
                        break;
                }
            }
        }

        #region Editing of team
        private void editTeam_Checked(object sender, RoutedEventArgs e) { this.editTeamGrid.IsEnabled = true; }

        private void editTeam_Unchecked(object sender, RoutedEventArgs e) { this.editTeamGrid.IsEnabled = false; }

        private void newTeam_Click(object sender, RoutedEventArgs e)
        {
            this.teamDropdown.SelectedIndex = -1;
            int currentTeamDropdownCount = this.teamDropdown.Items.Count;
            var newTeamWindow = new NewTeamWindow();
            newTeamWindow.ShowDialog();
            this.teamDropdown.ItemsSource = null;

            this.teamDropdown.ItemsSource = this.GetTeams();

            if (currentTeamDropdownCount != this.teamDropdown.Items.Count) {
                this.teamDropdown.SelectedIndex = this.teamDropdown.Items.Count - 1;
            }
        }

        private void deleteTeam_Click(object sender, RoutedEventArgs e)
        {
            if (this.teamDropdown.SelectedItem != null)
            {
                var team = (Team) this.teamDropdown.SelectedItem;

                DalLocator.TeamDal.Delete(team);
                this.teamDropdown.ItemsSource = null;
                this.teamDropdown.ItemsSource = this.GetTeams();

                this.ClearFields();
            }
        }

        private void promoteTeam_Click(object sender, RoutedEventArgs e)
        {
            var promotelist = new List<StudentMember>();

            foreach (StudentMember member in (((Team) this.teamDropdown.SelectedItem).TeamMembers))
            {
                if (member.Night && member.Navigation &&
                    member.Motor && member.RopeWorks &&
                    member.Drabant && member.Gaffelrigger)
                {
                    promotelist.Add(member);
                }
                else
                {
                    MessageBox.Show("" + member.FullName
                                    + " er endnu ikke klar til at få sit duelighedsbevis\nTovværksarbejde " +
                                    member.RopeWorks + "\nNavigation " + member.Navigation + "\nMotor- og brandlære "
                                    + member.Motor + "\nNatsejlads " +
                                    member.Night + "\nDrabantsejling " + member.Drabant + "\nGaffelriggersejling "
                                    + member.Gaffelrigger);
                }
            }
            foreach (StudentMember studentMember in promotelist)
            {
                ((Team) this.teamDropdown.SelectedItem).TeamMembers.Remove(studentMember);
                this.PromoteMember(studentMember);
            }
            DalLocator.TeamDal.Update((Team) this.teamDropdown.SelectedItem);


            if (((Team) this.teamDropdown.SelectedItem).TeamMembers.Count == 0)
            {
                DalLocator.TeamDal.Delete((Team) this.teamDropdown.SelectedItem);
                MessageBox.Show("Alle Medlemmer på holdet er nu forfremmet, og holdet er slettet.");
            }
            else
            { MessageBox.Show("Alle elever som har mulighed for forfremmelse, er nu forfremmet."); }

            this.ClearFields();
            this.teamDropdown.ItemsSource = null;
            this.teamDropdown.ItemsSource = this.GetTeams();
            this.DataGridCollection = CollectionViewSource.GetDefaultView(this.GetStudents());
            this.DataGridCollection.Filter = this.Filter;


        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            if (this.MemberDataGrid.SelectedItem == null) { return; }
            IList selectedMembers = this.MemberDataGrid.SelectedItems;

            foreach (StudentMember selectedMember in selectedMembers)
            {
                if (!this.MembersList.Contains(selectedMember)
                    && DalLocator.StudentMemberDal.GetOne(selectedMember.StudentMemberId).AssociatedTeamId == 0)
                {
                    this.MembersList.Add(selectedMember);
                }
                else
                {
                    MessageBox.Show(String.Format("{0} er allerede på {1}!", selectedMember.FullName,
                                                  this.MembersList.Contains(selectedMember) ? "holdet" : "et hold"));
                }
            }

            this.RefreshDatagrid(this.CurrentMemberDataGrid, this.MembersList);
        }

        private void RemoveStudent_Click(object sender, RoutedEventArgs e)
        {
            if (this.CurrentMemberDataGrid.SelectedItem == null) { return; }
            var selectedMembers = CurrentMemberDataGrid.SelectedItems;

            foreach (StudentMember selectedMember in selectedMembers) {
                this.MembersList.Remove(selectedMember);
            }
            
            this.RefreshDatagrid(this.CurrentMemberDataGrid, this.MembersList);
        }

        private void saveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (this.teamDropdown.SelectedIndex == -1) { return; }
            int currentTeamDropdownIndex = this.teamDropdown.SelectedIndex;
            var currentTeam = ((Team) this.teamDropdown.SelectedItem);

            if (this.Level1RadioButton.IsChecked == true) {
                currentTeam.Level = Team.ClassLevel.First;
            }
            else if (this.Level2RadioButton.IsChecked == true) { currentTeam.Level = Team.ClassLevel.Second; }

            if (this.teamName.Text != String.Empty)
            {
                if (currentTeam.Name != this.teamName.Text)
                {
                    if (DalLocator.TeamDal.GetAll().All(x => x.Name != this.teamName.Text)) {
                        currentTeam.Name = this.teamName.Text;
                    }
                    else
                    {
                        MessageBox.Show("Et hold med dette navn eksisterer allerede!");
                    }
                }
            }

            ((Team) this.teamDropdown.SelectedItem).TeamMembers.Clear();
            foreach (StudentMember member in this.MembersList)
            {
                member.AssociatedTeam = ((Team) this.teamDropdown.SelectedItem);
                currentTeam.TeamMembers.Add(member);
            }

            DalLocator.TeamDal.Update(currentTeam);

            this.teamDropdown.ItemsSource = null;
            this.teamDropdown.ItemsSource = this.GetTeams();
            this.teamDropdown.SelectedIndex = currentTeamDropdownIndex;

            MessageBox.Show("Ændringerne er gemt.");
        }
        #endregion

        #region Lecture
        private void lectureDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.LectureDataClear();
            this.StudentCheckBoxNameChange();
            if (this.lectureDropdown.SelectedItem == null) { return; }

            this.lectureGrid.IsEnabled = true;
            this.DeleteLecture.IsEnabled = true;


            if (((Team) this.teamDropdown.SelectedItem).Level == Team.ClassLevel.First)
            {
                this.NavigationCheckBox.IsEnabled = false;
                this.MotorCheckBox.IsEnabled = false;
                this.GaffelriggerCheckBox.IsEnabled = false;
                this.NightCheckBox.IsEnabled = true;
                this.DrabantCheckBox.IsEnabled = true;
                this.RopeWorksCheckBox.IsEnabled = true;
            }
            else
            {
                this.NavigationCheckBox.IsEnabled = true;
                this.MotorCheckBox.IsEnabled = true;
                this.GaffelriggerCheckBox.IsEnabled = true;
                this.NightCheckBox.IsEnabled = false;
                this.DrabantCheckBox.IsEnabled = false;
                this.RopeWorksCheckBox.IsEnabled = false;
            }
        }

        private void updateLecture_Click(object sender, RoutedEventArgs e)
        {
            this.SaveLectureInfo();
            MessageBox.Show("Oplysninger er gemt");
            this.AssignToughtLectureItemsToMember();
        }

        private void newLecture1_Click(object sender, RoutedEventArgs e)
        {
            var window = new NewLecture(this.teamDropdown.SelectedItem);
            window.ShowDialog();
            ((Team) this.teamDropdown.SelectedItem).Lectures =
                DalLocator.LectureDal.GetAll()
                          .Where(x => x.TeamId == ((Team) this.teamDropdown.SelectedItem).TeamId)
                          .ToList();
            this.lectureDropdown.ItemsSource = null;
            this.lectureDropdown.ItemsSource =
                ((Team) this.teamDropdown.SelectedItem).Lectures.OrderBy(lect => lect.DateOfLecture);
        }

        private void DeleteLecture_Click(object sender, RoutedEventArgs e)
        {
            var team = (Team) this.teamDropdown.SelectedItem;
            var lecture = ((Lecture) this.lectureDropdown.SelectedItem);
            this.LectureDataClear();
            DalLocator.LectureDal.Delete(lecture);
            team.Lectures = null;
            team.Lectures = DalLocator.LectureDal.GetAll(x => x.TeamId == team.TeamId).ToList();
            this.lectureDropdown.ItemsSource = null;
            this.lectureDropdown.ItemsSource = team.Lectures.OrderBy(lect => lect.DateOfLecture);
        }

        private void StudentsProgress_Click(object sender, RoutedEventArgs e)
        {
            if (this.teamDropdown.SelectedItem != null)
            {
                var studentProgress = new StudentsProgressWindow(((Team) this.teamDropdown.SelectedItem));
                studentProgress.ShowDialog();
            }
            else
            {
                MessageBox.Show("Der er ikke valgt et hold");
            }
        }
        #endregion

        #endregion

        #region Search
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
            var data = obj as StudentMember;

            if (data != null && data.Position == SailClubMember.Positions.Student)
            {
                if (!string.IsNullOrEmpty(this._filterString))
                {
                    // Sanitise input to lower
                    string lower = this._filterString.ToLower();

                    // Check if either of the data points for the members match the filterstring
                    if (data.FirstName != null) { if (data.FirstName.ToLower().Contains(lower)) { return true; } }

                    if (data.LastName != null) { if (data.LastName.ToLower().Contains(lower)) { return true; } }

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
