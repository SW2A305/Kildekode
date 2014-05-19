using System.Collections.Generic;
using System.Windows;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    ///     Interaction logic for ParticipantsPopup.xaml
    /// </summary>
    public partial class ParticipantsPopup : Window
    {
        public ParticipantsPopup(Event participant)
        {
            this.InitializeComponent();

            ICollection<Person> participantsList = participant.Participants;

            this.ParticipantsListbox.ItemsSource = participantsList;

            this.ParticipantsListbox.Items.Refresh();
        }

        private void Close(object sender, RoutedEventArgs e) { this.Close(); }
    }
}
