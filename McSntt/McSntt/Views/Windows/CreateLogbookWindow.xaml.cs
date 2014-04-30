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
        public List<Person> CrewList = new List<Person>();
        private RegularTrip RegularSailTrip = new RegularTrip();
        private Logbook currentLogbook = new Logbook();

        public CreateLogbookWindow(/*RegularTrip sailTrip*/)
        {
            InitializeComponent();
            
            /*RegularSailTrip = sailTrip;*/
            RegularSailTrip = new RegularTrip
            {
                Boat = new Boat() {NickName = "Bodil2"},
                ArrivalTime = new DateTime(2014, 09, 9, 12, 0, 0),
                BoatId = 1,
                Captain = new Person() {FirstName = "Sponge", LastName = "Bob"},
                Comments = "Det blir sjaw!",
                DepartureTime = new DateTime(2014, 09, 9, 09, 0, 0),
                PurposeAndArea = "u' ti' æ ' van' og' hjem' ien...",
                WeatherConditions = "Det 'en bæt' wind...",
                RegularTripId = 9
            };
           
            FormålTextBox.Text = RegularSailTrip.PurposeAndArea;
            BådTextBox.Text = RegularSailTrip.Boat.NickName;
            DateTimePickerPlannedDepature.Value = RegularSailTrip.DepartureTime;
            DateTimePickerPlannedArrival.Value = RegularSailTrip.ArrivalTime;
            CaptainTextBox.Text = RegularSailTrip.Captain.FirstName + " " + RegularSailTrip.Captain.LastName;



        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var createCrewWindow = new CreateCrewWindow(CrewList);
            createCrewWindow.ShowDialog();


            CrewList = createCrewWindow._crewList;

            CrewDataGrid.ItemsSource = null;
            CrewDataGrid.ItemsSource = CrewList;
        }

        private void FileLogbookButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (JaRadioButton.IsChecked == true || NejRadioButton.IsChecked == true && 
                DateTimePickerActualArrival !=null && DateTimePickerActualDeparture != null )
            {
                RegularSailTrip.PurposeAndArea = FormålTextBox.Text;
                currentLogbook.DamageDescription = SkadesrapportTextBox.Text;
                currentLogbook.ActualCrew = CrewList;
                currentLogbook.ActualArrivalTime = DateTimePickerActualArrival.Value.GetValueOrDefault();
                currentLogbook.ActualDepartureTime = DateTimePickerActualDeparture.Value.GetValueOrDefault();
                if (JaRadioButton.IsChecked == true)
                {
                    currentLogbook.DamageInflicted = true;
                    //Notify someone that the boat is damaged
                }
                if (NejRadioButton.IsChecked == true)
                {
                    currentLogbook.DamageInflicted = false;
                }
                this.Close();
            }
            else MessageBox.Show("Udfyld venligst dato felterne og om båden blev skadet under sejladsen");
            //Implement updateDatabase method, which updates the values of the CurrentSailTrip

        }
    }
}
