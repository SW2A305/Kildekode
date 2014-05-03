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
using McSntt.DataAbstractionLayer;
using McSntt.Models;
using McSntt.Views.Windows;

namespace McSntt.Views.UserControls
{
    /// <summary>
    /// Interaction logic for FrontPage.xaml
    /// </summary>
    public partial class FrontPage : UserControl
    {
        public FrontPage()
            : this(new SailClubMember())
        {
            
        }
        public FrontPage(SailClubMember p)
        {
            InitializeComponent();

            WelcomeBlock.Text = string.Format("Velkommen {0}!", p.FullName);
            InfoTextBlock.Text =
                "Til højre ses dine kommende ture, samt dem hvorpå der endnu ikker er udfyldt en logbog for.";

            LoadData();
        }

        public void LoadData()
        {
            /*RegularSailTrip = sailTrip;*/
            
            // TEST PERSONER

            var person1 = new Person { FirstName = "Knold", LastName = "Tot", PersonId = 0 };
            var person2 = new Person { FirstName = "Son", LastName = "Goku", PersonId = 1 };
            var person3 = new Person { FirstName = "Sponge", LastName = "Bob", PersonId = 2 };
            var testlist = new List<Person> { person1, person2, person3 };

            RegularSailTrip = new RegularTrip
            {
                Boat = new Boat() { NickName = "Bodil2" },
                ArrivalTime = new DateTime(2014, 09, 9, 12, 0, 0),
                BoatId = 1,
                Captain = person3,
                Comments = "Det blir sjaw!",
                DepartureTime = new DateTime(2014, 09, 9, 09, 0, 0),
                PurposeAndArea = "u' ti' æ ' van' og' hjem' ien...",
                WeatherConditions = "Det 'en bæt' wind...",
                RegularTripId = 9,
                Crew = testlist

            };

            RegularSailTrip2 = new RegularTrip
            {
                Boat = new Boat() { NickName = "Bodil2" },
                ArrivalTime = new DateTime(2014, 03, 9, 12, 0, 0),
                BoatId = 1,
                Captain = person3,
                Comments = "Det blir sjaw!",
                DepartureTime = new DateTime(2014, 03, 9, 09, 0, 0),
                PurposeAndArea = "u' ti' æ ' van' og' hjem' ien...",
                WeatherConditions = "Det 'en bæt' wind...",
                RegularTripId = 9,
                Crew = testlist
            };

            var SailTripList = new List<RegularTrip>();
            SailTripList.Add(RegularSailTrip);
            SailTripList.Add(RegularSailTrip2);

            UpcommingTripsDataGrid.ItemsSource = null;
            UpcommingTripsDataGrid.ItemsSource = SailTripList.Where(t => t.DepartureTime > DateTime.Now);

            LogbookDataGrid.ItemsSource = null;
            LogbookDataGrid.ItemsSource = SailTripList.Where(t => t.ArrivalTime < DateTime.Now && t.Logbook == null);
        }

        private RegularTrip RegularSailTrip = new RegularTrip();
        private RegularTrip RegularSailTrip2 = new RegularTrip();

        private void LogbookDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var LogBookWindow = new CreateLogbookWindow();
            LogBookWindow.ShowDialog();

            /* TODO: after database.
            LogbookDataGrid.ItemsSource = null;
            LogbookDataGrid.ItemsSource = SailTripList.Where(t => t.ArrivalTime < DateTime.Now && t.Logbook == null); */
        }

        private void UpcommingTripsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //TODO: Show info about up and comming trip
        }
    }
}
