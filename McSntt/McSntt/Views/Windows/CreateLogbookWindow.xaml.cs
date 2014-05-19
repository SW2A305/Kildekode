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
    ///     Interaction logic for CreateLogbookWindow.xaml
    /// </summary>
    public partial class CreateLogbookWindow : Window
    {
        private readonly RegularTrip RegularSailTrip = new RegularTrip();
        private readonly SailClubMember _currentSailClubMember;

        private readonly DateTime _hasBeenFilledTime;
        private readonly Logbook currentLogbook = new Logbook();
        private readonly ILogbookDal logbookDal = DalLocator.LogbookDal;
        private readonly IRegularTripDal regularTripDal = DalLocator.RegularTripDal;
        public ICollection<Person> CrewList = new List<Person>();

        public CreateLogbookWindow(RegularTrip regularSailTrip, SailClubMember p)
        {
            this.InitializeComponent();

            this._currentSailClubMember = p;

            this.RegularSailTrip = regularSailTrip;

            this.CrewList = regularSailTrip.Crew.ToList();
            this.CrewDataGrid.ItemsSource = this.CrewList;
            this.PurposeTextBox.Text = regularSailTrip.PurposeAndArea;
            this.BoatTextBox.Text = regularSailTrip.Boat.NickName;
            this.DateTimePickerPlannedDepature.Value = regularSailTrip.DepartureTime;
            this.DateTimePickerPlannedArrival.Value = regularSailTrip.ArrivalTime;
            this.CaptainComboBox.DisplayMemberPath = "FullName";
            this.CaptainComboBox.ItemsSource = this.CrewList;
            this.CaptainComboBox.SelectedValue =
                this.CrewList.FirstOrDefault(x => x.PersonId == regularSailTrip.Captain.PersonId);
            this.DateTimePickerActualArrival.Value = regularSailTrip.ArrivalTime;
            this.DateTimePickerActualDeparture.Value = regularSailTrip.DepartureTime;
            this._hasBeenFilledTime = DateTime.Now;
        }

        private void ChangeCrewButtonClick(object sender, RoutedEventArgs e)
        {
            var createCrewWindow = new CreateCrewWindow(this.CrewList);
            createCrewWindow.ShowDialog();


            this.CrewList = createCrewWindow.CrewList;

            this.CrewDataGrid.ItemsSource = null;
            this.CrewDataGrid.ItemsSource = this.CrewList;

            if (!this.CrewList.Contains((Person) this.CaptainComboBox.SelectedValue)) {
                this.CaptainComboBox.SelectedValue = -1;
            }
            this.CaptainComboBox.ItemsSource = null;
            this.CaptainComboBox.ItemsSource = this.CrewList;
        }

        private void FileLogbookButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.YesRadioButton.IsChecked == false && this.NoRadioButton.IsChecked == false) {
                MessageBox.Show("Udfyld venligst om båden blev skadet under sejladsen");
            }
            else if ((this.DateTimePickerActualArrival.Value == this._hasBeenFilledTime ||
                      this.DateTimePickerActualDeparture.Value == this._hasBeenFilledTime)) {
                          MessageBox.Show("Ændre venligst din faktiske afgang og/eller faktiske ankomst");
                      }
            else if ((this.YesRadioButton.IsChecked == true) && this.DamageTextBox.Text == String.Empty) {
                MessageBox.Show("Udfyld venligst skadesrapporten med en beskrivelse af skaden");
            }
            else if (this.WeatherConditionTextBox.Text == String.Empty) {
                MessageBox.Show("Udfyld venligst vejrforholdene");
            }
            else if (!this.CrewList.Contains((Person) this.CaptainComboBox.SelectedValue)) {
                MessageBox.Show("Vælg venligst en gyldig Kaptajn");
            }
            else if (this.YesRadioButton.IsChecked == true || this.NoRadioButton.IsChecked == true)
            {
                if (this.YesRadioButton.IsChecked == true)
                {
                    this.currentLogbook.DamageInflicted = true;

                    //Notify someone that the boat is damaged
                }

                if (this.NoRadioButton.IsChecked == true) { this.currentLogbook.DamageInflicted = false; }

                this.RegularSailTrip.PurposeAndArea = this.PurposeTextBox.Text;
                this.currentLogbook.DamageDescription = this.DamageTextBox.Text;
                this.currentLogbook.ActualCrew = this.CrewList;
                this.currentLogbook.ActualArrivalTime = this.DateTimePickerActualArrival.Value;
                this.currentLogbook.ActualDepartureTime = this.DateTimePickerActualDeparture.Value;
                this.currentLogbook.FiledBy = this._currentSailClubMember;
                this.RegularSailTrip.WeatherConditions = this.WeatherConditionTextBox.Text;
                this.RegularSailTrip.Crew = this.CrewList;
                this.RegularSailTrip.Logbook = this.currentLogbook;

                this.logbookDal.Create(this.currentLogbook);

                this.regularTripDal.Update(this.RegularSailTrip);

                this.Close();
            }
        }
    }
}
