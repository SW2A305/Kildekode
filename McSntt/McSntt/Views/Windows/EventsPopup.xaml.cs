using System;
using System.Windows;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    ///     Interaction logic for Window1.xaml
    /// </summary>
    public partial class EventsPopup : Window
    {
        private readonly bool IsUpdate;
        public Event newEvent = new Event();

        public EventsPopup(Event newEvent)
        {
            this.InitializeComponent();

            this.newEvent = newEvent;

            this.EventNameBox.Text = newEvent.EventTitle;
            this.EventDescriptionBox.Text = newEvent.Description;
            this.ChooseDate.Value = newEvent.EventDate;
            this.SubscriptionCheckbox.IsChecked = newEvent.SignUpReq;

            if (newEvent.EventDate == default(DateTime)) { this.ChooseDate.Value = DateTime.Today; }
            newEvent.Created = false;
            this.Label.Content = "Rediger begivenhed";

            Button.Content = "Rediger";
            this.IsUpdate = true;
        }

        public EventsPopup()
        {
            this.InitializeComponent();

            this.EventNameBox.Text = "";
            this.EventDescriptionBox.Text = "";
            this.ChooseDate.Value = DateTime.Now;
            this.SubscriptionCheckbox.IsChecked = false;

            this.newEvent.Created = false;
            this.Label.Content = "Opret begivenhed";
        }

        private void Create_Event(object sender, RoutedEventArgs e)
        {
            #region If not filled check
            if (!string.IsNullOrEmpty(this.EventNameBox.Text)) { this.newEvent.EventTitle = this.EventNameBox.Text; }

            if (!string.IsNullOrEmpty(this.EventDescriptionBox.Text)) {
                this.newEvent.Description = this.EventDescriptionBox.Text;
            }
            #endregion

            // Get the EventDate as Value or Default Value
            this.newEvent.EventDate = this.ChooseDate.Value;

            #region Warning messages
            if (string.IsNullOrEmpty(this.EventNameBox.Text)) {
                MessageBox.Show("Du mangler at angive en titel!");
            }

            else if (string.IsNullOrEmpty(this.EventDescriptionBox.Text)) {
                MessageBox.Show("Du mangler at angive en beskrivelse!");
            }
            #endregion

            if (this.SubscriptionCheckbox.IsChecked == true)
            {
                this.newEvent.SignUpReq = true;
                this.newEvent.SignUpMsg = "Tilmelding krævet!";
            }
            else
            {
                this.newEvent.SignUpReq = false;
                this.newEvent.SignUpMsg = "";
            }

            if (!string.IsNullOrEmpty(this.EventNameBox.Text) && !string.IsNullOrEmpty(this.EventDescriptionBox.Text))
            {
                this.newEvent.Created = true;
                if (this.IsUpdate) {
                    DalLocator.EventDal.Update(this.newEvent);
                }
                else
                {
                    DalLocator.EventDal.Create(this.newEvent);
                }
                this.Close();
            }
        }
    }
}
