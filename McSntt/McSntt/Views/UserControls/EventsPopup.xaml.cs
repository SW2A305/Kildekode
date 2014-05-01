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
        public IList<Event> Events = new List<Event>();
        public EventsPopup(IList<Event> Events)
        {
            InitializeComponent();

            this.Events = Events;

            ChooseDate.Value = DateTime.Today;
        }

        private void Create_Event(object sender, RoutedEventArgs e)
        {
            var newEvent = new Event();

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

            #region Warning messages

            if (string.IsNullOrEmpty(EventNameBox.Text))
            {
                MessageBox.Show("Du mangler at angive en titel!");
            }

            else if (string.IsNullOrEmpty(EventDescriptionBox.Text))
            {
                MessageBox.Show("Du mangler at angive en beskrivelse!");
            }

            if (SubscriptionCheckbox.IsChecked == true)
            {
                newEvent.SignUpReq = true;
            }

            #endregion

            // Get the EventDate as Value or Default Value
            newEvent.EventDate = ChooseDate.Value.GetValueOrDefault();
/*
            IEventDal db = new EventEfDal();
            db.Create(newEvent);
*/
            if (!string.IsNullOrEmpty(EventNameBox.Text) && !string.IsNullOrEmpty(EventDescriptionBox.Text))
            {
                Events.Add(newEvent);



                this.Close();
            }
        }
    }
}