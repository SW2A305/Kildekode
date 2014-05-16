﻿using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using McSntt.DataAbstractionLayer.Mock;

namespace McSntt.Helpers
{
    public static class MockToSqlite
    {
        #region Table Names
        public const string TableBoats = "boats";
        public const string TableDbSettings = "db_settings";
        public const string TableEvents = "events";
        public const string TableEventParticipantsBinder = "event_participants_binder";
        public const string TableLectures = "lectures";
        public const string TableLecturePresentMembersBinder = "lecture_present_members_binder";
        public const string TableLogbooks = "logbooks";
        public const string TableLogbookActualCrewBinder = "logbook_actual_crew_binder";
        public const string TablePersons = "persons";
        public const string TableRegularTrips = "regular_trips";
        public const string TableRegularTripCrewBinder = "regular_trip_crew_binder";
        public const string TableSailClubMembers = "sail_club_members";
        public const string TableStudentMembers = "student_members";
        public const string TableTeams = "teams";
        #endregion

        private const string AppName = "McSnttt";
        private const string DbFileName = "McSntttMock.db";

        private static string _dbFilePath;

        private static string DbFilePath
        {
            get
            {
                if (String.IsNullOrEmpty(_dbFilePath))
                {
                    string roamingAppDataFolder =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);

                    if (!Directory.Exists(roamingAppDataFolder)) { Directory.CreateDirectory(roamingAppDataFolder); }

                    _dbFilePath = Path.Combine(roamingAppDataFolder, DbFileName);
                }

                return _dbFilePath;
            }
        }

