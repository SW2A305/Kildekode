using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for StudentMainWindow.xaml
    /// </summary>
    public partial class StudentMainWindow : Window
    {
        public StudentMainWindow(SailClubMember activeUser)
        {
            // Set the list as the current DataContext
            InitializeComponent();

            Closing += Window_Closing;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
