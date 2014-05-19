using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using McSntt.DataAbstractionLayer.Mock;
using McSntt.Models;

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
            var runCount = 0;

            // Remove file if it exists
            if (File.Exists(DbFilePath)) { File.Delete(DbFilePath); }

            while (File.Exists(DbFilePath)) {
                System.Threading.Thread.Sleep(100);
                runCount++;

                if (runCount > 20) { break; }
            }

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
                                              "captain_id INTEGER, purpose_and_area TEXT, " +
                                              "weather_conditions TEXT, logbook_id INTEGER, created_by_id" +
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
                                              "logbook_id, actual_departure_time, actual_arrival_time, " +
                                              "damage_inflicted, damage_description, answer_from_boat_chief, " +
                                              "filed_by_id" +
                                              ") VALUES (" +
                                              "@logbookId, @actualDepartureTime, @actualArrivalTime, " +
                                              "@damageInflicted, @damageDescription, @answerFromBoatChief, " +
                                              "@filedById" +
                                              ")",
                                              TableLogbooks);

                            command.Parameters.Add("@logbookId", DbType.Int64);
                            command.Parameters.Add("@actualDepartureTime", DbType.DateTime);
                            command.Parameters.Add("@actualArrivalTime", DbType.DateTime);
                            command.Parameters.Add("@damageInflicted", DbType.Boolean);
                            command.Parameters.Add("@damageDescription", DbType.String);
                            command.Parameters.Add("@answerFromBoatChief", DbType.String);
                            command.Parameters.Add("@filedById", DbType.Int64);

                            foreach (var logbook in logbooks)
                            {
                                command.Parameters["@logbookId"].Value = logbook.LogbookId;
                                command.Parameters["@actualDepartureTime"].Value = logbook.ActualDepartureTime;
                                command.Parameters["@actualArrivalTime"].Value = logbook.ActualArrivalTime;
                                command.Parameters["@damageInflicted"].Value = logbook.DamageInflicted;
                                command.Parameters["@damageDescription"].Value = logbook.DamageDescription;
                                command.Parameters["@answerFromBoatChief"].Value = logbook.AnswerFromBoatChief;
                                command.Parameters["@filedById"].Value = logbook.FiledById;

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
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var personDal = new PersonMockDal();
                        var persons = personDal.GetAll();

                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("INSERT INTO {0} (" +
                                              "person_id, first_name, last_name, address, postcode, cityname, " +
                                              "date_of_birth, boat_driver, gender, phone_number, email" +
                                              ") VALUES (" +
                                              "@personId, @firstName, @lastName, @address, @postcode, @cityname, " +
                                              "@dateOfBirth, @boatDriver, @gender, @phoneNumber, @email" +
                                              ")",
                                              TablePersons);

                            command.Parameters.Add("@personId", DbType.Int64);
                            command.Parameters.Add("@firstName", DbType.String);
                            command.Parameters.Add("@lastName", DbType.String);
                            command.Parameters.Add("@address", DbType.String);
                            command.Parameters.Add("@postcode", DbType.String);
                            command.Parameters.Add("@cityname", DbType.String);
                            command.Parameters.Add("@dateOfBirth", DbType.String);
                            command.Parameters.Add("@boatDriver", DbType.Boolean);
                            command.Parameters.Add("@gender", DbType.Int32);
                            command.Parameters.Add("@phoneNumber", DbType.String);
                            command.Parameters.Add("@email", DbType.String);

                            foreach (var person in persons)
                            {
                                command.Parameters["@personId"].Value = person.PersonId;
                                command.Parameters["@firstName"].Value = person.FirstName;
                                command.Parameters["@lastName"].Value = person.LastName;
                                command.Parameters["@address"].Value = person.Address;
                                command.Parameters["@postcode"].Value = person.Postcode;
                                command.Parameters["@cityname"].Value = person.Cityname;
                                command.Parameters["@dateOfBirth"].Value = person.DateOfBirth;
                                command.Parameters["@boatDriver"].Value = person.BoatDriver;
                                command.Parameters["@gender"].Value = (int)person.Gender;
                                command.Parameters["@phoneNumber"].Value = person.PhoneNumber;
                                command.Parameters["@email"].Value = person.Email;

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

                #region Persist data: RegularTrip
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
                                              "regular_trip_id, departure_time, arrival_time, weather_conditions, " +
                                              "purpose_and_area, boat_id, captain_id, logbook_id, created_by_id" +
                                              ") VALUES (" +
                                              "@regularTripId, @departureTime, @arrivalTime, @weatherConditions, " +
                                              "@purposeAndArea, @boatId, @captainId, @logbookId, @createdById" +
                                              ")",
                                              TableRegularTrips);

                            command.Parameters.Add("@regularTripId", DbType.Int64);
                            command.Parameters.Add("@departureTime", DbType.DateTime);
                            command.Parameters.Add("@arrivalTime", DbType.DateTime);
                            command.Parameters.Add("@weatherConditions", DbType.String);
                            command.Parameters.Add("@purposeAndArea", DbType.String);
                            command.Parameters.Add("@boatId", DbType.Int64);
                            command.Parameters.Add("@captainId", DbType.Int64);
                            command.Parameters.Add("@logbookId", DbType.Int64);
                            command.Parameters.Add("@createdById", DbType.Int64);

                            foreach (var trip in trips)
                            {
                                command.Parameters["@regularTripId"].Value = trip.RegularTripId;
                                command.Parameters["@departureTime"].Value = trip.DepartureTime;
                                command.Parameters["@arrivalTime"].Value = trip.ArrivalTime;
                                command.Parameters["@weatherConditions"].Value = trip.WeatherConditions;
                                command.Parameters["@purposeAndArea"].Value = trip.PurposeAndArea;
                                command.Parameters["@boatId"].Value = trip.BoatId;
                                command.Parameters["@captainId"].Value = trip.CaptainId;
                                command.Parameters["@logbookId"].Value = trip.LogbookId;
                                command.Parameters["@createdById"].Value = trip.CreatedById;

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
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var memberDal = new SailClubMemberMockDal();
                        var members = memberDal.GetAll();

                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("INSERT INTO {0} (" +
                                              "sail_club_member_id, position, username, password_hash, person_id" +
                                              ") VALUES (" +
                                              "@sailClubMemberId, @position, @username, @passwordHash, @personId" +
                                              ")",
                                              TableSailClubMembers);

                            command.Parameters.Add("@sailClubMemberId", DbType.Int64);
                            command.Parameters.Add("@position", DbType.Int32);
                            command.Parameters.Add("@username", DbType.String);
                            command.Parameters.Add("@passwordHash", DbType.String);
                            command.Parameters.Add("@personId", DbType.String);

                            foreach (var member in members)
                            {
                                command.Parameters["@sailClubMemberId"].Value = member.SailClubMemberId;
                                command.Parameters["@position"].Value = member.Position;
                                command.Parameters["@username"].Value = member.Username;
                                command.Parameters["@passwordHash"].Value = member.PasswordHash;
                                command.Parameters["@personId"].Value = member.PersonId;

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

                #region Persist data: StudentMember
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var studentDal = new StudentMemberMockDal();
                        var students = studentDal.GetAll();

                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("INSERT INTO {0} (" +
                                              "student_member_id, sail_club_member_id, rope_works, navigation, " +
                                              "motor, drabant, gaffelrigger, night, team_id" +
                                              ") VALUES (" +
                                              "@studentMemberId, @sailClubMemberId, @ropeWorks, @navigation, " +
                                              "@motor, @drabant, @gaffelrigger, @night, @teamId" +
                                              ")",
                                              TableStudentMembers);

                            command.Parameters.Add("@studentMemberId", DbType.Int64);
                            command.Parameters.Add("@sailClubMemberId", DbType.Int64);
                            command.Parameters.Add("@ropeWorks", DbType.Boolean);
                            command.Parameters.Add("@navigation", DbType.Boolean);
                            command.Parameters.Add("@motor", DbType.Boolean);
                            command.Parameters.Add("@drabant", DbType.Boolean);
                            command.Parameters.Add("@gaffelrigger", DbType.Boolean);
                            command.Parameters.Add("@night", DbType.Boolean);
                            command.Parameters.Add("@teamId", DbType.Int64);

                            foreach (var student in students)
                            {
                                command.Parameters["@studentMemberId"].Value = student.StudentMemberId;
                                command.Parameters["@sailClubMemberId"].Value = student.SailClubMemberId;
                                command.Parameters["@ropeWorks"].Value = student.RopeWorks;
                                command.Parameters["@navigation"].Value = student.Navigation;
                                command.Parameters["@motor"].Value = student.Motor;
                                command.Parameters["@drabant"].Value = student.Drabant;
                                command.Parameters["@gaffelrigger"].Value = student.Gaffelrigger;
                                command.Parameters["@night"].Value = student.Night;
                                command.Parameters["@teamId"].Value = student.AssociatedTeamId;

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

                #region Persist data: Team
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var teamDal = new TeamMockDal();
                        var teams = teamDal.GetAll();

                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("INSERT INTO {0} (" +
                                              "team_id, name, level, teacher_id" +
                                              ") VALUES (" +
                                              "@teamId, @name, @level, @teacherId" +
                                              ")",
                                              TableTeams);

                            command.Parameters.Add("@teamId", DbType.Int64);
                            command.Parameters.Add("@name", DbType.String);
                            command.Parameters.Add("@level", DbType.Int32);
                            command.Parameters.Add("@teacherId", DbType.Int64);

                            foreach (var team in teams)
                            {
                                command.Parameters["@teamId"].Value = team.TeamId;
                                command.Parameters["@name"].Value = team.Name;
                                command.Parameters["@level"].Value = (int)team.Level;
                                command.Parameters["@teacherId"].Value = team.TeacherId;

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

                conn.Close();
            }

            return true;
        }

        public static bool RestoreMockData()
        {
            if (!File.Exists(DbFilePath)) { return false; }

            using (var conn = new SQLiteConnection(GetConnectionString()))
            {
                conn.Open();

                #region Load data: Boat
                try
                {
                    var boatDal = new BoatMockDal();

                    using (var command = conn.CreateCommand()) {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format("SELECT * FROM {0}",
                            TableBoats);

                        using (var reader = command.ExecuteReader()) {
                            while (reader.Read())
                            {
                                var boat = new Boat()
                                           {
                                               BoatId = reader.TryReadLong("boat_id"),
                                               NickName = reader.TryReadString("nickname"),
                                               ImagePath = reader.TryReadString("image_path"),
                                               Operational = reader.TryReadBoolean("operational"),
                                               Type = (BoatType) reader.TryReadInt("type")
                                           };

                                boatDal.CreateWithId(boat);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                #endregion

                #region Load data: Person
                try
                {
                    var personDal = new PersonMockDal();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format("SELECT * FROM {0}",
                            TablePersons);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var person = new Person()
                                {
                                    PersonId = reader.TryReadLong("person_id"),
                                    FirstName = reader.TryReadString("first_name"),
                                    LastName = reader.TryReadString("last_name"),
                                    Address = reader.TryReadString("address"),
                                    Postcode = reader.TryReadString("postcode"),
                                    Cityname = reader.TryReadString("cityname"),
                                    DateOfBirth = reader.TryReadString("date_of_birth"),
                                    BoatDriver = reader.TryReadBoolean("boat_driver"),
                                    Email = reader.TryReadString("email"),
                                    Gender = (Gender) reader.TryReadInt("gender"),
                                    PhoneNumber = reader.TryReadString("phone_number")
                                };

                                personDal.CreateWithId(person);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                #endregion

                #region Load data: SailClubMember
                try
                {
                    var memberDal = new SailClubMemberMockDal();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format("SELECT * FROM {0}",
                            TableSailClubMembers);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var sailClubMember = new SailClubMember()
                                {
                                    SailClubMemberId = reader.TryReadLong("sail_club_member_id"),
                                    PersonId = reader.TryReadLong("person_id"),
                                    Position = (SailClubMember.Positions) reader.TryReadInt("position"),
                                    Username = reader.TryReadString("username"),
                                    PasswordHash = reader.TryReadString("password_hash")
                                };

                                memberDal.CreateWithId(sailClubMember);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                #endregion

                #region Load data: Event
                try
                {
                    var eventDal = new EventMockDal();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format("SELECT * FROM {0}",
                            TableEvents);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var @event = new Event()
                                {
                                    EventId = reader.TryReadLong("event_id"),
                                    EventDate = reader.TryReadDateTime("event_date"),
                                    EventTitle = reader.TryReadString("event_title"),
                                    Created = reader.TryReadBoolean("created"),
                                    Description = reader.TryReadString("description"),
                                    SignUpMsg = reader.TryReadString("sign_up_msg"),
                                    SignUpReq = reader.TryReadBoolean("sign_up_req")
                                };

                                eventDal.CreateWithId(@event);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                #endregion

                #region Load data: Team
                try
                {
                    var teamDal = new TeamMockDal();
                    var memberDal = new SailClubMemberMockDal();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format("SELECT * FROM {0}",
                            TableTeams);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var team = new Team()
                                {
                                    TeamId = reader.TryReadLong("team_id"),
                                    Level = (Team.ClassLevel) reader.TryReadInt("level"),
                                    Name = reader.TryReadString("name"),
                                    TeacherId = reader.TryReadLong("teacher_id")
                                };

                                if (team.TeacherId > 0) { team.Teacher = memberDal.GetOne(team.TeacherId); }

                                teamDal.CreateWithId(team);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                #endregion

                #region Load data: StudentMember
                try
                {
                    var studentDal = new StudentMemberMockDal();
                    var teamDal = new TeamMockDal();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format("SELECT * FROM {0}",
                            TableStudentMembers);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var studentMember = new StudentMember()
                                {
                                    StudentMemberId = reader.TryReadLong("student_member_id"),
                                    SailClubMemberId = reader.TryReadLong("sail_club_member_id"),
                                    AssociatedTeamId = reader.TryReadLong("team_id"),
                                    Drabant = reader.TryReadBoolean("drabant"),
                                    Gaffelrigger = reader.TryReadBoolean("gaffelrigger"),
                                    RopeWorks = reader.TryReadBoolean("rope_works"),
                                    Motor = reader.TryReadBoolean("motor"),
                                    Night = reader.TryReadBoolean("night"),
                                    Navigation = reader.TryReadBoolean("navigation")
                                };

                                if (studentMember.AssociatedTeamId > 0) 
                                {
                                    studentMember.AssociatedTeam = teamDal.GetOne(studentMember.AssociatedTeamId);
                                }

                                studentDal.CreateWithId(studentMember);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                #endregion

                #region Load data: Lecture
                try
                {
                    var lectureDal = new LectureMockDal();
                    var teamDal = new TeamMockDal();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format("SELECT * FROM {0}",
                            TableLectures);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var lecture = new Lecture()
                                {
                                    LectureId = reader.TryReadLong("lecture_id"),
                                    DateOfLecture = reader.TryReadDateTime("date_of_lecture"),
                                    TeamId = reader.TryReadLong("team_id"),
                                    Drabant = reader.TryReadBoolean("drabant"),
                                    Gaffelrigger = reader.TryReadBoolean("gaffelrigger"),
                                    Motor = reader.TryReadBoolean("motor"),
                                    Navigation = reader.TryReadBoolean("navigation"),
                                    Night = reader.TryReadBoolean("night"),
                                    RopeWorksLecture = reader.TryReadBoolean("rope_works")
                                };

                                if (lecture.TeamId > 0) { lecture.Team = teamDal.GetOne(lecture.TeamId); }

                                lectureDal.CreateWithId(lecture);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                #endregion

                #region Load data: Logbook
                try
                {
                    var logbookDal = new LogbookMockDal();
                    var memberDal = new SailClubMemberMockDal();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format("SELECT * FROM {0}",
                            TableLogbooks);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var logbook = new Logbook()
                                {
                                    LogbookId = reader.TryReadLong("logbook_id"),
                                    FiledById = reader.TryReadLong("filed_by_id"),
                                    ActualArrivalTime = reader.TryReadDateTime("actual_arrival_time"),
                                    ActualDepartureTime = reader.TryReadDateTime("actual_departure_time"),
                                    AnswerFromBoatChief = reader.TryReadString("answer_from_boat_chief"),
                                    DamageDescription = reader.TryReadString("damage_description"),
                                    DamageInflicted = reader.TryReadBoolean("damage_inflicted")
                                };

                                if (logbook.FiledById > 0) { logbook.FiledBy = memberDal.GetOne(logbook.FiledById); }

                                logbookDal.CreateWithId(logbook);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                #endregion

                #region Load data: RegularTrip
                try
                {
                    var tripDal = new RegularTripMockDal();
                    var boatDal = new BoatMockDal();
                    var personDal = new PersonMockDal();
                    var logbookDal = new LogbookMockDal();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format("SELECT * FROM {0}",
                            TableRegularTrips);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var trip = new RegularTrip()
                                {
                                    RegularTripId = reader.TryReadLong("regular_trip_id"),
                                    BoatId = reader.TryReadLong("boat_id"),
                                    CaptainId = reader.TryReadLong("captain_id"),
                                    LogbookId = reader.TryReadLong("logbook_id"),
                                    CreatedById = reader.TryReadLong("created_by_id"),
                                    DepartureTime = reader.TryReadDateTime("departure_time"),
                                    ArrivalTime = reader.TryReadDateTime("arrival_time"),
                                    PurposeAndArea = reader.TryReadString("purpose_and_area"),
                                    WeatherConditions = reader.TryReadString("weather_conditions")
                                };

                                if (trip.BoatId > 0) { trip.Boat = boatDal.GetOne(trip.BoatId); }
                                if (trip.CaptainId > 0) { trip.Captain = personDal.GetOne(trip.CaptainId); }
                                if (trip.CreatedById > 0) { trip.CreatedBy = personDal.GetOne(trip.CreatedById); }
                                if (trip.LogbookId > 0) { trip.Logbook = logbookDal.GetOne(trip.LogbookId); }

                                tripDal.CreateWithId(trip);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                #endregion

                #region Load sub-data: Event->Participants
                try
                {
                    var eventDal = new EventMockDal();
                    var personDal = new PersonMockDal();
                    var events = eventDal.GetAll();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format("SELECT * FROM {0} " +
                                          "WHERE event_id = @eventId",
                            TableEventParticipantsBinder);
                        command.Parameters.Add("@eventId", DbType.Int64);

                        foreach (var @event in events)
                        {
                            @event.Participants = new List<Person>();
                            command.Parameters["@eventId"].Value = @event.EventId;

                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var personId = reader.TryReadLong("person_id");

                                    if (personId > 0) { @event.Participants.Add(personDal.GetOne(personId)); }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                #endregion

                #region Load sub-data: Lecture->PresentMembers
                try
                {
                    var lectureDal = new LectureMockDal();
                    var studentDal = new StudentMemberMockDal();
                    var lectures = lectureDal.GetAll();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format("SELECT * FROM {0} " +
                                          "WHERE lecture_id = @lectureId",
                            TableLecturePresentMembersBinder);
                        command.Parameters.Add("@lectureId", DbType.Int64);

                        foreach (var lecture in lectures)
                        {
                            lecture.PresentMembers = new List<StudentMember>();
                            command.Parameters["@lectureId"].Value = lecture.LectureId;

                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var studentId = reader.TryReadLong("student_member_id");

                                    if (studentId > 0) { lecture.PresentMembers.Add(studentDal.GetOne(studentId)); }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                #endregion

                #region Load sub-data: Logbook->ActualCrew
                try
                {
                    var logbookDal = new LogbookMockDal();
                    var personDal = new PersonMockDal();
                    var logbooks = logbookDal.GetAll();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format("SELECT * FROM {0} " +
                                          "WHERE logbook_id = @logbookId",
                            TableLogbookActualCrewBinder);
                        command.Parameters.Add("@logbookId", DbType.Int64);

                        foreach (var logbook in logbooks)
                        {
                            logbook.ActualCrew = new List<Person>();
                            command.Parameters["@logbookId"].Value = logbook.LogbookId;

                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var personId = reader.TryReadLong("person_id");

                                    if (personId > 0) { logbook.ActualCrew.Add(personDal.GetOne(personId)); }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                #endregion

                #region Load sub-data: RegularTrip->Crew
                try
                {
                    var tripDal = new RegularTripMockDal();
                    var personDal = new PersonMockDal();
                    var trips = tripDal.GetAll();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format("SELECT * FROM {0} " +
                                          "WHERE regular_trip_id = @regularTripId",
                            TableRegularTripCrewBinder);
                        command.Parameters.Add("@regularTripId", DbType.Int64);

                        foreach (var trip in trips)
                        {
                            trip.Crew = new List<Person>();
                            command.Parameters["@regularTripId"].Value = trip.RegularTripId;

                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var personId = reader.TryReadLong("person_id");

                                    if (personId > 0) { trip.Crew.Add(personDal.GetOne(personId)); }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                #endregion

                conn.Close();
            }

            return true;
        }

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

        #region SQLiteDataReader Extension Methods
        public static bool TryReadBoolean(this SQLiteDataReader reader, string column)
        {
            bool result = false;
            int columnIndex = reader.GetOrdinal(column);

            if (!reader.IsDBNull(columnIndex)) { result = reader.GetBoolean(columnIndex); }

            return result;
        }

        public static DateTime TryReadDateTime(this SQLiteDataReader reader, string column)
        {
            DateTime result = default(DateTime);
            int columnIndex = reader.GetOrdinal(column);

            if (!reader.IsDBNull(columnIndex)) { result = reader.GetDateTime(columnIndex); }

            return result;
        }

        public static int TryReadInt(this SQLiteDataReader reader, string column)
        {
            int result = 0;
            int columnIndex = reader.GetOrdinal(column);

            if (!reader.IsDBNull(columnIndex)) { result = reader.GetInt32(columnIndex); }

            return result;
        }

        public static long TryReadLong(this SQLiteDataReader reader, string column)
        {
            long result = 0;
            int columnIndex = reader.GetOrdinal(column);

            if (!reader.IsDBNull(columnIndex)) { result = reader.GetInt64(columnIndex); }

            return result;
        }

        public static string TryReadString(this SQLiteDataReader reader, string column)
        {
            string result = "";
            int columnIndex = reader.GetOrdinal(column);

            if (!reader.IsDBNull(columnIndex)) { result = reader.GetString(columnIndex); }

            return result;
        }

        public static T TryReadData<T>(this SQLiteDataReader reader, string column)
        {
            T result = default(T);
            int columnIndex = reader.GetOrdinal(column);

            if (!reader.IsDBNull(columnIndex)) { result = reader.GetFieldValue<T>(columnIndex); }

            return result;
        }
        #endregion
    }
}
