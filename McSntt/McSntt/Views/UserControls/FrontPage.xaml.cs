﻿using System;
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
                "Til højre ses dine kommende ture, samt dem hvorpå der endnu ikker er udfyldt en logbog for.";

            LoadData();
        }

        public void LoadData()
        {
            var db = new RegularTripEfDal();

            var sailTripList = db.GetAll().Where(t => t.Crew.Contains(GlobalInformation.CurrentUser)).ToList();

            UpcommingTripsDataGrid.ItemsSource = null;
            UpcommingTripsDataGrid.ItemsSource = sailTripList.Where(t => t.DepartureTime > DateTime.Now);

            LogbookDataGrid.ItemsSource = null;
            LogbookDataGrid.ItemsSource = sailTripList.Where(t => t.ArrivalTime < DateTime.Now && t.Logbook == null);
        }

        private RegularTrip _regularSailTrip = new RegularTrip();
        private RegularTrip _regularSailTrip2 = new RegularTrip();

        private void LogbookDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var logBookWindow = new CreateLogbookWindow(GlobalInformation.CurrentUser);
            logBookWindow.ShowDialog();

            /* TODO: after database.
            LogbookDataGrid.ItemsSource = null;
            LogbookDataGrid.ItemsSource = SailTripList.Where(t => t.ArrivalTime < DateTime.Now && t.Logbook == null); */
        }

        private void UpcommingTripsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //TODO: Show info about up and comming trip
        }
    }
}
