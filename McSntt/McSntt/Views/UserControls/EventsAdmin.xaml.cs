using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using McSntt.DataAbstractionLayer;
using McSntt.Helpers;
using McSntt.Models;
using McSntt.Views.Windows;

namespace McSntt.Views.UserControls
{
    /// <summary>
    ///     Interaction logic for Events.xaml
    /// </summary>
    public partial class EventsAdmin : UserControl
    {
        public IList<Event> EventsList = new List<Event>();

        public EventsAdmin()
        {
            this.InitializeComponent();

            if (GlobalInformation.CurrentUser.Position != SailClubMember.Positions.Admin)
            {
                this.CreateBnt.Visibility = Visibility.Hidden;
                this.EditBnt.Visibility = Visibility.Hidden;
                this.DeleteBnt.Visibility = Visibility.Hidden;
            }
            IEventDal eventDal = DalLocator.EventDal;

            this.EventsList = eventDal.GetAll().ToList();
            eventDal.LoadData(this.EventsList);
            this.EventsList = this.EventsList.OrderBy(x => x.EventDate).ToList();
            this.AgendaListbox.ItemsSource = this.EventsList;
        }

        private void Create_Event(object sender, RoutedEventArgs e)
        {
            Window createEventPopup = new EventsPopup();
            createEventPopup.ShowDialog();
            this.AgendaListbox.ItemsSource = this.EventsList;

            this.EventsList = DalLocator.EventDal.GetAll().ToList();
            DalLocator.EventDal.LoadData(this.EventsList);

                this.EventsList = this.EventsList.OrderBy(x => x.EventDate).ToList();

                this.AgendaListbox.ItemsSource = this.EventsList;

                this.AgendaListbox.Items.Refresh();
            
        }

        private void Edit_Event(object sender, RoutedEventArgs e)
        {
            int i = this.AgendaListbox.SelectedIndex;

            if (i >= 0)
            {
                Event selectedEvent = this.EventsList.ElementAt(i);

                Window createEventPopup = new EventsPopup(selectedEvent);
                createEventPopup.ShowDialog();

                this.EventsList.RemoveAt(i);

                this.EventsList.Insert(i, selectedEvent);

                this.EventsList = this.EventsList.OrderBy(x => x.EventDate).ToList();

                this.AgendaListbox.ItemsSource = this.EventsList;

                this.AgendaListbox.Items.Refresh();

                this.Descriptionbox.Text = null;

                this.Descriptionbox.Text = this.AgendaListbox.SelectedItem.ToString();

                DalLocator.EventDal.Update(selectedEvent);
            }
            else
            {
                MessageBox.Show("Vælg en begivenhed at redigere!");
            }
        }

        private void Delete_Event(object sender, RoutedEventArgs e)
        {
            int i = this.AgendaListbox.SelectedIndex;

            if (i >= 0)
            {
                DalLocator.EventDal.Delete(this.EventsList[i]);

                this.EventsList = DalLocator.EventDal.GetAll().ToList();
                DalLocator.EventDal.LoadData(this.EventsList);
                this.AgendaListbox.ItemsSource = this.EventsList;
            }
            else
            {
                MessageBox.Show("Vælg en begivenhed som skal slettes!");
            }

            this.AgendaListbox.Items.Refresh();
        }

        private void Subscribe(object sender, RoutedEventArgs e)
        {
            int i = this.AgendaListbox.SelectedIndex;

            if (i >= 0)
            {
                Event selectedEvent = this.EventsList.ElementAt(i);

                if (selectedEvent.SignUpReq)
                {
                    if (selectedEvent.Participants == null) { selectedEvent.Participants = new List<Person>(); }

                    if (selectedEvent.Participants.Any(x => x.PersonId == GlobalInformation.CurrentUser.PersonId)) {
                        MessageBox.Show("Du er allerede tilmeldt!");
                    }
                    else
                    {
                        selectedEvent.Participants.Add(GlobalInformation.CurrentUser);
                        DalLocator.EventDal.Update(selectedEvent);
                        MessageBox.Show("Du er nu tilmeldt!");
                    }
                }
                else
                {
                    MessageBox.Show("Der er ikke krævet tilmelding på denne begvenhed!");
                }
            }
            else
            {
                MessageBox.Show("Vælg en begivenhed at tilmelde!");
            }
        }

        private void Show_Participants(object sender, RoutedEventArgs e)
        {
            int i = this.AgendaListbox.SelectedIndex;

            if (i >= 0)
            {
                Event selectedEvent = this.EventsList.ElementAt(i);

                if (selectedEvent.SignUpReq == false) {
                    MessageBox.Show("Der er ikke krævet tilmelding på denne begivenhed!");
                }
                else
                {
                    Window showParticipants = new ParticipantsPopup(selectedEvent);
                    showParticipants.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Vælg en begivenhed!");
            }
        }

        private void Unsubscribe(object sender, RoutedEventArgs e)
        {
            int i = this.AgendaListbox.SelectedIndex;

            if (i >= 0)
            {
                Event selectedEvent = this.EventsList.ElementAt(i);

                if (selectedEvent.SignUpReq)
                {
                    if (selectedEvent.Participants != null)
                    {
                        if (selectedEvent.Participants.Any(x => x.PersonId == GlobalInformation.CurrentUser.PersonId))
                        {
                            Person currentUser =
                                selectedEvent.Participants.FirstOrDefault(
                                                                          x =>
                                                                          x.PersonId
                                                                          == GlobalInformation.CurrentUser.PersonId);

                            selectedEvent.Participants.Remove(currentUser);
                            MessageBox.Show("Du er nu frameldt!");
                            DalLocator.EventDal.Update(selectedEvent);
                        }
                        else
                        {
                            MessageBox.Show("Du er ikke tilmeldt begivenheden!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Der er ikke nogen tilmeldte til begivenheden!");
                    }
                }
                else
                {
                    MessageBox.Show("Der er ikke krævet tilmelding på denne begivenhed!");
                }
            }
            else
            {
                MessageBox.Show("Vælg en begivenhed at framelde dig fra!");
            }
        }

        private void AgendaListbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AgendaListbox.SelectedItem != null) {
                this.Descriptionbox.Text = this.AgendaListbox.SelectedItem.ToString();
            }
            else
            {
                this.Descriptionbox.Text = String.Empty;
            }
        }
    }
}
