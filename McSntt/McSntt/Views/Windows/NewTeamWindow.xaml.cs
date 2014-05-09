using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for NewTeamWindow.xaml
    /// </summary>
    public partial class NewTeamWindow : Window
    {
        public NewTeamWindow()
        {
            InitializeComponent();
            TeamName.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (DalLocator.TeamDal.GetAll().All(x => x.Name != TeamName.Text) )
            {
                if (!string.IsNullOrEmpty(TeamName.Text))
                {
                    // TODO: The instansiating of the lists can be remove when the database works
                    var team = new Team { Name = TeamName.Text, Teacher = GlobalInformation.CurrentUser };
                    DalLocator.TeamDal.Create();
                    Close();
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
            if (e.Key == Key.Return)
            {
                OkButton_Click(sender, e);
            }
        }
    }
}
