﻿using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace McSntt.Helpers
{
    public static class DatabaseManager
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

        #region DB Settings - Keys
        public const string DbSettingDbVersion = "DbVersion";
        #endregion

        #region Constants, fields and properties, oh my!
        private const int DbVersion = 2;
        private const string AppName = "McSnttt";
        private const string DbFileName = "McSnttt.db";
        private const string TestDbFileName = "McSnttt.Test.db";

        private static bool _useTestDb;
        private static string _dbFilePath;
        private static string _connectionString;

        private static string DbFilePath
        {
            get
            {
                if (String.IsNullOrEmpty(_dbFilePath))
                {
                    string roamingAppDataFolder =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);

                    if (!Directory.Exists(roamingAppDataFolder)) { Directory.CreateDirectory(roamingAppDataFolder); }

                    _dbFilePath = Path.Combine(roamingAppDataFolder, (_useTestDb ? TestDbFileName : DbFileName));
                }

                return _dbFilePath;
            }
        }

        private static string ConnectionString
        {
            get
            {
                if (String.IsNullOrEmpty(_connectionString)) {
                    _connectionString = String.Format("Data Source={0};Version=3", DbFilePath);
                }

                return _connectionString;
            }
        }

        public static SQLiteConnection DbConnection
        {
            get { return new SQLiteConnection(ConnectionString); }
        }
        #endregion

        public static void InitializeDatabase(bool useTestDb = false)
        {
            long dbVersion = 0;

            _useTestDb = useTestDb;

            // If we're using a test DB, we should remove it each time, in order to reset it.
            if (useTestDb) { File.Delete(DbFilePath); }

            // Make sure database even exists
            if (!File.Exists(DbFilePath)) { CreateDatabase(); }

            using (SQLiteConnection db = DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = String.Format("SELECT value FROM {0} WHERE name = @name", TableDbSettings);
                    command.Parameters.Add(new SQLiteParameter("@name", DbSettingDbVersion));

                    using (SQLiteDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) { dbVersion = ReadData<long>(reader, reader.GetOrdinal("value")); }
                    }
                }

                db.Close();
            }

            if (dbVersion < DbVersion) { for (long i = dbVersion; i < DbVersion; i++) { UpdateDatabase(i, i + 1); } }
        }

        private static void UpdateDatabase(long fromVersion, long toVersion)
        {
            if (fromVersion == 0 && toVersion == 1) { UpdateDatabaseFromVersion0ToVersion1(); }
            if (fromVersion == 1 && toVersion == 2) { UpdateDatabaseFromVersion1ToVersion2(); }
        }

        private static void UpdateDatabaseFromVersion0ToVersion1()
        {
            using (SQLiteConnection db = DbConnection)
            {
                db.Open();

                using (SQLiteTransaction transaction = db.BeginTransaction())
                {
                    try
                    {
                        #region Create Table: Boats
                        using (SQLiteCommand command = db.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (boat_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                              "type INTEGER, nickname TEXT, image_path TEXT, operational INTEGER" +
                                              ")",
                                              TableBoats);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create table: Events
                        using (SQLiteCommand command = db.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (event_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                              "event_date TEXT, event_title TEXT, sign_up_req INTEGER, description TEXT, "
                                              +
                                              "sign_up_msg TEXT, created INTEGER" +
                                              ")",
                                              TableEvents);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create table: Lectures
                        using (SQLiteCommand command = db.CreateCommand())
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
                        using (SQLiteCommand command = db.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (logbook_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                              "actual_departure_time TEXT, actual_arrival_time TEXT, damage_inflicted INTEGER, "
                                              +
                                              "damage_description TEXT, answer_from_boat_chief TEXT, filed_by_id INTEGER"
                                              +
                                              ")",
                                              TableLogbooks);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create table: Persons
                        using (SQLiteCommand command = db.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (person_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                              "first_name TEXT, last_name TEXT, address TEXT, postcode TEXT, " +
                                              "cityname TEXT, date_of_birth TEXT, boat_driver INTEGER, gender INTEGER, "
                                              +
                                              "phone_number TEXT, email TEXT" +
                                              ")",
                                              TablePersons);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create table: RegularTrips
                        using (SQLiteCommand command = db.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (regular_trip_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                              "departure_time TEXT, arrival_time TEXT, boat_id INTEGER, " +
                                              "captain_id INTEGER, expected_arrival_time TEXT, purpose_and_area TEXT, "
                                              +
                                              "weather_conditions TEXT, logbook_id INTEGER" +
                                              ")",
                                              TableRegularTrips);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create table: SailClubMembers
                        using (SQLiteCommand command = db.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (sail_club_member_id INTEGER PRIMARY KEY AUTOINCREMENT, "
                                              +
                                              "person_id INTEGER, position INTEGER, username TEXT, password_hash TEXT" +
                                              ")",
                                              TableSailClubMembers);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create table: StudentMembers
                        using (SQLiteCommand command = db.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format(
                                              "CREATE TABLE {0} (student_member_id INTEGER PRIMARY KEY AUTOINCREMENT, "
                                              +
                                              "sail_club_member_id INTEGER, team_id INTEGER, " +
                                              "rope_works INTEGER, navigation INTEGER, motor INTEGER, drabant INTEGER, "
                                              +
                                              "gaffelrigger INTEGER, night INTEGER" +
                                              ")",
                                              TableStudentMembers);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create table: Teams
                        using (SQLiteCommand command = db.CreateCommand())
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
                        using (SQLiteCommand command = db.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("CREATE TABLE {0} (" +
                                              "event_id TEXT, person_id INTEGER" +
                                              ")",
                                              TableEventParticipantsBinder);
                            command.ExecuteNonQuery();
                        }

                        using (SQLiteCommand command = db.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("CREATE UNIQUE INDEX IF NOT EXISTS event_participants " +
                                              "ON {0} (event_id, person_id)",
                                              TableEventParticipantsBinder);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create binding-table: LogbookActualCrewBinder
                        using (SQLiteCommand command = db.CreateCommand())
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

                        using (SQLiteCommand command = db.CreateCommand())
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
                        using (SQLiteCommand command = db.CreateCommand())
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

                        using (SQLiteCommand command = db.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("CREATE UNIQUE INDEX IF NOT EXISTS regular_trip_crew " +
                                              "ON {0} (regular_trip_id, person_id)",
                                              TableRegularTripCrewBinder);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Create binding-table: LecturePresentMembersBinder
                        using (SQLiteCommand command = db.CreateCommand())
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

                        using (SQLiteCommand command = db.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("CREATE UNIQUE INDEX IF NOT EXISTS lecture_present_members " +
                                              "ON {0} (lecture_id, student_member_id)",
                                              TableLecturePresentMembersBinder);
                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Update DB version
                        using (SQLiteCommand command = db.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = String.Format("UPDATE {0} SET value = @value WHERE name = @name",
                                                                TableDbSettings);
                            command.Parameters.Add(new SQLiteParameter("@name", DbSettingDbVersion));
                            command.Parameters.Add(new SQLiteParameter("@value", 1));
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

                db.Close();
            }
        }

        private static void UpdateDatabaseFromVersion1ToVersion2()
        {
            using (SQLiteConnection db = DbConnection)
            {
                db.Open();

                using (SQLiteTransaction transaction = db.BeginTransaction())
                {
                    try
                    {
                        #region Alter Table: RegularTrips
                        using (SQLiteCommand command = db.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText =
                                String.Format("ALTER TABLE {0} " +
                                              "ADD COLUMN created_by_id INTEGER",
                                              TableRegularTrips);

                            command.ExecuteNonQuery();
                        }
                        #endregion

                        #region Update DB version
                        using (SQLiteCommand command = db.CreateCommand())
                        {
                            command.CommandType = CommandType.Text;
                            command.CommandText = String.Format("UPDATE {0} SET value = @value WHERE name = @name",
                                                                TableDbSettings);
                            command.Parameters.Add(new SQLiteParameter("@name", DbSettingDbVersion));
                            command.Parameters.Add(new SQLiteParameter("@value", 2));
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

                db.Close();
            }

            // TODO Remove this eventually...
            if (!_useTestDb) { DbSeedData.CreateSeedData(); }
        }

        private static void CreateDatabase()
        {
            SQLiteConnection.CreateFile(DbFilePath);

            using (SQLiteConnection db = DbConnection)
            {
                db.Open();

                // Create table that tracks database version
                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = String.Format(
                                                        "CREATE TABLE {0} (name TEXT NOT NULL, value INTEGER)",
                                                        TableDbSettings);
                    command.ExecuteNonQuery();
                }

                // Insert version tracker setting
                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = String.Format("INSERT INTO {0} (name, value) VALUES (@name, @version)",
                                                        TableDbSettings);
                    command.Parameters.Add(new SQLiteParameter("@name", DbSettingDbVersion));
                    command.Parameters.Add(new SQLiteParameter("@version", 0));
                    command.ExecuteNonQuery();
                }

                db.Close();
            }
        }

        #region SQLiteDataReader Helper Methods
        public static bool ReadBoolean(SQLiteDataReader reader, int columnIndex)
        {
            bool result = false;

            if (!reader.IsDBNull(columnIndex)) { result = reader.GetBoolean(columnIndex); }

            return result;
        }

        public static int ReadInt(SQLiteDataReader reader, int columnIndex)
        {
            int result = 0;

            if (!reader.IsDBNull(columnIndex)) { result = reader.GetInt32(columnIndex); }

            return result;
        }

        public static string ReadString(SQLiteDataReader reader, int columnIndex)
        {
            string result = "";

            if (!reader.IsDBNull(columnIndex)) { result = reader.GetString(columnIndex); }

            return result;
        }

        public static T ReadData<T>(SQLiteDataReader reader, int columnIndex)
        {
            T result = default(T);

            if (!reader.IsDBNull(columnIndex)) { result = reader.GetFieldValue<T>(columnIndex); }

            return result;
        }
        #endregion
    }
}
