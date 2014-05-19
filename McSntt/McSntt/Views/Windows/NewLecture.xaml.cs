using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.Views.Windows
{
    /// <summary>
    ///     Interaction logic for NewLecture.xaml
    /// </summary>
    public partial class NewLecture : Window
    {
        private readonly Team _currentTeam;

        public NewLecture(object currentTeam)
        {
            this.InitializeComponent();
            this._currentTeam = currentTeam as Team;
        }


        private void CompleteLectureCreate_Click(object sender, RoutedEventArgs e)
        {
            var lecture = new Lecture
                          {
                              DateOfLecture = this.DateTimePickerPlannedLectureTime.Value
                          };
            lecture.TeamId = this._currentTeam.TeamId;
            lecture.PresentMembers = new Collection<StudentMember>();
            DalLocator.LectureDal.Create(lecture);
            DateTime Departure = this.DateTimePickerPlannedLectureTime.Value;
            DateTime Arrival = this.DateTimePickerPlannedLectureTimeEnd.Value;

            IEnumerable<Boat> boat = DalLocator.BoatDal.GetAll(
                                                               x =>
                                                               x.Type
                                                               == (this._currentTeam.Level == Team.ClassLevel.First
                                                                       ? BoatType.Gaffelrigger
                                                                       : BoatType.Drabant));

            var trip = new RegularTrip
                       {
                           ArrivalTime = Arrival,
                           Boat = boat.First(),
                           Captain = this._currentTeam.Teacher,
                           Crew = this._currentTeam.TeamMembers.Cast<Person>().ToList(),
                           CreatedBy = GlobalInformation.CurrentUser,
                           DepartureTime = Departure,
                           PurposeAndArea = "Undervisning med hold " + this._currentTeam.Name,
                           WeatherConditions = ""
                       };
            DalLocator.RegularTripDal.Create(trip);


            MessageBox.Show("Lektionen er nu oprettet");
            this.Close();
        }
    }
}
