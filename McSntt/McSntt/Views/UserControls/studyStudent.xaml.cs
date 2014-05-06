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
using System.Windows.Navigation;
using System.Windows.Shapes;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.Views.UserControls
{
    /// <summary>
    /// Interaction logic for studyStudent.xaml
    /// </summary>
    public partial class studyStudent : UserControl
    {
        public studyStudent()
        {
            InitializeComponent();
            progressGrid.IsEnabled = false;
            practicalCheck1.IsChecked = ((StudentMember) GlobalInformation.CurrentUser).Drabant;
            practicalCheck2.IsChecked = ((StudentMember) GlobalInformation.CurrentUser).Gaffelrigger;
            practicalCheck3.IsChecked = ((StudentMember)GlobalInformation.CurrentUser).Night;
            theoryCheck1.IsChecked = ((StudentMember)GlobalInformation.CurrentUser).RopeWorks;
            theoryCheck2.IsChecked = ((StudentMember)GlobalInformation.CurrentUser).Navigation;
            theoryCheck3.IsChecked = ((StudentMember)GlobalInformation.CurrentUser).Motor;
            practicalCheck1.IsEnabled = false;
            practicalCheck2.IsEnabled = false;
            practicalCheck3.IsEnabled = false;
            theoryCheck1.IsEnabled = false;
            theoryCheck2.IsEnabled = false;
            theoryCheck3.IsEnabled = false;
            teamName.Text = ((StudentMember) GlobalInformation.CurrentUser).AssociatedTeam.Name;
            if (((StudentMember) GlobalInformation.CurrentUser).AssociatedTeam.Level == Team.ClassLevel.First)
            {
                level.Text = "1. års sejlerhold";
            }
            else
            {
                level.Text = "2. års sejlerhold";
            }

        }

       
    }
}
