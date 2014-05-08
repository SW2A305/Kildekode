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
            if(regularSailTrip.Logbook.DamageInflicted)

            YesRadioButton.IsChecked = regularSailTrip.Logbook.DamageInflicted;
            NoRadioButton.IsChecked = !regularSailTrip.Logbook.DamageInflicted;
            DateTimePickerActualArrival.Value = regularSailTrip.Logbook.ActualArrivalTime;
            DateTimePickerActualDeparture.Value = regularSailTrip.Logbook.ActualDepartureTime;
            DamageTextBox.Text = regularSailTrip.Logbook.DamageDescription;
            AnswerFromBoatChiefTextBox.Text = regularSailTrip.Logbook.AnswerFromBoatChief;
        }
    }
}
