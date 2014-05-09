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
        private readonly RegularTrip RegularSailTrip1 = new RegularTrip();
        private readonly IBoatDal boatDal = DalLocator.BoatDal;
        private readonly ILogbookDal logbookDal = DalLocator.LogbookDal;
        private readonly IRegularTripDal regularTripDal = DalLocator.RegularTripDal;

        public Boat CurrentBoat = new Boat();
        public RegularTrip CurrentSailtrip = new RegularTrip();

        public Boats()
        {
            InitializeComponent();

            IEnumerable<Boat> boatList = boatDal.GetAll();

            BoatComboBox.ItemsSource = boatList;
            BoatComboBox.DisplayMemberPath = "NickName";

            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("pack://application:,,,/Images/gray.PNG");
            image.EndInit();
            BoatImage.Source = image;
            if (GlobalInformation.CurrentUser.Position == SailClubMember.Positions.Admin)
                AnswerDamageReportButton.Visibility = Visibility.Visible;
            else AnswerDamageReportButton.Visibility = Visibility.Hidden;

            BookButton.IsEnabled = false;
            ChangeButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
        }

        private void LogbookDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var logBookWindow = new ViewSpecificLogbookWindow((RegularTrip) LogbookDataGrid.CurrentItem);
            logBookWindow.ShowDialog();
        }


        private void BoatComboBox_OnSelectionChanged(object sender, EventArgs e)
        {
            if (BoatComboBox.SelectedIndex != -1)
            {
                CurrentBoat = (Boat) BoatComboBox.SelectionBoxItem;

                IEnumerable<RegularTrip> ListOfTripsWithLogbook =
                    regularTripDal.GetRegularTrips(
                        x => x.Boat.BoatId == CurrentBoat.BoatId && x.Logbook != null);

                IEnumerable<RegularTrip> ListOfBookings =
                    regularTripDal.GetRegularTrips(
                        x => x.Boat.BoatId == CurrentBoat.BoatId && x.DepartureTime >= DateTime.Now)
                        .OrderBy(x => x.DepartureTime);

                // Grey out the book button for support memebers and guests
                if (GlobalInformation.CurrentUser.Position != SailClubMember.Positions.SupportMember)
                    BookButton.IsEnabled = true;

                if (CurrentBoat.ImagePath != null)
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri("pack://application:,,,/Images/" + CurrentBoat.ImagePath);
                    image.EndInit();
                    BoatImage.Source = image;
                }
                else
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri("pack://application:,,,/Images/gray.PNG");
                    image.EndInit();
                    BoatImage.Source = image;
                }
                string operationel = "";
                if (CurrentBoat.Operational)
                {
                    operationel = "Operationel";
                }
                else if (!CurrentBoat.Operational)
                {
                    operationel = "Ikke operationel";
                }

                BoatTypeTextBox.Text = Enum.GetName(typeof (BoatType), CurrentBoat.Type);
                BoatStatusTextBox.Text = operationel;

                LogbookDataGrid.ItemsSource = null;
                LogbookDataGrid.ItemsSource = ListOfTripsWithLogbook;

                BookedTripsDataGrid.ItemsSource = null;
                BookedTripsDataGrid.ItemsSource = ListOfBookings;
            }
            else BookButton.IsEnabled = false;
        }

        private void ChooseLogbookButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentSailtrip = (RegularTrip) LogbookDataGrid.SelectedItem;
            if (CurrentSailtrip == null)
                MessageBox.Show("Vælg venligst en Logbog du gerne vil se");
            else
            {
                var logbookwindow = new ViewSpecificLogbookWindow(CurrentSailtrip);
                logbookwindow.ShowDialog();
            }
        }

        private void AnswerDamageReportButton_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentSailtrip = (RegularTrip) LogbookDataGrid.SelectedItem;
            if (CurrentSailtrip == null)
            {
                MessageBox.Show("Vælg venligst en Logbog du gerne vil svare på");
            }
            else
            {
                var DamageReportWindow = new DamageReportWindow(CurrentSailtrip);
                DamageReportWindow.ShowDialog();

                if (DamageReportWindow.DamageReport != String.Empty && DamageReportWindow.IsAnswered)
                {
                    CurrentSailtrip.Logbook.AnswerFromBoatChief = DamageReportWindow.DamageReport;
                }
                else
                {
                    MessageBox.Show("Du bedes trykke udfør for at gemme dit svar og samtidig må dit svar ikke være tomt.");
                }
                logbookDal.Update(CurrentSailtrip.Logbook);
            }
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            var BookWindow = new CreateBoatBookingWindow(BoatComboBox.SelectedIndex);
            BookWindow.ShowDialog();
        }

        private void ChangeButton_OnClick(object sender, RoutedEventArgs e)
        {
            var changewindow = new CreateBoatBookingWindow((RegularTrip)BookedTripsDataGrid.SelectedItem);
            changewindow.ShowDialog();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO: Database slet medlem og opdater grid
        }

        private void BookedTripsDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookedTripsDataGrid.SelectedIndex != -1 && GlobalInformation.CurrentUser.Position == SailClubMember.Positions.Admin)
            {
                DeleteButton.IsEnabled = true;
                ChangeButton.IsEnabled = true;
            }
        }
    }
}