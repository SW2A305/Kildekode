using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Windows;
using System.Windows.Forms;
using McSntt.DataAbstractionLayer;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for CreateBoatBooking.xaml
    /// </summary>
    public partial class CreateBoatBookingWindow : Window
    {
        public CreateBoatBookingWindow()
        {
            InitializeComponent();
            
            var dbm = new BoatEfDal();
            BoatComboBox.ItemsSource = dbm.GetAll();
            BoatComboBox.DisplayMemberPath = "NickName";
            BoatComboBox.SelectedValuePath = "Id";

            CaptainComboBox.DisplayMemberPath = "FirstName";
            CaptainComboBox.SelectedValuePath = "MemberId";

            DateTimeStart.Value = DateTime.Now;
            DateTimeEnd.Value = DateTime.Now;
        }

        private void BoatComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            
        }
        public List<Person> CrewList = new List<Person>();

        private void ChangeCrew_Click(object sender, RoutedEventArgs e)
        {
            var createCrewWindow = new CreateCrewWindow(CrewList);
            createCrewWindow.ShowDialog();
            
            CrewList = createCrewWindow._crewList;

            CrewDataGrid.ItemsSource = null;
            CrewDataGrid.ItemsSource = CrewList;
            CaptainComboBox.ItemsSource = CrewList;
        }

        private void CaptainComboBox_OnDropDownClosed(object sender, EventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Check if this is a valid trip. If not return saying error. 
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Add warning window
        }


    }
}
