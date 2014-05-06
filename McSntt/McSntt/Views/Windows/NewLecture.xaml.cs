using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using McSntt.DataAbstractionLayer;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    /// Interaction logic for NewLecture.xaml
    /// </summary>
    public partial class NewLecture : Window
    {
        private Team _currentTeam;
        public NewLecture(object currentTeam)
        {
            InitializeComponent();
            _currentTeam = currentTeam as Team;
        }



        private void CompleteLectureCreate_Click(object sender, RoutedEventArgs e)
        {
            var lecture = new Lecture
            {
                DateOfLecture = DateTimePickerPlannedLectureTime.Value.GetValueOrDefault()
            };
            DalLocator.LectureDal.Create(lecture);
            var Departure = DateTimePickerPlannedLectureTime.Value.GetValueOrDefault();
            var Arrival = DateTimePickerPlannedLectureTime_Copy.Value.GetValueOrDefault();
            var book = new CreateBoatBookingWindow(Departure, Arrival, _currentTeam);
        }
    }
}
