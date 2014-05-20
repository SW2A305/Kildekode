using System;
using System.Linq;
using System.Windows;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    ///     Interaction logic for CreateAndEditBoats.xaml
    /// </summary>
    public partial class CreateAndEditBoats : Window
    {
        private readonly Boat newBoat = new Boat();

        public CreateAndEditBoats()
        {
            this.InitializeComponent();

            this.CheckBox.IsChecked = true;
            this.BoatTypeComboBox.ItemsSource = Enum.GetValues(typeof (BoatType)).Cast<BoatType>();
        }

        public CreateAndEditBoats(Boat boat) : this()
        {
            this.CheckBox.IsChecked = boat.Operational;
            this.BoatTypeComboBox.SelectedIndex = (int) boat.Type;
            this.NickNameTextBox.Text = boat.NickName;
            this.SaveButton.Click -= this.Button_Click;
            this.SaveButton.Click += this.EditButton_Click;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.NickNameTextBox.Text == String.Empty || this.NickNameTextBox.Text == "Skriv navn på båden") {
                MessageBox.Show("Giv Venligst båden et navn.");
            }
            else if (this.BoatTypeComboBox.SelectedIndex == -1) {
                MessageBox.Show("Vælg venligst en Bådtype for båden");
            }
            else
            {
                this.newBoat.NickName = this.NickNameTextBox.Text;
                this.newBoat.Type = (BoatType) this.BoatTypeComboBox.SelectedItem;
                this.newBoat.ImagePath = "SundetLogo.png";
                this.newBoat.Operational = this.CheckBox.IsChecked.GetValueOrDefault();
                DalLocator.BoatDal.Create(this.newBoat);
                this.Close();
                MessageBox.Show("Båden er nu oprettet");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NickNameTextBox.Text == String.Empty || this.NickNameTextBox.Text == "Skriv navn på båden") {
                MessageBox.Show("Giv Venligst båden et navn.");
            }
            else if (this.BoatTypeComboBox.SelectedIndex == -1) {
                MessageBox.Show("Vælg venligst en Bådtype for båden");
            }
            else
            {
                this.newBoat.NickName = this.NickNameTextBox.Text;
                this.newBoat.Type = (BoatType) this.BoatTypeComboBox.SelectedItem;
                this.newBoat.Operational = CheckBox.IsChecked == true;
                DalLocator.BoatDal.Update(this.newBoat);
                this.Close();
            }
        }

        private void NickNameTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            this.NickNameTextBox.Text = String.Empty;
        }
    }
}
