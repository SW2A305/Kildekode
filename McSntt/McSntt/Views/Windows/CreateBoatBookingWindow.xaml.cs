using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for CreateBoatBooking.xaml
    /// </summary>
    public partial class CreateBoatBookingWindow : Window
    {
        public CreateBoatBookingWindow()
        {
            InitializeComponent();
            using (var db = new McSntttContext())
            {
                db.Boats.Load();
                BoatComboBox.ItemsSource = db.Boats.Local;
                BoatComboBox.DisplayMemberPath = "NickName";
                BoatComboBox.SelectedValuePath = "Id";
            }
        }

        private void BoatComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            
        }
    }
}
