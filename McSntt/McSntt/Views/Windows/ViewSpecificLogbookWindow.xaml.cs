using System.Linq;
using System.Windows;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    ///     Interaction logic for ViewSpecificLogbookWindow.xaml
    /// </summary>
    public partial class ViewSpecificLogbookWindow : Window
    {
        public ViewSpecificLogbookWindow(RegularTrip regularSailTrip)
        {
            this.InitializeComponent();

            this.CrewDataGrid.ItemsSource = regularSailTrip.Crew;
            this.PurposeTextBox.Text = regularSailTrip.PurposeAndArea;
            this.BoatTextBox.Text = regularSailTrip.Boat.NickName;
            this.DateTimePickerPlannedDepature.Value = regularSailTrip.DepartureTime;
            this.DateTimePickerPlannedArrival.Value = regularSailTrip.ArrivalTime;
            this.CaptainComboBox.DisplayMemberPath = "FullName";
            this.CaptainComboBox.ItemsSource = regularSailTrip.Crew;
            this.CaptainComboBox.SelectedValue =
                regularSailTrip.Crew.FirstOrDefault(x => x.PersonId == regularSailTrip.Captain.PersonId);
            this.YesRadioButton.IsChecked = regularSailTrip.Logbook.DamageInflicted;
            this.NoRadioButton.IsChecked = !regularSailTrip.Logbook.DamageInflicted;
            this.DateTimePickerActualArrival.Value = regularSailTrip.Logbook.ActualArrivalTime;
            this.DateTimePickerActualDeparture.Value = regularSailTrip.Logbook.ActualDepartureTime;
            this.DamageTextBox.Text = regularSailTrip.Logbook.DamageDescription;
            this.AnswerFromBoatChiefTextBox.Text = regularSailTrip.Logbook.AnswerFromBoatChief;
            this.WeatherConditionTextBox.Text = regularSailTrip.WeatherConditions;

            if (regularSailTrip.Logbook.DamageInflicted) { this.YesRadioButton.IsChecked = true; }
            else
            { this.NoRadioButton.IsChecked = true; }
        }
    }
}
