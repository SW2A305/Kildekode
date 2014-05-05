using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace McSntt.Helpers
{
    public static class SqliteManager
    {
        #region Table Names
        public const string TableBoats = "boats";
        public const string TableDbSettings = "db_settings";
        public const string TableEvents = "events";
        public const string TableLectures = "lectures";
        public const string TableLogbooks = "logbooks";
        public const string TablePersons = "persons";
        public const string TableRegularTrips = "regular_trips";
        public const string TableSailClubMembers = "sail_club_members";
        public const string TableStudentMembers = "student_members";
        public const string TableTeams = "teams";
        #endregion

        #region DB Settings - Keys
        public const string DbSettingDbVersion = "DbVersion";
        #endregion

        #region Constants, fields and properties, oh my!
        private const int DbVersion = 1;
        private const string AppName = "McSnttt";
        private const string DbFileName = "McSnttt.db";

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

                    if (!Directory.Exists(roamingAppDataFolder))
                    {
                        Directory.CreateDirectory(roamingAppDataFolder);
                    }

                    _dbFilePath = Path.Combine(roamingAppDataFolder, DbFileName);
                }

                return _dbFilePath;
            }
        }

        private static string ConnectionString
        {
            get
            {
                if (String.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = String.Format("Data Source={0};Version=3", DbFilePath);
                }

                return _connectionString;
            }
        }

        public static SQLiteConnection DbConnection
        {
            get
            {
                return new SQLiteConnection(ConnectionString);
            }
        }
        #endregion


        public static void InitializeDatabase()
        {
            // Make sure database even exists
            if (!File.Exists(DbFilePath))
            {
                CreateDatabase();
            }

            using (var db = DbConnection)
            {
                var dbVersion = 0;

                db.Open();

                using (var command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = String.Format("SELECT value FROM {0} WHERE name = @name", TableDbSettings);
                    command.Parameters.Add(new SQLiteParameter("@name", DbSettingDbVersion));

                    var reader = command.ExecuteReader();

                    if (reader.NextResult())
                    {
                        dbVersion = reader.GetInt32(reader.GetOrdinal("value"));
                    }
                }

                if (dbVersion < DbVersion)
                {
                    for (int i = dbVersion; i < DbVersion; i++)
                    {
                        UpdateDatabase(db, i, i + 1);
                    }
                }

                db.Close();
            }
        }

        private static void UpdateDatabase(SQLiteConnection db, int fromVersion, int toVersion)
        {
            if (fromVersion == 0 && toVersion == 1)
            {
                UpdateDatabaseFromVersion0ToVersion1(db);
            }
        }

        private static void UpdateDatabaseFromVersion0ToVersion1(SQLiteConnection db)
        {
            using (var transaction = db.BeginTransaction())
            {
                try
                {
                    #region Create Table: Boats
                    using (var command = db.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format(
                                          "CREATE TABLE {0} (boat_id INTEGER PRIMARY KEY, " +
                                          "type INTEGER, nickname TEXT, image_path TEXT, operational INTEGER" +
                                          ")",
                                          TableBoats);
                        command.ExecuteNonQuery();
                    }
                    #endregion

                    #region Create table: Events
                    using (var command = db.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format(
                                          "CREATE TABLE {0} (event_id INTEGER PRIMARY KEY, " +
                                          "event_date TEXT, event_title TEXT, sign_up_req INTEGER, description TEXT, " +
                                          "sign_up_msg TEXT, created INTEGER" +
                                          ")",
                                          TableEvents);
                        command.ExecuteNonQuery();
                    }
                    #endregion

                    #region Create table: Lectures
                    using (var command = db.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format(
                                          "CREATE TABLE {0} (lecture_id INTEGER PRIMARY KEY, " +
                                          "team_id INTEGER, date_of_lecture TEXT, rope_works INTEGER, navigation INTEGER, "
                                          +
                                          "motor INTEGER, drabant INTEGER, gaffelrigger INTEGER, night INTEGER" +
                                          ")",
                                          TableLectures);
                        command.ExecuteNonQuery();
                    }
                    #endregion

                    #region Create table: Logbooks
                    using (var command = db.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format(
                                          "CREATE TABLE {0} (logbook_id INTEGER PRIMARY KEY, " +
                                          "actual_departure_time TEXT, actual_arrival_time TEXT, damage_inflicted INTEGER, "
                                          +
                                          "damage_description TEXT, answer_from_boat_chief TEXT, filed_by_id INTEGER" +
                                          ")",
                                          TableLogbooks);
                        command.ExecuteNonQuery();
                    }
                    #endregion

                    #region Create table: Persons
                    using (var command = db.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format(
                                          "CREATE TABLE {0} (person_id INTEGER PRIMARY KEY, " +
                                          "first_name TEXT, last_name TEXT, address TEXT, postcode TEXT, cityname TEXT, "
                                          +
                                          "date_of_birth TEXT, boat_driver INTEGER, gender INTEGER, phone_number TEXT, "
                                          +
                                          "email TEXT" +
                                          ")",
                                          TablePersons);
                        command.ExecuteNonQuery();
                    }
                    #endregion

                    #region Create table: RegularTrips
                    using (var command = db.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format(
                                          "CREATE TABLE {0} (regular_trip_id INTEGER PRIMARY KEY, " +
                                          "departure_time TEXT, arrival_time TEXT, boat_id INTEGER, captain_id INTEGER, "
                                          +
                                          "expected_arrival_time TEXT, purpose_and_area TEXT, weather_conditions TEXT, "
                                          +
                                          "logbook_id INTEGER" +
                                          ")",
                                          TableRegularTrips);
                        command.ExecuteNonQuery();
                    }
                    #endregion

                    #region Create table: SailClubMembers
                    using (var command = db.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format(
                                          "CREATE TABLE {0} (sail_club_member_id INTEGER PRIMARY KEY, " +
                                          "person_id INTEGER, position INTEGER, username TEXT, password_hash TEXT" +
                                          ")",
                                          TableSailClubMembers);
                        command.ExecuteNonQuery();
                    }
                    #endregion

                    #region Create table: StudentMembers
                    using (var command = db.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format(
                                          "CREATE TABLE {0} (student_member_id INTEGER PRIMARY KEY, " +
                                          "sail_club_member_id INTEGER, team_id INTEGER, " +
                                          "rope_works INTEGER, navigation INTEGER, motor INTEGER, drabant INTEGER, " +
                                          "gaffelrigger INTEGER, night INTEGER" +
                                          ")",
                                          TableStudentMembers);
                        command.ExecuteNonQuery();
                    }
                    #endregion

                    #region Create table: Teams
                    using (var command = db.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format(
                                          "CREATE TABLE {0} (team_id INTEGER PRIMARY KEY, " +
                                          "name TEXT, level INTEGER" +
                                          ")",
                                          TableTeams);
                        command.ExecuteNonQuery();
                    }
                    #endregion

                    #region Update DB version
                    using (var command = db.CreateCommand())
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
                }
            }
        }

        private static void CreateDatabase()
        {
            SQLiteConnection.CreateFile(DbFilePath);

            using (var db = DbConnection)
            {
                db.Open();

                // Create table that tracks database version
                using (var command = db.CreateCommand()) {
                    command.CommandType = CommandType.Text;
                    command.CommandText = String.Format("CREATE TABLE {0} (name TEXT, value TEXT)", TableDbSettings);
                    command.ExecuteNonQuery();
                }

                // Insert version tracker setting
                using (var command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = String.Format("INSERT INTO {0} (name, value) VALUES (@name, @version)", TableDbSettings);
                    command.Parameters.Add(new SQLiteParameter("@name", DbSettingDbVersion));
                    command.Parameters.Add(new SQLiteParameter("@version", 0));
                    command.ExecuteNonQuery();
                }

                db.Close();
            }
        }
    }
}
