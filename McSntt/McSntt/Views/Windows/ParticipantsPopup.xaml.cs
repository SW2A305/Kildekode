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
using System.Windows.Shapes;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for ParticipantsPopup.xaml
    /// </summary>
    public partial class ParticipantsPopup : Window
    {
        public ParticipantsPopup(Event participant)
        {
            InitializeComponent();

            var participantsList = participant.Participants;

            ParticipantsListbox.ItemsSource = participantsList;

            ParticipantsListbox.Items.Refresh();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}