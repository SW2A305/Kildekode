using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using McSntt.Models;
using McSntt.Views.Windows;
using MessageBox = System.Windows.MessageBox;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for CreateLogbookWindow.xaml
    /// </summary>
    public partial class CreateLogbookWindow : Window
    {
        public IList<Person> CrewList = new List<Person>();
        private RegularTrip RegularSailTrip = new RegularTrip();
        private Logbook currentLogbook = new Logbook();

        private readonly DateTime _hasBeenFilledTime = new DateTime();

        public CreateLogbookWindow(/*RegularTrip sailTrip*/)
        {
            InitializeComponent();
            
            /*RegularSailTrip = sailTrip;*/

            // TEST PERSONER

            var person1 = new Person {FirstName = "Knold", LastName = "Tot", PersonId = 0};
            var person2 = new Person { FirstName = "Son", LastName = "Goku", PersonId = 1 };
            var person3 = new Person {FirstName = "Sponge", LastName = "Bob", PersonId = 2};
            var testlist = new List<Person> {person1, person2, person3};

            RegularSailTrip = new RegularTrip
            {
                Boat = new Boat() {NickName = "Bodil2"},
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
            CrewList = RegularSailTrip.Crew;
            CrewDataGrid.ItemsSource = CrewList;
            PurposeTextBox.Text = RegularSailTrip.PurposeAndArea;
            BoatTextBox.Text = RegularSailTrip.Boat.NickName;
            DateTimePickerPlannedDepature.Value = RegularSailTrip.DepartureTime;
            DateTimePickerPlannedArrival.Value = RegularSailTrip.ArrivalTime;
            CaptainComboBox.DisplayMemberPath = "FullName";
            CaptainComboBox.ItemsSource = CrewList;
            CaptainComboBox.SelectedValue = RegularSailTrip.Captain;
            DateTimePickerActualArrival.Value = DateTime.Now;
            DateTimePickerActualDeparture.Value = DateTime.Now;
            _hasBeenFilledTime = DateTime.Now;

        }
        
        private void ChangeCrewButtonClick(object sender, RoutedEventArgs e)
        {
            var createCrewWindow = new CreateCrewWindow(CrewList);
            createCrewWindow.ShowDialog();


            CrewList = createCrewWindow.CrewList;

            CrewDataGrid.ItemsSource = null;
            CrewDataGrid.ItemsSource = CrewList;

            if (!CrewList.Contains((Person) CaptainComboBox.SelectedValue))
            {
                CaptainComboBox.SelectedValue = -1;
            }
            CaptainComboBox.ItemsSource = null;
            CaptainComboBox.ItemsSource = CrewList;
        }

        private void FileLogbookButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (YesRadioButton.IsChecked == false && NoRadioButton.IsChecked == false)
            {
                MessageBox.Show("Udfyld venligst om båden blev skadet under sejladsen");
            }
            else if ((DateTimePickerActualArrival.Value == _hasBeenFilledTime ||
                     DateTimePickerActualDeparture.Value == _hasBeenFilledTime))
            {
                MessageBox.Show("Ændre venligst din faktiske afgang og/eller faktiske ankomst fra defaultværdien");
            }
            else if (YesRadioButton.IsChecked == true && DamageTextBox.Text == String.Empty)
            {
                MessageBox.Show("Udfyld venligst skadesrapporten med en beskrivelse af skaden");
            }
            else if (!CrewList.Contains((Person)CaptainComboBox.SelectedValue))
            {
                MessageBox.Show("Vælg venligst en gyldig Kaptajn");
            }
            else if (YesRadioButton.IsChecked == true || NoRadioButton.IsChecked == true) 
            {
                    if (YesRadioButton.IsChecked == true)
                    {
                        currentLogbook.DamageInflicted = true;

                        //Notify someone that the boat is damaged
                    }
                
                    if (NoRadioButton.IsChecked == true)
                    {
                        currentLogbook.DamageInflicted = false;
                    }

                RegularSailTrip.PurposeAndArea = PurposeTextBox.Text;
                currentLogbook.DamageDescription = DamageTextBox.Text;
                currentLogbook.ActualCrew = CrewList;
                currentLogbook.ActualArrivalTime = DateTimePickerActualArrival.Value.GetValueOrDefault();
                currentLogbook.ActualDepartureTime = DateTimePickerActualDeparture.Value.GetValueOrDefault();
                RegularSailTrip.Crew = CrewList;
                RegularSailTrip.Logbook = currentLogbook;

                this.Close();}
            }

            //Implement updateDatabase method, which updates the values of the Regular/CurrentTrip, and also uploads the Logbook
     }
}

