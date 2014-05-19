using System;
using System.Windows;
using McSntt.Models;

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

            EventNameBox.Text = newEvent.EventTitle;
            EventDescriptionBox.Text = newEvent.Description;
            ChooseDate.Value = newEvent.EventDate;
            SubscriptionCheckbox.IsChecked = newEvent.SignUpReq;

            if (newEvent.EventDate == default(DateTime))
            {
                ChooseDate.Value = DateTime.Today;
            }
            newEvent.Created = false;
            Label.Content = "Rediger begivenhed";
        }

        public EventsPopup()
        {
            InitializeComponent();

            EventNameBox.Text = "";
            EventDescriptionBox.Text = "";
            ChooseDate.Value = DateTime.Now;
            SubscriptionCheckbox.IsChecked = false;

            newEvent.Created = false;
            Label.Content = "Opret begivenhed";

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
            newEvent.EventDate = ChooseDate.Value;

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
            else
            {
                newEvent.SignUpReq = false;
                newEvent.SignUpMsg = "";
            }

            if (!string.IsNullOrEmpty(EventNameBox.Text) && !string.IsNullOrEmpty(EventDescriptionBox.Text))
            {
                newEvent.Created = true;
                this.Close();
            }
        }
    }
}