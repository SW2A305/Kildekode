using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using McSntt.DataAbstractionLayer;
using McSntt.Helpers;
using McSntt.Models;
using McSntt.Views.Windows;

namespace McSntt.Views.UserControls
{
    /// <summary>
    ///     Interaction logic for Boats.xaml
    /// </summary>
    /// ry>
    public partial class Boats : UserControl
    {
        private readonly IBoatDal boatDal = DalLocator.BoatDal;
        private readonly ILogbookDal logbookDal = DalLocator.LogbookDal;
        private readonly IRegularTripDal regularTripDal = DalLocator.RegularTripDal;

        public Boat CurrentBoat = new Boat();
        public RegularTrip CurrentSailtrip = new RegularTrip();

        public Boats()
        {
            this.InitializeComponent();

            IEnumerable<Boat> boatList = this.boatDal.GetAll();

            this.BoatComboBox.ItemsSource = boatList;
            this.BoatComboBox.DisplayMemberPath = "NickName";

            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("pack://application:,,,/Images/SundetLogo.PNG");
            image.EndInit();
            this.BoatImage.Source = image;

            this.BookButton.IsEnabled = false;
            this.ChangeButton.IsEnabled = false;
            this.DeleteButton.IsEnabled = false;


            if (GlobalInformation.CurrentUser.Position == SailClubMember.Positions.Admin)
            {
                this.AnswerDamageReportButton.Visibility = Visibility.Visible;
                this.EditBoatButton.IsEnabled = false;
            }
            else
            {
                this.AnswerDamageReportButton.Visibility = Visibility.Hidden;

                this.AddBoatButton.Visibility = Visibility.Hidden;
                this.EditBoatButton.Visibility = Visibility.Hidden;
            }
        }

        private void LogbookDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var logBookWindow = new ViewSpecificLogbookWindow((RegularTrip) this.LogbookDataGrid.CurrentItem);
            logBookWindow.ShowDialog();
        }

        private IEnumerable<RegularTrip> GetBookings()
        {
            return this.regularTripDal.GetAll(
                                              x =>
                                              x.Boat.BoatId == this.CurrentBoat.BoatId
                                              && x.DepartureTime >= DateTime.Now)
                       .OrderBy(x => x.DepartureTime);
        }


        private void BoatComboBox_OnSelectionChanged(object sender, EventArgs e)
        {
            if (this.BoatComboBox.SelectedIndex != -1)
            {
                this.EditBoatButton.IsEnabled = true;
                this.CurrentBoat = (Boat) this.BoatComboBox.SelectionBoxItem;

                IEnumerable<RegularTrip> ListOfTripsWithLogbook =
                    this.regularTripDal.GetAll(
                                               x => x.Boat.BoatId == this.CurrentBoat.BoatId && x.Logbook != null);

                IEnumerable<RegularTrip> listOfBookings = this.GetBookings();

                // Grey out the book button for support members and guests
                if (GlobalInformation.CurrentUser.Position != SailClubMember.Positions.SupportMember) {
                    this.BookButton.IsEnabled = true;
                }

                if (this.CurrentBoat.ImagePath != null)
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri("pack://application:,,,/Images/" + this.CurrentBoat.ImagePath);
                    image.EndInit();
                    this.BoatImage.Source = image;
                }
                else
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri("pack://application:,,,/Images/gray.PNG");
                    image.EndInit();
                    this.BoatImage.Source = image;
                }
                string operationel = "";
                if (this.CurrentBoat.Operational) {
                    operationel = "Operationel";
                }
                else if (!this.CurrentBoat.Operational) { operationel = "Ikke operationel"; }

                this.BoatTypeTextBox.Text = Enum.GetName(typeof (BoatType), this.CurrentBoat.Type);
                this.BoatStatusTextBox.Text = operationel;

                this.LogbookDataGrid.ItemsSource = null;
                this.LogbookDataGrid.ItemsSource = ListOfTripsWithLogbook;

