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
    /// Interaction logic for DamageReportWindow.xaml
    /// </summary>
    public partial class DamageReportWindow : Window
    {
        public string DamageReport;
        public bool IsAnswered = false;

        public DamageReportWindow(RegularTrip regularSailTrip)
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DamageReport = DamageTextBox.Text;
            if (DamageReport != String.Empty)
            {
                IsAnswered = true;
            }
            this.Close();

            //update the element sent to the windows damagereportanswer

        }
    }
}
