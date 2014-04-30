using System;
using System.Collections;
using System.Collections.Generic;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class studyTeacher : UserControl
    {

        public studyTeacher()
        {
            InitializeComponent();
            ITeamDal teamDal = new TeamEfDal();

            #region Data til test (skal slettes)
            var student1 = new SailClubMember();
            student1.FirstName = "Knold";
            var student2 = new SailClubMember();
            student2.FirstName = "Tot";
            var team2 = new Team { Name = "Hold 8" };

            team2.TeamMembers.Add(student1);
            team2.TeamMembers.Add(student2);
            
            #endregion

            
            teamDropdown.ItemsSource = teamDal.GetAll();
            teamDropdown.DisplayMemberPath = "Name";
            teamDropdown.SelectedValuePath = "TeamId";

            
            editTeamGrid.IsEnabled = false;

            

        }



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
            memberSearch.Text = string.Empty;
            level1.IsChecked = false;
            level2.IsChecked = false;
        }

        private void deleteTeam_Click(object sender, RoutedEventArgs e)
        {
            var team = (Team) teamDropdown.SelectionBoxItem;
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
                MessageBox.Show("" + ((Team) teamDropdown.SelectedItem).TeamMembers.Count);
            }
            catch (NullReferenceException ex)
            {                               
            }
            studentDropdown.DisplayMemberPath = "FirstName";
            studentDropdown.SelectedValuePath = "FirstName";

        }
    }
}