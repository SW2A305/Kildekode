using System;
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
using McSntt.Views.Windows;
using McSntt.Models;

namespace McSntt.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Events.xaml
    /// </summary>
    public partial class EventsAdmin : UserControl
    {

        //public Event newEvent = new Event();
        public IList<Event> EventsList = new List<Event>();

        public EventsAdmin()
        {
            InitializeComponent();
        }
        private void Create_Event(object sender, RoutedEventArgs e)
        {
           
            var newEvent = new Event();
            Window createEventPopup = new EventsPopup(newEvent);

            createEventPopup.ShowDialog();
            AgendaListbox.ItemsSource = EventsList;

            if (newEvent.Created)
            {
                EventsList.Add(newEvent);
                AgendaListbox.Items.Refresh();
            }


            // Lige nu bruges listen fra Event klasse  ikke, den ville ikke twerke med den :(
            //newEvent.EventList.Add(newEvent);
            //AgendaListbox.ItemsSource = newEvent.EventList;


        }

        private void Edit_Event(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Event(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Begivenhed er slettet");
        }

        private void Subscripe(object sender, RoutedEventArgs e)
        {

        }

        private void Show_Participants(object sender, RoutedEventArgs e)
        {
            Window showParticipants = new ParticipantsPopup();
            showParticipants.Show();
        }
    }
}