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

        public CreateBoatBookingWindow(RegularTrip rt) : this(-1)
        {
            // The id is combobox id + 1 (So boatId - 1)
            this.BoatComboBox.SelectedIndex = (int) (rt.Boat.BoatId - 1);
            this.DateTimeStart.Value = rt.DepartureTime;
            this.DateTimeEnd.Value = rt.ArrivalTime;
            this.CrewList = rt.Crew.ToList();
            this.PurposeTextBox.Text = rt.PurposeAndArea;

            // Not alloed to change the time of this trip
            this.DateTimeStart.IsReadOnly = true;
            this.DateTimeEnd.IsReadOnly = true;

            this.InputTrip = rt;

            this.CompleteBooking.Content = "Gem ændringer";
            this.CompleteBooking.Click -= this.SaveButton_Click;
            this.CompleteBooking.Click += this.ChangeButton_Click;
            this.CancelBooking.Content = "Annuler ændirnger";

            this.CrewDataGrid.ItemsSource = null;
            this.CrewDataGrid.ItemsSource = this.CrewList;
            this.CaptainComboBox.ItemsSource = null;
            this.CaptainComboBox.ItemsSource = this.CrewList.Where(x => x.BoatDriver);
            this.CaptainComboBox.SelectedItem = this.CrewList.Where(x => rt.Captain.PersonId == x.PersonId).Select(x => x);
            //   this.CrewList.IndexOf(this.CrewList.First(p => p.PersonId == rt.CaptainId));
        }

        public CreateBoatBookingWindow(int index)
        {
            this.InitializeComponent();

            IBoatDal dbm = DalLocator.BoatDal;
            this.BoatComboBox.ItemsSource = dbm.GetAll();
            this.BoatComboBox.DisplayMemberPath = "NickName";
            this.BoatComboBox.SelectedValuePath = "Id";
            this.BoatComboBox.SelectedIndex = index;

            this.CrewList.Add(GlobalInformation.CurrentUser);
            this.CrewDataGrid.ItemsSource = this.CrewList;

            this.CaptainComboBox.DisplayMemberPath = "FullName";
            this.CaptainComboBox.SelectedValuePath = "MemberId";
            this.CaptainComboBox.ItemsSource = this.CrewList;

            this.DateTimeStart.Value = DateTime.Today;
            this.DateTimeEnd.Value = DateTime.Today;
        }

        public CreateBoatBookingWindow(DateTime departure, DateTime arrival, Team currentTeam) : this(-1)
        {
            List<Boat> boats = DalLocator.BoatDal.GetAll().ToList();
            var anya = new Boat
                       {
                           Type =
                               (currentTeam.Level == Team.ClassLevel.Second) ? BoatType.Gaffelrigger : BoatType.Drabant
                       };

            Boat currentBoat = boats.FirstOrDefault(
                                                    x => x.Type == anya.Type);

            this.BoatComboBox.SelectedIndex = boats.FindIndex(b => b == currentBoat);
            this.CrewList.Add(GlobalInformation.CurrentUser);
            this.CaptainComboBox.SelectedIndex = 0;
            foreach (StudentMember member in currentTeam.TeamMembers) { this.CrewList.Add(member); }
            this.DateTimeStart.Value = departure;
            this.DateTimeEnd.Value = arrival;
            this.PurposeTextBox.Text = "Undervisning af:" + currentTeam.Name;

            this.SaveButton_Click(new object(), new RoutedEventArgs());
        }

        public RegularTrip InputTrip { get; set; }

        private void ChangeCrew_Click(object sender, RoutedEventArgs e)
        {
            var createCrewWindow = new CreateCrewWindow(this.CrewList);
            createCrewWindow.ShowDialog();

            this.CrewList = createCrewWindow.CrewList.ToList();

            // After the crew is changed refresh the data grid and captain selector
            this.CrewDataGrid.ItemsSource = null;
            this.CrewDataGrid.ItemsSource = this.CrewList;
            this.CaptainComboBox.ItemsSource = null;
            this.CaptainComboBox.ItemsSource = this.CrewList.Where(x => x.BoatDriver);

            // Assign a default captain to the first member
            if (this.CaptainComboBox.SelectedIndex == -1 && this.CrewList.Count > 0) {
                this.CaptainComboBox.SelectedIndex = 0;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            RegularTrip thisTrip = this.CreateSailTrip();

            if (thisTrip != null)
            {
                if (thisTrip.CanMakeReservation())
                {
                    DalLocator.RegularTripDal.Create(thisTrip);
                    this.Close();
                    return;
                }

                MessageBox.Show("Der findes allerede en tur inden for dit valgte interval");
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            RegularTrip thisTrip = this.CreateSailTrip();

            if (thisTrip != null)
            {
                //TODO: Is this the corret way to update? The updated trip can be completely diffirent from the original
                // Assign the old trip id to the new one
                thisTrip.RegularTripId = this.InputTrip.RegularTripId;
                // Do we need to load the crew first? (it's a one-to-many relation)
                DalLocator.RegularTripDal.Update(thisTrip);
                this.Close();
            }
        }

        private RegularTrip CreateSailTrip()
        {
            // Get the current selected boat
            var boat = (Boat) this.BoatComboBox.SelectionBoxItem;

            if (boat.Operational == false)
            {
                MessageBox.Show("Båden valgt er ikke operationel");
                return null;
            }

            // Get the startTime as a datetime
            DateTime startTime = this.DateTimeStart.Value;

            if (startTime < DateTime.Now)
            {
                MessageBox.Show("Starttidspunktet angivet er i fortiden");
                return null;
            }

            // Get the endTime as a datetime
            DateTime endTime = this.DateTimeEnd.Value;

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
            List<Person> crewSelected = this.CrewList;

            // Return error if no crew is selected
            if (crewSelected.Count == 0)
            {
                MessageBox.Show("Besætningslisten er tom");
                return null;
            }

            // Return error if no captain is selected
            if (this.CaptainComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Kaptajn ej angivet");
                return null;
            }

            // Get the captain
            var captain = (Person) this.CaptainComboBox.SelectionBoxItem;

            // Return error if no purpose is given
            if (this.PurposeTextBox.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Formål ej angivet");
                return null;
            }

            string purpose = this.PurposeTextBox.Text.Trim();

            // All checks are passed, create the trip.
            var trip = new RegularTrip
                       {
                           CreatedBy = GlobalInformation.CurrentUser,
                           Boat = boat,
                           DepartureTime = startTime,
                           ArrivalTime = endTime,
                           Crew = crewSelected,
                           Captain = captain,
                           PurposeAndArea = purpose
                       };

            return trip;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Er du sikker på at du vil annullere din booking?", "Bådbooking",
                                                      MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) { this.Close(); }
        }
    }
}
