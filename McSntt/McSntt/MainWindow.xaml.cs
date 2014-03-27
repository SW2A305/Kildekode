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
using McSntt.Models;

namespace McSntt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Create testmember in order to display data. 
            var testMember = new SailClubMember
            {
                FirstName = "Troels",
                LastName = "Kroegh",
                Address = "Scoresbysundvej 8",
                Postcode = "9210",
                Cityname = "Aalborg SØ",
                Gender = Gender.Male,
                MemberId = 1337,
                Position = Positions.Admin,
                DateOfBirth = new DateTime(1994,06,13)
            };

            // Put members into a list
            var listOfMembers = new List<SailClubMember>();
            listOfMembers.Add(testMember);
            listOfMembers.Add(testMember);

            // Set the list as the current DataContext
            this.DataContext = listOfMembers;

            InitializeComponent();
        }

        // Exit application if the shutdown button is pressed.
        private void ExitButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
