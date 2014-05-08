using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using McSntt.DataAbstractionLayer.Sqlite;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            DatabaseManager.InitializeDatabase();

            var boatDal = new BoatSqliteDal();
            var eventDal = new EventSqliteDal();
            var lectureDal = new LectureSqliteDal();
            var logbookDal = new LogbookSqliteDal();
            var personDal = new PersonSqliteDal();
            var tripDal = new RegularTripSqliteDal();
            var memberDal = new SailClubMemberSqliteDal();
            var studentDal = new StudentMemberSqliteDal();
            var teamDal = new TeamSqliteDal();

            // ===== BOATS =====
            #region Boat Testing
            var boats = new Boat[]
                        {
                            new Boat()
                            {
                                NickName = "Trololol",
                                Operational = true,
                                Type = BoatType.Gaffelrigger,
                                ImagePath = "SundetLogo.png"
                            }, 
                            new Boat()
                            {
                                NickName = "Sinky",
                                Operational = false,
                                Type = BoatType.Drabant,
                                ImagePath = "SundetLogo.png"
                            }
                        };

            bool resultCreateBoat = boatDal.Create(boats);

            MessageBox.Show(String.Format("Success: {0}\r\nTeams: {1} ({2}), {3} ({4})", resultCreateBoat, boats[0].NickName, boats[0].BoatId, boats[1].NickName, boats[1].BoatId));

            boats[0].NickName = "Tralalala";

            bool resultUpdateBoat = boatDal.Update(boats);

            MessageBox.Show(String.Format("Success: {0}\r\nTeams: {1} ({2}), {3} ({4})", resultUpdateBoat, boats[0].NickName, boats[0].BoatId, boats[1].NickName, boats[1].BoatId));
            #endregion


            // ===== TEAMS =====
            #region Team Testing
            var teams = new Team[]
                       {
                           new Team()
                           {
                               Name = "Team Funk",
                               Level = Team.ClassLevel.First
                           },
                           new Team()
                           {
                               Name = "Team Groovy",
                               Level = Team.ClassLevel.Second
                           }
                       };

            bool resultCreate = teamDal.Create(teams);

            MessageBox.Show(String.Format("Success: {0}\r\nTeams: {1} ({2}), {3} ({4})", resultCreate, teams[0].Name, teams[0].TeamId, teams[1].Name, teams[1].TeamId));

            teams[0].Name = "Team Funky";

            bool resultUpdate = teamDal.Update(teams);

            MessageBox.Show(String.Format("Success: {0}\r\nTeams: {1} ({2}), {3} ({4})", resultCreate, teams[0].Name, teams[0].TeamId, teams[1].Name, teams[1].TeamId));
            #endregion

            this.Shutdown(1);
        }
    }
}
