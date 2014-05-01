using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using McSntt.DataAbstractionLayer;
using McSntt.Models;

namespace McSntt.Views.UserControls
{
    /// <summary>
    ///     Interaction logic for Boats.xaml
    /// </summary>
    /// ry>
    public partial class Boats : UserControl
    {
        public Boat CurrentBoat = new Boat();
        
        private IList<SailTrip> ListOfTrips = new List<SailTrip>();

        private RegularTrip RegularSailTrip1 = new RegularTrip();
        private RegularTrip RegularSailTrip2 = new RegularTrip();
        private RegularTrip RegularSailTrip3 = new RegularTrip();

        private Logbook Regularlogbook1 = new Logbook();
        private Logbook Regularlogbook2 = new Logbook();
        private Logbook Regularlogbook3 = new Logbook();
        public Boats()
        {
            InitializeComponent();
            /*var db = new BoatEfDal();
            db.GetAll(); */
            //Mock Boat
            var båd1 = new Boat() {NickName = "Bodil1", Operational = true};
            var båd2 = new Boat() {NickName = "båd"};
            var båd3 = new Boat() {NickName = "SS", ImagePath = "pack://application:,,,/Images/SundetLogo.PNG"};
            var boatList = new List<Boat>();

            boatList.Add(båd1);
            boatList.Add(båd2);
            boatList.Add(båd3);

            BoatComboBox.ItemsSource = boatList;
            BoatComboBox.DisplayMemberPath = "NickName";

            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("pack://application:,,,/Images/gray.PNG");
            image.EndInit();
            BoatImage.Source = image;

            var person1 = new Person { FirstName = "Knold", LastName = "Tot", PersonId = 0 };
            var person2 = new Person { FirstName = "Son", LastName = "Goku", PersonId = 1 };
            var person3 = new Person { FirstName = "Sponge", LastName = "Bob", PersonId = 2 };
            var testlist = new List<Person> { person1, person2, person3 };

            Regularlogbook1 = new Logbook
            {
                ActualArrivalTime = new DateTime(2014, 09, 9, 12, 0, 0),
                ActualCrew = testlist,
                ActualDepartureTime = new DateTime(2014, 08, 9, 12, 0, 0),
                FiledBy = (SailClubMember) RegularSailTrip1.Captain,
                DamageInflicted = true,
                DamageDescription = "Det gik gal!",
                AnswerFromBoatChief = "Det sku ikk for godt!"
            };
            Regularlogbook2 = new Logbook
            {
                ActualArrivalTime = new DateTime(2014, 09, 9, 12, 0, 0),
                ActualCrew = testlist,
                ActualDepartureTime = new DateTime(2014, 08, 9, 12, 0, 0),
                FiledBy = (SailClubMember)RegularSailTrip1.Captain,
                DamageInflicted = false,
            };
            Regularlogbook3 = new Logbook
            {
                ActualArrivalTime = new DateTime(2014, 09, 9, 12, 0, 0),
                ActualCrew = testlist,
                ActualDepartureTime = new DateTime(2014, 08, 9, 12, 0, 0),
                FiledBy = (SailClubMember)RegularSailTrip1.Captain,
                DamageInflicted = true,
                DamageDescription = "Det gik gal på det vand!!",
                AnswerFromBoatChief = "Det sku ikk for godt!"
            };

            RegularSailTrip1 = new RegularTrip
            {
                Boat = båd1,
                ArrivalTime = new DateTime(2014, 09, 9, 12, 0, 0),
                BoatId = 1,
                Captain = person3,
                Comments = "Det blir sjaw!",
                DepartureTime = new DateTime(2014, 09, 9, 09, 0, 0),
                PurposeAndArea = "u' ti' æ ' van' og' hjem' ien...",
                WeatherConditions = "Det 'en bæt' wind...",
                RegularTripId = 9,
                Crew = testlist,
                Logbook = Regularlogbook1

            };

            RegularSailTrip2 = new RegularTrip
            {
                Boat = båd1,
                ArrivalTime = new DateTime(2014, 09, 9, 12, 0, 0),
                BoatId = 1,
                Captain = person3,
                Comments = "Det blir skæg!",
                DepartureTime = new DateTime(2014, 09, 9, 09, 0, 0),
                PurposeAndArea = "u' ti' æ ' van' og' hjem' ien...",
                WeatherConditions = "Det 'en bæt' wind...",
                RegularTripId = 9,
                Crew = testlist,
                Logbook = Regularlogbook2

            };

            RegularSailTrip3 = new RegularTrip
            {
                Boat = båd1,
                ArrivalTime = new DateTime(2014, 09, 9, 12, 0, 0),
                BoatId = 1,
                Captain = person3,
                Comments = "Det blir slet ikke spur sjaw!!",
                DepartureTime = new DateTime(2014, 09, 9, 09, 0, 0),
                PurposeAndArea = "u' ti' æ ' van' og' hjem' ien...",
                WeatherConditions = "Det 'en bæt' wind...",
                RegularTripId = 9,
                Crew = testlist,
                Logbook = Regularlogbook3

            };
            ListOfTrips.Add(RegularSailTrip1);
            ListOfTrips.Add(RegularSailTrip2);
            ListOfTrips.Add(RegularSailTrip3);

        }


        private void BoatComboBox_OnSelectionChanged(object sender, EventArgs e)
        {
            CurrentBoat = (Boat)BoatComboBox.SelectionBoxItem;
            NameLabel.Content = CurrentBoat.NickName;
            if (CurrentBoat.ImagePath != null)
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(CurrentBoat.ImagePath);
                image.EndInit();
                BoatImage.Source = image;
            }
            else
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri("pack://application:,,,/Images/gray.PNG");
                image.EndInit();
                BoatImage.Source = image; 
            }
            string operationel = "";
            if (CurrentBoat.Operational)
            {
                operationel = "Operationel";
            }
            else if (!CurrentBoat.Operational)
            {
                operationel = "Ikke operationel";
            }

            LogbookListBox.ItemsSource = ListOfTrips;

            BoatTypeTextBox.Text = Enum.GetName(typeof(BoatType), CurrentBoat.Type);
            BoatStatusTextBox.Text = operationel;
        }
    }
}