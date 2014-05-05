using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
        private IBoatDal boatDal = DalLocator.BoatDal;
        private IRegularTripDal regularTripDal = DalLocator.RegularTripDal;
        private ILogbookDal logbookDal = DalLocator.LogbookDal;

        private readonly RegularTrip RegularSailTrip1 = new RegularTrip();
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

        }


        private void BoatComboBox_OnSelectionChanged(object sender, EventArgs e)
        {
            if (BoatComboBox.SelectedIndex != -1)
            {
                CurrentBoat = (Boat) BoatComboBox.SelectionBoxItem;
                
                IEnumerable<RegularTrip> ListOfTrips =  regularTripDal.GetRegularTrips(x => x.Boat.BoatId == CurrentBoat.BoatId
                                                                                       && x.Logbook != null );

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
                LogbookDataGrid.ItemsSource = ListOfTrips;

            }
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
            CurrentSailtrip = (RegularTrip)LogbookDataGrid.SelectedItem;
            if (CurrentSailtrip == null)
            {
                MessageBox.Show("Vælg venligst en Logbog du gerne vil se");
            }
            else
            {
                var DamageReportWindow = new DamageReportWindow(CurrentSailtrip);
                DamageReportWindow.ShowDialog();
                CurrentSailtrip.Logbook.AnswerFromBoatChief = DamageReportWindow.DamageReport;
                logbookDal.Update(CurrentSailtrip.Logbook);
            }
        }
    }
}