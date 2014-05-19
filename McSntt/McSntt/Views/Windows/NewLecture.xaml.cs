using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Xceed.Wpf.DataGrid.Converters;

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
                DateOfLecture = DateTimePickerPlannedLectureTime.Value
            };
            lecture.TeamId = _currentTeam.TeamId;
            lecture.PresentMembers = new Collection<StudentMember>();
            DalLocator.LectureDal.Create(lecture);
            var Departure = DateTimePickerPlannedLectureTime.Value;
            var Arrival = DateTimePickerPlannedLectureTimeEnd.Value;

            var boat = DalLocator.BoatDal.GetAll(
                x => x.Type == (_currentTeam.Level == Team.ClassLevel.First ? BoatType.Gaffelrigger : BoatType.Drabant));

            var trip = new RegularTrip()
            {
                ArrivalTime = Arrival,
                Boat = boat.First(),
                Captain = _currentTeam.Teacher,
                Crew = _currentTeam.TeamMembers.Cast<Person>().ToList(),
                CreatedBy = GlobalInformation.CurrentUser,
                DepartureTime = Departure,
                PurposeAndArea = "Undervisning med hold " + _currentTeam.Name,
                WeatherConditions = ""
            };
            DalLocator.RegularTripDal.Create(trip);
            
            
            MessageBox.Show("Lektionen er nu oprettet");
            this.Close();
        }
    }
}
