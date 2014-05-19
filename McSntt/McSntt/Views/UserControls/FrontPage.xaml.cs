using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using McSntt.DataAbstractionLayer;
using McSntt.Helpers;
using McSntt.Models;
using McSntt.Views.Windows;

namespace McSntt.Views.UserControls
{
    /// <summary>
    ///     Interaction logic for FrontPage.xaml
    /// </summary>
    public partial class FrontPage : UserControl
    {
        private RegularTrip _regularSailTrip = new RegularTrip();
        private RegularTrip _regularSailTrip2 = new RegularTrip();

        public FrontPage()
        {
            this.InitializeComponent();

            this.WelcomeBlock.Text = string.Format("Velkommen {0}!", GlobalInformation.CurrentUser.FullName);
            this.InfoTextBlock.Text =
                "Herunder er dine kommende sejlture og til højre er dem, du mangler at udflyde logbog for.";

            this.CreateLogBookButton.IsEnabled = false;
            this.ChangeButton.IsEnabled = false;
            this.DeleteButton.IsEnabled = false;
            this.RemoveFromTrip.IsEnabled = false;

            this.LoadData();
        }

        public void LoadData()
        {
            IRegularTripDal db = DalLocator.RegularTripDal;
            long usrId = GlobalInformation.CurrentUser.PersonId;

            List<RegularTrip> sailTripList = db.GetAll().ToList();

            db.LoadData(sailTripList);

            this.UpcommingTripsDataGrid.ItemsSource = null;
            this.UpcommingTripsDataGrid.ItemsSource =
                sailTripList.Where(t => t.Crew.Select(p => p.PersonId).Contains(usrId))
                            .Where(t => t.DepartureTime > DateTime.Now);

            this.LogbookDataGrid.ItemsSource = null;
            this.LogbookDataGrid.ItemsSource =
                sailTripList.Where(
                                   t =>
                                   t.CreatedBy.PersonId == usrId && t.ArrivalTime < DateTime.Now && t.Logbook == null);
        }

        private void LogbookDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.CreateLogBookButton_Click(new object(), new RoutedEventArgs());
        }

        private void CreateLogBookButton_Click(object sender, RoutedEventArgs e)
        {
            var logBookWindow = new CreateLogbookWindow((RegularTrip) this.LogbookDataGrid.SelectedItem,
                                                        GlobalInformation.CurrentUser);
            logBookWindow.ShowDialog();

            IRegularTripDal db = DalLocator.RegularTripDal;
            long usrId = GlobalInformation.CurrentUser.PersonId;
            List<RegularTrip> sailTripList =
                db.GetAll(t => t.CreatedBy.PersonId == usrId && t.ArrivalTime < DateTime.Now && t.Logbook == null)
                  .ToList();

            this.LogbookDataGrid.ItemsSource = null;
            this.LogbookDataGrid.ItemsSource = sailTripList;
        }

        private void LogbookDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Enable the button if a trip is selected.
            this.CreateLogBookButton.IsEnabled = this.LogbookDataGrid.SelectedIndex != -1;
        }

        private void ChangeButton_OnClick(object sender, RoutedEventArgs e)
        {
            var changewindow = new CreateBoatBookingWindow((RegularTrip) this.UpcommingTripsDataGrid.SelectedItem);
            changewindow.ShowDialog();

            this.LoadData();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            DalLocator.RegularTripDal.Delete((RegularTrip) this.UpcommingTripsDataGrid.SelectedItem);
            this.LoadData();
        }

        private void RemoveFromTrip_OnClick(object sender, RoutedEventArgs e)
        {
            SailClubMember person = GlobalInformation.CurrentUser;
            var trip = ((RegularTrip) this.UpcommingTripsDataGrid.SelectedItem);

            if (person.PersonId == trip.Captain.PersonId)
            {
                MessageBox.Show(
                                "Du er kaptajn for denne tur, du kan ikke fjerne dig selv. \nGør en anden til kaptajn først.");
            }
            else
            {
                IRegularTripDal dal = DalLocator.RegularTripDal;

                dal.LoadData(trip);
                Person p2 = trip.Crew.FirstOrDefault(p => p.PersonId == person.PersonId);
                trip.Crew.Remove(p2);
                dal.Update(trip);
            }

            this.LoadData();
        }

        private void UpcommingTripsDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.UpcommingTripsDataGrid.SelectedIndex != -1)
            {
                if (GlobalInformation.CurrentUser.PersonId
                    == ((RegularTrip) this.UpcommingTripsDataGrid.SelectedItem).CreatedBy.PersonId
                    || GlobalInformation.CurrentUser.PersonId
                    == ((RegularTrip) this.UpcommingTripsDataGrid.SelectedItem).Captain.PersonId)
                {
                    this.DeleteButton.IsEnabled = true;
                    this.ChangeButton.IsEnabled = true;
                    this.RemoveFromTrip.IsEnabled = false;
                }
                else
                {
                    this.DeleteButton.IsEnabled = false;
                    this.ChangeButton.IsEnabled = false;
                    this.RemoveFromTrip.IsEnabled = true;
                }
            }
        }
    }
}
