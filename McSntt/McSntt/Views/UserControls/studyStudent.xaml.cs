using System;
using System.Linq;
using System.Windows.Controls;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.Views.UserControls
{
    /// <summary>
    ///     Interaction logic for StudyStudent.xaml
    /// </summary>
    public partial class StudyStudent : UserControl
    {
        public StudyStudent()
        {
            this.InitializeComponent();
            this.progressGrid.IsEnabled = false;
            this.practicalCheck1.IsChecked = GlobalInformation.CurrentStudentMember.Drabant;
            this.practicalCheck2.IsChecked = GlobalInformation.CurrentStudentMember.Gaffelrigger;
            this.practicalCheck3.IsChecked = GlobalInformation.CurrentStudentMember.Night;
            this.theoryCheck1.IsChecked = GlobalInformation.CurrentStudentMember.RopeWorks;
            this.theoryCheck2.IsChecked = GlobalInformation.CurrentStudentMember.Navigation;
            this.theoryCheck3.IsChecked = GlobalInformation.CurrentStudentMember.Motor;
            this.practicalCheck1.IsEnabled = false;
            this.practicalCheck2.IsEnabled = false;
            this.practicalCheck3.IsEnabled = false;
            this.theoryCheck1.IsEnabled = false;
            this.theoryCheck2.IsEnabled = false;
            this.theoryCheck3.IsEnabled = false;

            DalLocator.StudentMemberDal.LoadData(GlobalInformation.CurrentStudentMember);
            DalLocator.TeamDal.LoadData(GlobalInformation.CurrentStudentMember.AssociatedTeam);

            this.teamName.Text = GlobalInformation.CurrentStudentMember.AssociatedTeam.Name;
            this.level.Text = GlobalInformation.CurrentStudentMember.AssociatedTeam.Level == Team.ClassLevel.First
                                  ? "1. års sejlerhold"
                                  : "2. års sejlerhold";
            this.nextSessionDate.Text = "" +
                                        (GlobalInformation.CurrentStudentMember.AssociatedTeam.Lectures.OrderBy(
                                                                                                                lect =>
                                                                                                                lect
                                                                                                                    .DateOfLecture))
                                            .FirstOrDefault(lect => lect.DateOfLecture > DateTime.Now)
                                            .DateOfLectureString;
        }
    }
}
