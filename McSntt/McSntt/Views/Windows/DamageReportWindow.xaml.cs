using System;
using System.Windows;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    ///     Interaction logic for DamageReportWindow.xaml
    /// </summary>
    public partial class DamageReportWindow : Window
    {
        private readonly RegularTrip currentTrip;
        public string DamageReport;
        public bool IsAnswered = false;

        public DamageReportWindow(RegularTrip regularSailTrip)
        {
            this.InitializeComponent();
            this.currentTrip = regularSailTrip;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DamageReport = this.DamageTextBox.Text;
            if (this.DamageReport != String.Empty) { this.IsAnswered = true; }

            if (this.OperationalCheckBox.IsChecked == true) { this.currentTrip.Boat.Operational = false; }
            DalLocator.BoatDal.Update(this.currentTrip.Boat);

            this.Close();

            //update the element sent to the windows damagereportanswer
        }
    }
}
