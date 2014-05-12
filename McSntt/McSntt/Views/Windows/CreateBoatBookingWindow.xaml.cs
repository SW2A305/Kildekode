using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using McSntt.DataAbstractionLayer;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    ///     Interaction logic for CreateBoatBooking.xaml
    /// </summary>
    public partial class CreateBoatBookingWindow
    {
        // Constructor to use this window to edit a trip
        public List<Person> CrewList = new List<Person>();

        public RegularTrip InputTrip { get; set; }

        public CreateBoatBookingWindow(RegularTrip rt) : this(-1)
        {
            // The id is combobox id + 1 (So boatId - 1)
            BoatComboBox.SelectedIndex = (int) (rt.Boat.BoatId - 1);
            DateTimeStart.Value = rt.DepartureTime;
            DateTimeEnd.Value = rt.ArrivalTime;
            CrewList = rt.Crew.ToList();
            CaptainComboBox.SelectedIndex = CrewList.IndexOf(CrewList.First(p => p.PersonId == rt.Captain.PersonId));
            PurposeTextBox.Text = rt.PurposeAndArea;

            InputTrip = rt;

            CompleteBooking.Content = "Gem ændringer";
            CompleteBooking.Click -= SaveButton_Click;
            CompleteBooking.Click += ChangeButton_Click;
            CancelBooking.Content = "Annuler ændirnger";

            CrewDataGrid.ItemsSource = null;
            CrewDataGrid.ItemsSource = CrewList;
            CaptainComboBox.ItemsSource = null;
            CaptainComboBox.ItemsSource = CrewList;
        }

        public CreateBoatBookingWindow(int index)
        {
            InitializeComponent();

            var dbm = DalLocator.BoatDal;
            BoatComboBox.ItemsSource = dbm.GetAll();
            BoatComboBox.DisplayMemberPath = "NickName";
            BoatComboBox.SelectedValuePath = "Id";
            BoatComboBox.SelectedIndex = index;

            CaptainComboBox.DisplayMemberPath = "FullName";
            CaptainComboBox.SelectedValuePath = "MemberId";
            CaptainComboBox.ItemsSource = CrewList;

            CrewList.Add(GlobalInformation.CurrentUser);
            CrewDataGrid.ItemsSource = CrewList;

            DateTimeStart.Value = DateTime.Today;
            DateTimeEnd.Value = DateTime.Today;
        }

        public CreateBoatBookingWindow(DateTime departure, DateTime arrival, Team currentTeam) : this(-1)
        {
            List<Boat> boats = DalLocator.BoatDal.GetAll().ToList();
            var anya = new Boat
            {
                Type = (currentTeam.Level == Team.ClassLevel.Second) ? BoatType.Gaffelrigger : BoatType.Drabant
            };

            Boat currentBoat = boats.FirstOrDefault(
                x => x.Type == anya.Type);

            BoatComboBox.SelectedIndex = boats.FindIndex(b => b == currentBoat);
            CrewList.Add(GlobalInformation.CurrentUser);
            CaptainComboBox.SelectedIndex = 0;
            foreach (StudentMember member in currentTeam.TeamMembers)
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var thisTrip = CreateSailTrip();

            if (thisTrip != null)
            {
                DalLocator.RegularTripDal.Create(thisTrip);
                this.Close();
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            var thisTrip = CreateSailTrip();

            if (thisTrip != null)
            {
                //TODO: Is this the corret way to update? The updated trip can be completely diffirent from the original
                // Assign the old trip id to the new one
                thisTrip.RegularTripId = InputTrip.RegularTripId;
                // Do we need to load the crew first? (it's a one-to-many relation)
                DalLocator.RegularTripDal.Update(thisTrip);
                this.Close();
            }
        }
        
        private RegularTrip CreateSailTrip()
        {
            // Get the current selected boat
            var boat = (Boat) BoatComboBox.SelectionBoxItem;

            if (boat.Operational == false)
            {
                MessageBox.Show("Båden valgt er ikke operationel");
                return null;
            }

            // Get the startTime as a datetime
            DateTime startTime = DateTimeStart.Value;

            if (startTime < DateTime.Now)
            {
                MessageBox.Show("Starttidspunktet angivet er i fortiden");
                return null;
            }

            // Get the endTime as a datetime
            DateTime endTime = DateTimeEnd.Value;

            if (endTime < startTime)
            {
                MessageBox.Show("Sluttidspunkt angivet er før starttidspunkt, tidsrejse tilbage i tiden er ej muligt");
                return null;
            }

            if (startTime == endTime)
            {
                MessageBox.Show("Start og sluttidspunkt angivet er det samme.");
                return null;
            }

            // Get the crew
            List<Person> crewSelected = CrewList;

            // Return error if no crew is selected
            if (crewSelected.Count == 0)
            {
                MessageBox.Show("Besætningslisten er tom");
                return null;
            }

            // Return error if no captain is selected
            if (CaptainComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Kaptajn ej angivet");
                return null;
            }

            // Get the captain
            var captain = (Person) CaptainComboBox.SelectionBoxItem;

            // Return error if no purpose is given
            if (PurposeTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Formål ej angivet");
                return null;
            }

            string purpose = PurposeTextBox.Text.Trim();

            // All checks are passed, create the trip.
            return new RegularTrip
            {
                Boat = boat,
                DepartureTime = startTime,
                ArrivalTime = endTime,
                Crew = crewSelected,
                Captain = captain,
                PurposeAndArea = purpose
            };
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Er du sikker på at du vil annullere din booking?", "Bådbooking",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
        }
    }
}
