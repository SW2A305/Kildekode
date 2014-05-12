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
    /// Interaction logic for StudyStudent.xaml
    /// </summary>
    public partial class StudyStudent : UserControl
    {
        public StudyStudent()
        {
            InitializeComponent();
            progressGrid.IsEnabled = false;
            practicalCheck1.IsChecked = GlobalInformation.CurrentStudentMember.Drabant;
            practicalCheck2.IsChecked = GlobalInformation.CurrentStudentMember.Gaffelrigger;
            practicalCheck3.IsChecked = GlobalInformation.CurrentStudentMember.Night;
            theoryCheck1.IsChecked = GlobalInformation.CurrentStudentMember.RopeWorks;
            theoryCheck2.IsChecked = GlobalInformation.CurrentStudentMember.Navigation;
            theoryCheck3.IsChecked = GlobalInformation.CurrentStudentMember.Motor;
            practicalCheck1.IsEnabled = false;
            practicalCheck2.IsEnabled = false;
            practicalCheck3.IsEnabled = false;
            theoryCheck1.IsEnabled = false;
            theoryCheck2.IsEnabled = false;
            theoryCheck3.IsEnabled = false;

            DalLocator.StudentMemberDal.LoadData(GlobalInformation.CurrentStudentMember);
            DalLocator.TeamDal.LoadData(GlobalInformation.CurrentStudentMember.AssociatedTeam);

            teamName.Text = GlobalInformation.CurrentStudentMember.AssociatedTeam.Name;
            level.Text = GlobalInformation.CurrentStudentMember.AssociatedTeam.Level == Team.ClassLevel.First ? "1. års sejlerhold" : "2. års sejlerhold";
            nextSessionDate.Text = "" +
                                   (GlobalInformation.CurrentStudentMember.AssociatedTeam.Lectures.OrderBy(
                                       lect => lect.DateOfLecture)).FirstOrDefault(lect => lect.DateOfLecture > DateTime.Now).DateOfLectureString;

        }

       
    }
}
