using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for AdminMainWindow.xaml
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        public AdminMainWindow()
        {
            // Set the list as the current DataContext
            InitializeComponent();

            Closing += Window_Closing;
        }
        public AdminMainWindow(SailClubMember activeUser)
        {
            // Set the list as the current DataContext
            InitializeComponent();

            ThisUser = activeUser;

            Closing += Window_Closing;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public SailClubMember ThisUser { get; set; }
    }
}
