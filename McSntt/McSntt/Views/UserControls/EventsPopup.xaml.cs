using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
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
using McSntt.Models;
using McSntt.Views.UserControls;
using McSntt.Views.Windows;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class EventsPopup : Window
    {


        public Event newEvent = new Event();

        public EventsPopup(Event newEvent)
        {
            InitializeComponent();
            
            this.newEvent = newEvent;
            
            ChooseDate.Value = DateTime.Today;

            newEvent.Created = false;
        }

        public EventsPopup(
            string selectedName, 
            DateTime selectedDate,
            string selectedDescription,
            bool SignUpReq)
        {
            InitializeComponent();

            //this.newEvent = newEvent;

            EventNameBox.Text = selectedName;
            EventDescriptionBox.Text = selectedDescription;
            ChooseDate.Value = selectedDate;
            SubscriptionCheckbox.IsChecked = SignUpReq;
        }

        private void Create_Event(object sender, RoutedEventArgs e)
        {

            #region If not filled check

            if (!string.IsNullOrEmpty(EventNameBox.Text))
            {
                newEvent.EventTitle = EventNameBox.Text;
            }

            if (!string.IsNullOrEmpty(EventDescriptionBox.Text))
            {
                newEvent.Description = EventDescriptionBox.Text;
            }

            #endregion

            // Get the EventDate as Value or Default Value
            newEvent.EventDate = ChooseDate.Value.GetValueOrDefault();

            #region Warning messages

            if (string.IsNullOrEmpty(EventNameBox.Text))
            {
                MessageBox.Show("Du mangler at angive en titel!");
            }

            else if (string.IsNullOrEmpty(EventDescriptionBox.Text))
            {
                MessageBox.Show("Du mangler at angive en beskrivelse!");
            }

            #endregion

            if (SubscriptionCheckbox.IsChecked == true)
            {
                newEvent.SignUpReq = true;
                newEvent.SignUpMsg = "Tilmelding krævet!";
            }

            if (!string.IsNullOrEmpty(EventNameBox.Text) && !string.IsNullOrEmpty(EventDescriptionBox.Text))
            {
                newEvent.Created = true;
                this.Close();
            }
        }
    }
}