        public static bool PersistMockData()
        {
            // Remove file if it exists
            if (File.Exists(DbFilePath)) { File.Delete(DbFilePath); }

            using (var conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                #region Create tables
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        #region Create table: Boats
                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("CREATE TABLE {0} (boat_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                              "type INTEGER, nickname TEXT, image_path TEXT, operational INTEGER" +
                                              ")",
                                              TableBoats);

                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create table: Events
                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (event_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                              "event_date TEXT, event_title TEXT, sign_up_req INTEGER, " +
                                              "description TEXT, sign_up_msg TEXT, created INTEGER" +
                                              ")",
                                              TableEvents);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create table: Lectures
                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (lecture_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                              "team_id INTEGER, date_of_lecture TEXT, rope_works INTEGER, " +
                                              "navigation INTEGER, motor INTEGER, drabant INTEGER, " +
                                              "gaffelrigger INTEGER, night INTEGER" +
                                              ")",
                                              TableLectures);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create table: Logbooks
                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (logbook_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                              "actual_departure_time TEXT, actual_arrival_time TEXT, " +
                                              "damage_inflicted INTEGER, damage_description TEXT, " +
                                              "answer_from_boat_chief TEXT, filed_by_id INTEGER" +
                                              ")",
                                              TableLogbooks);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create table: Persons
                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (person_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                              "first_name TEXT, last_name TEXT, address TEXT, postcode TEXT, " +
                                              "cityname TEXT, date_of_birth TEXT, boat_driver INTEGER, " +
                                              "gender INTEGER, phone_number TEXT, email TEXT" +
                                              ")",
                                              TablePersons);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create table: RegularTrips
                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (regular_trip_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                              "departure_time TEXT, arrival_time TEXT, boat_id INTEGER, " +
                                              "captain_id INTEGER, expected_arrival_time TEXT, " +
                                              "purpose_and_area TEXT, weather_conditions TEXT, " +
                                              "logbook_id INTEGER" +
                                              ")",
                                              TableRegularTrips);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create table: SailClubMembers
                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (" +
                                              "sail_club_member_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                              "person_id INTEGER, position INTEGER, username TEXT, password_hash TEXT" +
                                              ")",
                                              TableSailClubMembers);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create table: StudentMembers
                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (" +
                                              "student_member_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                              "sail_club_member_id INTEGER, team_id INTEGER, " +
                                              "rope_works INTEGER, navigation INTEGER, motor INTEGER, " +
                                              "drabant INTEGER, gaffelrigger INTEGER, night INTEGER" +
                                              ")",
                                              TableStudentMembers);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create table: Teams
                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (team_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                              "name TEXT, level INTEGER, teacher_id INTEGER" +
                                              ")",
                                              TableTeams);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create binding-table: EventParticipantsBinder
                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("CREATE TABLE {0} (" +
                                              "event_id TEXT, person_id INTEGER" +
                                              ")",
                                              TableEventParticipantsBinder);
                            command.ExecuteNonQuery();
                        }

                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("CREATE UNIQUE INDEX IF NOT EXISTS event_participants " +
                                              "ON {0} (event_id, person_id)",
                                              TableEventParticipantsBinder);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create binding-table: LecturePresentMembersBinder
                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (" +
                                              "lecture_id TEXT, student_member_id INTEGER" +
                                              ")",
                                              TableLecturePresentMembersBinder);
                            command.ExecuteNonQuery();
                        }

                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("CREATE UNIQUE INDEX IF NOT EXISTS lecture_present_members " +
                                              "ON {0} (lecture_id, student_member_id)",
                                              TableLecturePresentMembersBinder);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create binding-table: LogbookActualCrewBinder
                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (" +
                                              "logbook_id TEXT, person_id INTEGER" +
                                              ")",
                                              TableLogbookActualCrewBinder);
                            command.ExecuteNonQuery();
                        }

                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("CREATE UNIQUE INDEX IF NOT EXISTS logbook_actual_crew " +
                                              "ON {0} (logbook_id, person_id)",
                                              TableLogbookActualCrewBinder);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create binding-table: RegularTripCrewBinder
                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (" +
                                              "regular_trip_id TEXT, person_id INTEGER" +
                                              ")",
                                              TableRegularTripCrewBinder);
                            command.ExecuteNonQuery();
                        }

                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("CREATE UNIQUE INDEX IF NOT EXISTS regular_trip_crew " +
                                              "ON {0} (regular_trip_id, person_id)",
                                              TableRegularTripCrewBinder);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                #endregion

                #region Persist data: Boat
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var boatDal = new BoatMockDal();
                        var boats = boatDal.GetAll();

                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("INSERT INTO {0} (" +
                                              "boat_id, type, nickname, image_path, operational" +
                                              ") VALUES (" +
                                              "@boatId, @type, @nickname, @imagePath, @operational" +
                                              ")",
                                              TableBoats);

                            command.Parameters.Add("@boatId", DbType.Int64);
                            command.Parameters.Add("@type", DbType.Int32);
                            command.Parameters.Add("@nickname", DbType.String);
                            command.Parameters.Add("@imagePath", DbType.String);
                            command.Parameters.Add("@operational", DbType.Boolean);

                            foreach (var boat in boats)
                            {
                                command.Parameters["@boatId"].Value = boat.BoatId;
                                command.Parameters["@type"].Value = (int) boat.Type;
                                command.Parameters["@nickname"].Value = boat.NickName;
                                command.Parameters["@imagePath"].Value = boat.ImagePath;
                                command.Parameters["@operational"].Value = boat.Operational;

                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                #endregion

                #region Persist data: Event
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var eventDal = new EventMockDal();
                        var events = eventDal.GetAll();

                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("INSERT INTO {0} (" +
                                              "event_id, event_date, event_title, sign_up_req, description, " +
                                              "sign_up_msg, created" +
                                              ") VALUES (" +
                                              "@eventId, @eventDate, @eventTitle, @signUpReq, @description, " +
                                              "@signUpMsg, @created" +
                                              ")",
                                              TableEvents);

                            command.Parameters.Add("@eventId", DbType.Int64);
                            command.Parameters.Add("@eventDate", DbType.DateTime);
                            command.Parameters.Add("@eventTitle", DbType.String);
                            command.Parameters.Add("@signUpReq", DbType.Boolean);
                            command.Parameters.Add("@description", DbType.String);
                            command.Parameters.Add("@signUpMsg", DbType.String);
                            command.Parameters.Add("@created", DbType.Boolean);

                            foreach (var @event in events)
                            {
                                command.Parameters["@eventId"].Value = @event.EventId;
                                command.Parameters["@eventDate"].Value = @event.EventDate;
                                command.Parameters["@eventTitle"].Value = @event.EventTitle;
                                command.Parameters["@signUpReq"].Value = @event.SignUpReq;
                                command.Parameters["@description"].Value = @event.Description;
                                command.Parameters["@signUpMsg"].Value = @event.SignUpMsg;
                                command.Parameters["@created"].Value = @event.Created;

                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                #endregion

                #region Persist sub-data: Event->Participants
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var eventDal = new EventMockDal();
                        var events = eventDal.GetAll();

                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("INSERT INTO {0} (" +
                                              "event_id, person_id" +
                                              ") VALUES (" +
                                              "@eventId, @personId" +
                                              ")",
                                              TableEventParticipantsBinder);

                            command.Parameters.Add("@eventId", DbType.Int64);
                            command.Parameters.Add("@personId", DbType.Int64);

                            foreach (var @event in events)
                            {
                                command.Parameters["@eventId"].Value = @event.EventId;

                                foreach (var person in @event.Participants)
                                {
                                    command.Parameters["@personId"].Value = person.PersonId;

                                    command.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                #endregion

                #region Persist data: Lecture
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var lectureDal = new LectureMockDal();
                        var lectures = lectureDal.GetAll();

                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("INSERT INTO {0} (" +
                                              "lecture_id, date_of_lecture, rope_works, navigation, motor, " +
                                              "drabant, gaffelrigger, night, team_id" +
                                              ") VALUES (" +
                                              "@lectureId, @dateOfLecture, @ropeWorks, @navigation, @motor, " +
                                              "@drabant, @gaffelrigger, @night, @teamId" +
                                              ")",
                                              TableLectures);

                            command.Parameters.Add("@lectureId", DbType.Int64);
                            command.Parameters.Add("@dateOfLecture", DbType.DateTime);
                            command.Parameters.Add("@ropeWorks", DbType.Boolean);
                            command.Parameters.Add("@navigation", DbType.Boolean);
                            command.Parameters.Add("@motor", DbType.Boolean);
                            command.Parameters.Add("@drabant", DbType.Boolean);
                            command.Parameters.Add("@gaffelrigger", DbType.Boolean);
                            command.Parameters.Add("@night", DbType.Boolean);
                            command.Parameters.Add("@teamId", DbType.Int64);

                            foreach (var lecture in lectures)
                            {
                                command.Parameters["@lectureId"].Value = lecture.LectureId;
                                command.Parameters["@dateOfLecture"].Value = lecture.DateOfLecture;
                                command.Parameters["@ropeWorks"].Value = lecture.RopeWorksLecture;
                                command.Parameters["@navigation"].Value = lecture.Navigation;
                                command.Parameters["@motor"].Value = lecture.Motor;
                                command.Parameters["@drabant"].Value = lecture.Drabant;
                                command.Parameters["@gaffelrigger"].Value = lecture.Gaffelrigger;
                                command.Parameters["@night"].Value = lecture.Night;
                                command.Parameters["@teamId"].Value = lecture.TeamId;

                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                #endregion

                #region Persist sub-data: Lecture->PresentMembers
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var lectureDal = new LectureMockDal();
                        var lectures = lectureDal.GetAll();

                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("INSERT INTO {0} (" +
                                              "lecture_id, student_member_id" +
                                              ") VALUES (" +
                                              "@lectureId, @studentMemberId" +
                                              ")",
                                              TableLecturePresentMembersBinder);

                            command.Parameters.Add("@lectureId", DbType.Int64);
                            command.Parameters.Add("@studentMemberId", DbType.Int64);

                            foreach (var lecture in lectures)
                            {
                                command.Parameters["@lectureId"].Value = lecture.LectureId;

                                foreach (var student in lecture.PresentMembers)
                                {
                                    command.Parameters["@studentMemberId"].Value = student.StudentMemberId;

                                    command.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                #endregion

                #region Persist data: Logbook
                #endregion

                #region Persist sub-data: Logbook->ActualCrew
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var logbookDal = new LogbookMockDal();
                        var logbooks = logbookDal.GetAll();

                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("INSERT INTO {0} (" +
                                              "logbook_id, person_id" +
                                              ") VALUES (" +
                                              "@logbookId, @personId" +
                                              ")",
                                              TableLogbookActualCrewBinder);

                            command.Parameters.Add("@logbookId", DbType.Int64);
                            command.Parameters.Add("@personId", DbType.Int64);

                            foreach (var logbook in logbooks)
                            {
                                command.Parameters["@logbookId"].Value = logbook.LogbookId;

                                foreach (var person in logbook.ActualCrew)
                                {
                                    command.Parameters["@personId"].Value = person.PersonId;

                                    command.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                #endregion

                #region Persist data: Person
                #endregion

                #region Persist data: RegularTrip
                #endregion

                #region Persist sub-data: RegularTrip->Crew
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var tripDal = new RegularTripMockDal();
                        var trips = tripDal.GetAll();

                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("INSERT INTO {0} (" +
                                              "regular_trip_id, person_id" +
                                              ") VALUES (" +
                                              "@regularTripId, @personId" +
                                              ")",
                                              TableRegularTripCrewBinder);

                            command.Parameters.Add("@regularTripId", DbType.Int64);
                            command.Parameters.Add("@personId", DbType.Int64);

                            foreach (var trip in trips)
                            {
                                command.Parameters["@regularTripId"].Value = trip.RegularTripId;

                                foreach (var person in trip.Crew)
                                {
                                    command.Parameters["@personId"].Value = person.PersonId;

                                    command.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                #endregion

                #region Persist data: SailClubMember
                #endregion

                #region Persist data: StudentMember
                #endregion

                #region Persist data: Team
                #endregion

                conn.Close();
            }

            return true;
        }

        public static bool RestoreMockData() { throw new NotImplementedException("Not yet implemented!"); }

        private static string GetConnectionString()
        {
            var connStr = new SQLiteConnectionStringBuilder
                          {
                              DataSource = DbFilePath,
                              Version = 3,
                              DefaultTimeout = 5,
                              JournalMode = SQLiteJournalModeEnum.Persist
                          };

            return connStr.ToString();
        }
    }
}
