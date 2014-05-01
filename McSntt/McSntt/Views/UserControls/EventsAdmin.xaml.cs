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

namespace McSntt.Views.UserControls
{
    /// <summary>
    /// Interaction logic for Events.xaml
    /// </summary>
    public partial class EventsAdmin : UserControl
    {
        public EventsAdmin()
        {
            InitializeComponent();
        }
        private void Create_Event(object sender, RoutedEventArgs e)
        {
            Window createEventPopup = new EventsPopup();
            createEventPopup.Show();
        }

        private void Edit_Event(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Rediger Begivenhed");
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