using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using McSntt.DataAbstractionLayer;
using McSntt.Helpers;
using McSntt.Models;
using DateTimePicker = McSntt.Views.UserControls.DateTimePicker;
using MessageBox = System.Windows.MessageBox;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for CreateLogbookWindow.xaml
    /// </summary>
    public partial class CreateLogbookWindow : Window
    {
        public ICollection<Person> CrewList = new List<Person>();
        private RegularTrip RegularSailTrip = new RegularTrip();
        private Logbook currentLogbook = new Logbook();
        private SailClubMember _currentSailClubMember;

        private IRegularTripDal regularTripDal = DalLocator.RegularTripDal;
        private ILogbookDal logbookDal = DalLocator.LogbookDal;

        private readonly DateTime _hasBeenFilledTime = new DateTime();

        public CreateLogbookWindow(RegularTrip sailTrip, SailClubMember p)
        {
            InitializeComponent();
            
            _currentSailClubMember = p;

            RegularSailTrip = sailTrip;

            CrewList = RegularSailTrip.Crew.ToList();
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
                MessageBox.Show("Ændre venligst din faktiske afgang og/eller faktiske ankomst");
            }
            else if ((NoRadioButton.IsChecked == true || YesButBrokenRadioButton.IsChecked == true) && DamageTextBox.Text == String.Empty)
            {
                MessageBox.Show("Udfyld venligst skadesrapporten med en beskrivelse af skaden");
            }
            else if (!CrewList.Contains((Person)CaptainComboBox.SelectedValue))
            {
                MessageBox.Show("Vælg venligst en gyldig Kaptajn");
            }
            else if (YesRadioButton.IsChecked == true || NoRadioButton.IsChecked == true
                        || YesButBrokenRadioButton.IsChecked == true) 
            {
                    if (YesRadioButton.IsChecked == true)
                    {
                        currentLogbook.DamageInflicted = false;

                        //Notify someone that the boat is damaged
                    }
                
                    if (NoRadioButton.IsChecked == true || YesButBrokenRadioButton.IsChecked == true)
                    {
                        currentLogbook.DamageInflicted = true;
                    }

                RegularSailTrip.PurposeAndArea = PurposeTextBox.Text;
                currentLogbook.DamageDescription = DamageTextBox.Text;
                currentLogbook.ActualCrew = CrewList;
                currentLogbook.ActualArrivalTime = DateTimePickerActualArrival.Value;
                currentLogbook.ActualDepartureTime = DateTimePickerActualDeparture.Value;
                currentLogbook.FiledBy = _currentSailClubMember;
                RegularSailTrip.Crew = CrewList;
                RegularSailTrip.Logbook = currentLogbook;

                logbookDal.Create(RegularSailTrip.Logbook);

                regularTripDal.Update(RegularSailTrip);
                
                this.Close();}

            }
     }
}

