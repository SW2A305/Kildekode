using System.Windows;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    ///     Interaction logic for StudentsProgress.xaml
    /// </summary>
    public partial class StudentsProgressWindow : Window
    {
        public StudentsProgressWindow(Team team)
        {
            this.InitializeComponent();

            this.StudentMembersDatagrid.ItemsSource = team.TeamMembers;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) { this.Close(); }
    }
}
