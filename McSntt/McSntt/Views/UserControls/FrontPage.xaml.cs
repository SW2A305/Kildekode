using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using McSntt.DataAbstractionLayer;
using McSntt.Helpers;
using McSntt.Models;
using McSntt.Views.Windows;

namespace McSntt.Views.UserControls
{
    /// <summary>
    /// Interaction logic for FrontPage.xaml
    /// </summary>
    public partial class FrontPage : UserControl
    {
        public FrontPage(){
            InitializeComponent();

            WelcomeBlock.Text = string.Format("Velkommen {0}!", GlobalInformation.CurrentUser.FullName);
            InfoTextBlock.Text =
                "Herunder er dine kommende sejlture og til højre er dem, du mangler at udflyde logbog for.";

            CreateLogBookButton.IsEnabled = false;
            ChangeButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            RemoveFromTrip.IsEnabled = false;

            LoadData();
        }

        public void LoadData()
        {
            var db = DalLocator.RegularTripDal;
            var usrId = GlobalInformation.CurrentUser.PersonId;

            var sailTripList = db.GetAll().ToList();

            db.LoadData(sailTripList);

            UpcommingTripsDataGrid.ItemsSource = null;
            UpcommingTripsDataGrid.ItemsSource =
                sailTripList.Where(t => t.Crew.Select(p => p.PersonId).Contains(usrId))
                    .Where(t => t.DepartureTime > DateTime.Now);

            LogbookDataGrid.ItemsSource = null;
            LogbookDataGrid.ItemsSource =
                sailTripList.Where(t => t.CreatedBy.PersonId == usrId && t.ArrivalTime < DateTime.Now && t.Logbook == null);
        }

        private RegularTrip _regularSailTrip = new RegularTrip();
        private RegularTrip _regularSailTrip2 = new RegularTrip();

        private void LogbookDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CreateLogBookButton_Click(new object(), new RoutedEventArgs());
        }

        private void CreateLogBookButton_Click(object sender, RoutedEventArgs e)
        {
            var logBookWindow = new CreateLogbookWindow((RegularTrip)LogbookDataGrid.SelectedItem, GlobalInformation.CurrentUser);
            logBookWindow.ShowDialog();

            var db = DalLocator.RegularTripDal;
            var usrId = GlobalInformation.CurrentUser.PersonId;
            var sailTripList = db.GetAll(t => t.CreatedBy.PersonId == usrId && t.ArrivalTime < DateTime.Now && t.Logbook == null).ToList();

            LogbookDataGrid.ItemsSource = null;
            LogbookDataGrid.ItemsSource = sailTripList;
        }

        private void LogbookDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Enable the button if a trip is selected.
            CreateLogBookButton.IsEnabled = LogbookDataGrid.SelectedIndex != -1;
        }

        private void ChangeButton_OnClick(object sender, RoutedEventArgs e)
        {
            var changewindow = new CreateBoatBookingWindow((RegularTrip) UpcommingTripsDataGrid.SelectedItem);
            changewindow.ShowDialog();

            LoadData();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            DalLocator.RegularTripDal.Delete((RegularTrip)UpcommingTripsDataGrid.SelectedItem);
            LoadData();
        }

        private void RemoveFromTrip_OnClick(object sender, RoutedEventArgs e)
        {
            var person = GlobalInformation.CurrentUser;
            var trip = ((RegularTrip) UpcommingTripsDataGrid.SelectedItem);

            if (person.PersonId == trip.Captain.PersonId)
            {
                MessageBox.Show(
                    "Du er kaptajn for denne tur, du kan ikke fjerne dig selv. \nGør en anden til kaptajn først.");
            }
            else
            {
                var dal = DalLocator.RegularTripDal;

                dal.LoadData(trip);
                var p2 = trip.Crew.FirstOrDefault(p => p.PersonId == person.PersonId);
                trip.Crew.Remove(p2);
                dal.Update(trip);
            }

            LoadData();
        }

        private void UpcommingTripsDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UpcommingTripsDataGrid.SelectedIndex != -1)
            {
                if (GlobalInformation.CurrentUser.PersonId == ((RegularTrip) UpcommingTripsDataGrid.SelectedItem).CreatedBy.PersonId
                    || GlobalInformation.CurrentUser.PersonId == ((RegularTrip)UpcommingTripsDataGrid.SelectedItem).Captain.PersonId)
                {
                    DeleteButton.IsEnabled = true;
                    ChangeButton.IsEnabled = true;
                    RemoveFromTrip.IsEnabled = false;
                }
                else
                {
                    DeleteButton.IsEnabled = false;
                    ChangeButton.IsEnabled = false;
                    RemoveFromTrip.IsEnabled = true;
                }
            }
        }
    }
}
