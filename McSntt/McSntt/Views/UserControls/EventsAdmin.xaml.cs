using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.AccessControl;
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
using McSntt.Helpers;
using McSntt.Views.Windows;
using McSntt.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace McSntt.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Events.xaml
    /// </summary>
    public partial class EventsAdmin : UserControl
    {

        public IList<Event> EventsList = new List<Event>();

        //public ICollection<Person> Participants = new List<Person>();

        //public Event newEvent = new Event();

        public EventsAdmin()
        {
            InitializeComponent();

            if (GlobalInformation.CurrentUser.Position != SailClubMember.Positions.Admin)
            {
                CreateBnt.Visibility = Visibility.Hidden;
                EditBnt.Visibility = Visibility.Hidden;
                DeleteBnt.Visibility = Visibility.Hidden;
            }
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

                EventsList = EventsList.OrderBy(x => x.EventDate).ToList();

                AgendaListbox.ItemsSource = EventsList;

                AgendaListbox.Items.Refresh();
            }
        }

        private void Edit_Event(object sender, RoutedEventArgs e)
        {
            int i = AgendaListbox.SelectedIndex;

            if (i >= 0)
            {
                var selectedEvent = EventsList.ElementAt(i);

                Window createEventPopup = new EventsPopup(selectedEvent);
                createEventPopup.ShowDialog();

                EventsList.RemoveAt(i);

                EventsList.Insert(i, selectedEvent);

                EventsList = EventsList.OrderBy(x => x.EventDate).ToList();

                AgendaListbox.ItemsSource = EventsList;

                AgendaListbox.Items.Refresh();
            }
            else MessageBox.Show("Vælg en begivenhed at redigere");
            
        }

        private void Delete_Event(object sender, RoutedEventArgs e)
        {
            int i = AgendaListbox.SelectedIndex;

            if (i >= 0)
            {
                EventsList.RemoveAt(i);
            }
            else MessageBox.Show("Vælg en begivenhed som skal slettes");

            AgendaListbox.Items.Refresh();
        }

        private void Subscribe(object sender, RoutedEventArgs e)
        {
            int i = AgendaListbox.SelectedIndex;

            if (i >= 0)
            {



                

                var selectedEvent = EventsList.ElementAt(i);

                selectedEvent.Participants = new List<Person>();

                selectedEvent.Participants.Add(GlobalInformation.CurrentUser);



                //newEvent.Participants = Participants;



                //newEvent.Participants.Add(GlobalInformation.CurrentUser);

                //Textbox.Text = newEvent.Participants.ToString();
            }
            else MessageBox.Show("Vælg en begivenhed at tilmelde");
        }

        private void Show_Participants(object sender, RoutedEventArgs e)
        {
            
            int i = AgendaListbox.SelectedIndex;

            if (i >= 0)
            {
                var selectedEvent = EventsList.ElementAt(i);
                Window showParticipants = new ParticipantsPopup(selectedEvent);
                showParticipants.ShowDialog();
            }
        }

        private void AgendaListbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AgendaListbox.SelectedItem != null)
            {
                Descriptionbox.Text = AgendaListbox.SelectedItem.ToString();
            }
            else Descriptionbox.Text = String.Empty;
        }
    }
}
