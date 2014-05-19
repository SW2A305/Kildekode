using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    ///     Interaction logic for NewTeamWindow.xaml
    /// </summary>
    public partial class NewTeamWindow : Window
    {
        public NewTeamWindow()
        {
            this.InitializeComponent();
            this.TeamName.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (DalLocator.TeamDal.GetAll().All(x => x.Name != this.TeamName.Text))
            {
                if (this.TeamName.Text != String.Empty)
                {
                    var team = new Team {Name = this.TeamName.Text, Teacher = GlobalInformation.CurrentUser};
                    DalLocator.TeamDal.Create(team);
                    this.Close();
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Et hold med dette navn eksisterer allerede!");
            }
        }

        private void TeamName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) { this.OkButton_Click(sender, e); }
        }
    }
}
