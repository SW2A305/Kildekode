using System.Windows;
using McSntt.Models;
using DateTimePicker = McSntt.Views.UserControls.DateTimePicker;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for ViewSpecificLogbookWindow.xaml
    /// </summary>
    public partial class ViewSpecificLogbookWindow : Window
    {
        public ViewSpecificLogbookWindow(RegularTrip regularSailTrip)
        {
            InitializeComponent();

            CrewDataGrid.ItemsSource = regularSailTrip.Crew; 
            PurposeTextBox.Text = regularSailTrip.PurposeAndArea;
            BoatTextBox.Text = regularSailTrip.Boat.NickName;
            DateTimePickerPlannedDepature.Value = regularSailTrip.DepartureTime;
            DateTimePickerPlannedArrival.Value = regularSailTrip.ArrivalTime;
            CaptainComboBox.DisplayMemberPath = "FullName";
            CaptainComboBox.ItemsSource = regularSailTrip.Crew; 
            CaptainComboBox.SelectedValue = regularSailTrip.Captain;
            YesRadioButton.IsChecked = regularSailTrip.Logbook.DamageInflicted;
            NoRadioButton.IsChecked = !regularSailTrip.Logbook.DamageInflicted;
            DateTimePickerActualArrival.Value = regularSailTrip.Logbook.ActualArrivalTime;
            DateTimePickerActualDeparture.Value = regularSailTrip.Logbook.ActualDepartureTime;
            DamageTextBox.Text = regularSailTrip.Logbook.DamageDescription;
            AnswerFromBoatChiefTextBox.Text = regularSailTrip.Logbook.AnswerFromBoatChief;
            WeatherConditionTextBox.Text = regularSailTrip.WeatherConditions;
        }
    }
}
