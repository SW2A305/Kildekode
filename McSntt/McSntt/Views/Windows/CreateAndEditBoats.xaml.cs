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
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for CreateAndEditBoats.xaml
    /// </summary>
    public partial class CreateAndEditBoats : Window
    {
        private Boat newBoat = new Boat();
        public CreateAndEditBoats()
        {
            InitializeComponent();

            CheckBox.IsChecked = true;
            BoatTypeComboBox.ItemsSource = Enum.GetValues(typeof (BoatType)).Cast<BoatType>();
        }

        public CreateAndEditBoats(Boat boat) : this()
        {
            CheckBox.IsChecked = boat.Operational;
            BoatTypeComboBox.SelectedIndex = (int)boat.Type;
            NickNameTextBox.Text = boat.NickName;
            SaveButton.Click -= Button_Click;
            SaveButton.Click += EditButton_Click;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NickNameTextBox.Text == String.Empty || NickNameTextBox.Text == "Skriv navn på båden")
            {
                MessageBox.Show("Giv Venligst båden et navn.");
            }
            else if (BoatTypeComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Vælg venligst en Bådtype for båden");
            }
            else
            {
                newBoat.NickName = NickNameTextBox.Text;
                newBoat.Type = (BoatType) BoatTypeComboBox.SelectedItem;
                newBoat.ImagePath = "SundetLogo.png";
                newBoat.Operational = CheckBox.IsChecked.GetValueOrDefault();
                DalLocator.BoatDal.Create(newBoat);
                this.Close();
                MessageBox.Show("Båden er nu oprettet");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (NickNameTextBox.Text == String.Empty || NickNameTextBox.Text == "Skriv navn på båden")
            {
                MessageBox.Show("Giv Venligst båden et navn.");
            }
            else if (BoatTypeComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Vælg venligst en Bådtype for båden");
            }
            else
            {
                newBoat.NickName = NickNameTextBox.Text;
                newBoat.Type = (BoatType)BoatTypeComboBox.SelectedItem;
                newBoat.Operational = CheckBox.IsChecked.GetValueOrDefault();
                DalLocator.BoatDal.Update(newBoat);
            }
        }

        private void NickNameTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            NickNameTextBox.Text = String.Empty;
        }
    }
}