                this.BookedTripsDataGrid.ItemsSource = null;
                this.BookedTripsDataGrid.ItemsSource = listOfBookings;
            }
            else
            {
                this.BookButton.IsEnabled = false;
                this.EditBoatButton.IsEnabled = false;
            }
        }

        private void ChooseLogbookButton_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentSailtrip = (RegularTrip) this.LogbookDataGrid.SelectedItem;
            if (this.CurrentSailtrip == null) {
                MessageBox.Show("Vælg venligst en Logbog du gerne vil se");
            }
            else
            {
                var logbookwindow = new ViewSpecificLogbookWindow(this.CurrentSailtrip);
                logbookwindow.ShowDialog();
            }
        }

        private void AnswerDamageReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.CurrentSailtrip = (RegularTrip) this.LogbookDataGrid.SelectedItem;
            if (this.CurrentSailtrip == null) {
                MessageBox.Show("Vælg venligst en Logbog du gerne vil svare på");
            }
            else
            {
                var DamageReportWindow = new DamageReportWindow(this.CurrentSailtrip);
                DamageReportWindow.ShowDialog();

                if (DamageReportWindow.DamageReport != String.Empty && DamageReportWindow.IsAnswered) {
                    this.CurrentSailtrip.Logbook.AnswerFromBoatChief = DamageReportWindow.DamageReport;
                }
                else
                {
                    MessageBox.Show(
                                    "Du bedes trykke udfør for at gemme dit svar og samtidig må dit svar ikke være tomt.");
                }
                this.logbookDal.Update(this.CurrentSailtrip.Logbook);
            }
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            var BookWindow = new CreateBoatBookingWindow(this.BoatComboBox.SelectedIndex);
            BookWindow.ShowDialog();

            IEnumerable<RegularTrip> listOfBookings = this.GetBookings();
            this.BookedTripsDataGrid.ItemsSource = null;
            this.BookedTripsDataGrid.ItemsSource = listOfBookings;
        }

        private void AddBoatButton_Click(object sender, RoutedEventArgs e)
        {
            var boatWindow = new CreateAndEditBoats();
            boatWindow.ShowDialog();
        }

        private void EditBoatButton_Click(object sender, RoutedEventArgs e)
        {
            var boatWindow = new CreateAndEditBoats((Boat) this.BoatComboBox.SelectedItem);
            boatWindow.ShowDialog();

            IEnumerable<RegularTrip> listOfBookings = this.GetBookings();
            this.BookedTripsDataGrid.ItemsSource = null;
            this.BookedTripsDataGrid.ItemsSource = listOfBookings;
        }

        private void ChangeButton_OnClick(object sender, RoutedEventArgs e)
        {
            RegularTrip trip =
                DalLocator.RegularTripDal.GetOne(((RegularTrip) this.BookedTripsDataGrid.SelectedItem).RegularTripId);
            DalLocator.RegularTripDal.LoadData(trip);

            var changewindow = new CreateBoatBookingWindow(trip);
            changewindow.ShowDialog();

            IEnumerable<RegularTrip> listofbookings = this.GetBookings();
            this.BookedTripsDataGrid.ItemsSource = null;
            this.BookedTripsDataGrid.ItemsSource = listofbookings;
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            DalLocator.RegularTripDal.Delete((RegularTrip) this.BookedTripsDataGrid.SelectedItem);

            IEnumerable<RegularTrip> listOfBookings = this.GetBookings();
            this.BookedTripsDataGrid.ItemsSource = null;
            this.BookedTripsDataGrid.ItemsSource = listOfBookings;
        }

        private void BookedTripsDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.BookedTripsDataGrid.SelectedIndex != -1)
            {
                if (GlobalInformation.CurrentUser.Position == SailClubMember.Positions.Admin ||
                    (GlobalInformation.CurrentUser.PersonId
                     == ((RegularTrip) this.BookedTripsDataGrid.SelectedItem).CreatedBy.PersonId ||
                     GlobalInformation.CurrentUser.PersonId
                     == ((RegularTrip) this.BookedTripsDataGrid.SelectedItem).Captain.PersonId))
                {
                    this.DeleteButton.IsEnabled = true;
                    this.ChangeButton.IsEnabled = true;
                }
                else
                {
                    this.DeleteButton.IsEnabled = false;
                    this.ChangeButton.IsEnabled = false;
                }
            }
            else
            {
                this.DeleteButton.IsEnabled = false;
                this.ChangeButton.IsEnabled = false;
            }
        }
    }
}
