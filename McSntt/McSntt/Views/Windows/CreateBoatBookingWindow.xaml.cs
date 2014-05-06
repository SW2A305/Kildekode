using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using McSntt.DataAbstractionLayer;
using McSntt.Helpers;
using McSntt.Models;
using MessageBox = System.Windows.MessageBox;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for CreateBoatBooking.xaml
    /// </summary>
    public partial class CreateBoatBookingWindow : Window
    {
        public CreateBoatBookingWindow(int index)
        {
            InitializeComponent();
            
            var dbm = new BoatEfDal();
            BoatComboBox.ItemsSource = dbm.GetAll();
            BoatComboBox.DisplayMemberPath = "NickName";
            BoatComboBox.SelectedValuePath = "Id";
            BoatComboBox.SelectedIndex = index;
            
            CaptainComboBox.DisplayMemberPath = "FirstName";
            CaptainComboBox.SelectedValuePath = "MemberId";
            CaptainComboBox.ItemsSource = CrewList;

            DateTimeStart.Value = DateTime.Today;
            DateTimeEnd.Value = DateTime.Today;

        }

        private void BoatComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            
        }
        public List<Person> CrewList = new List<Person>();

        public CreateBoatBookingWindow(DateTime departure, DateTime arrival, Team currentTeam) : this(-1)
        {
            List<Boat> boats = new List<Boat>();
            boats = DalLocator.BoatDal.GetAll().ToList();
            Boat Anya = new Boat
            {
                Type = (currentTeam.Level == Team.ClassLevel.Second) ? BoatType.Gaffelrigger : BoatType.Drabant
            };

            Boat currentBoat = boats.FirstOrDefault(
                x => x.Type == Anya.Type);

            BoatComboBox.SelectedIndex = boats.FindIndex(b => b == currentBoat);
            CrewList.Add(GlobalInformation.CurrentUser);
            CaptainComboBox.SelectedIndex = 0;
            foreach (var member in currentTeam.TeamMembers)
            {
                CrewList.Add(member);
            }
            DateTimeStart.Value = departure;
            DateTimeEnd.Value = arrival;
            PurposeTextBox.Text = "Undervisning af:" + currentTeam.Name;
            SaveButton_Click(new object(), new RoutedEventArgs());

        }

        private void ChangeCrew_Click(object sender, RoutedEventArgs e)
        {
            var createCrewWindow = new CreateCrewWindow(CrewList);
            createCrewWindow.ShowDialog();

            CrewList = createCrewWindow.CrewList.ToList();

            // After the crew is changed refresh the data grid and captain selector
            CrewDataGrid.ItemsSource = null;
            CrewDataGrid.ItemsSource = CrewList;
            CaptainComboBox.ItemsSource = null;
            CaptainComboBox.ItemsSource = CrewList;
            
            // Assign a default captain to the first member
            if (CaptainComboBox.SelectedIndex == -1 && CrewList.Count > 0)
            {
                CaptainComboBox.SelectedIndex = 0;
            }
            
        }

        private void CaptainComboBox_OnDropDownClosed(object sender, EventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Check if this is a valid trip. If not return saying error. 
            // Get the current selected boat
            Boat boat = (Boat)BoatComboBox.SelectionBoxItem;

            // Get the startTime as a datetime
            DateTime startTime = DateTimeStart.Value.GetValueOrDefault();

            if (startTime < DateTime.Now)
            {
                MessageBox.Show("Starttidspunktet angivet er i fortiden");
                return;
            }

            // Get the endTime as a datetime
            DateTime endTime = DateTimeEnd.Value.GetValueOrDefault();

            if (endTime < startTime)
            {
                MessageBox.Show("Sluttidspunkt angivet er før starttidspunkt, tidsrejse tilbage i tiden er ej muligt");
                return;
            }

            if (startTime == endTime)
            {
                MessageBox.Show("Start og sluttidspunkt angivet er det samme.");
                return;
            }

            // Get the crew
            var crewSelected = CrewList;

            // Return error if no crew is selected
            if (crewSelected.Count == 0)
            {
                MessageBox.Show("Besætningslisten er tom");
                return;
            }

            // Return error if no captain is selected
            if (CaptainComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Kaptajn ej angivet");
                return;
            }

            // Get the captain
            var captain = (Person) CaptainComboBox.SelectionBoxItem;

            // Return error if no purpose is given
            if (PurposeTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Formål ej angivet");
                return;
            }

            var purpose = PurposeTextBox.Text.Trim();

            // All checks are passed, create the trip.
            var complete = new RegularTrip()
            {
                Boat = boat,
                DepartureTime = startTime,
                ExpectedArrivalTime = endTime,
                Crew = crewSelected,
                Captain = captain,
                Comments = purpose
            };

            var dbm = new RegularTripEfDal();
            
            if (dbm.Create(complete))
            {
                MessageBox.Show("Din tur er oprettet!");
            }
            else
            {
                MessageBox.Show("Doh!");
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Add warning window
            MessageBoxResult result = MessageBox.Show("Er du sikker på at du vil annullere din booking?", "Bådbooking", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}
