using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using McSntt.Models;
using McSntt.Views.Windows;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for CreateLogbookWindow.xaml
    /// </summary>
    public partial class CreateLogbookWindow : Window
    {
        public List<Person> CrewList = new List<Person>();
 

        public CreateLogbookWindow()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var createCrewWindow = new CreateCrewWindow(CrewList);
            createCrewWindow.ShowDialog();


            CrewList = createCrewWindow._crewList;

            CrewDataGrid.ItemsSource = null;
            CrewDataGrid.ItemsSource = CrewList;
        }
    }
}
