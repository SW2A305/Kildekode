using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Sqlite
{
    public class RegularTripSqliteDal : IRegularTripDal
    {
        public bool Create(params RegularTrip[] items)
        {
            int insertedRows = 0;
            int tripRowsInserted = 0;
            int crewRowsInserted = 0;
            int crewRowsExpected = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand tripCommand = db.CreateCommand())
                {
                    using (var personCommand = db.CreateCommand())
                    {
                        tripCommand.CommandType = CommandType.Text;
                        tripCommand.CommandText =
                            String.Format("INSERT INTO {0} (departure_time, arrival_time, boat_id, captain_id, " +
                                          "purpose_and_area, weather_conditions, logbook_id, created_by_id) " +
                                          "VALUES (@departureTime, @arrivalTime, @boatId, @captainId, " +
                                          "@purposeAndArea, @weatherConditions, @logbookId, @createdById)",
                                          DatabaseManager.TableRegularTrips);

                        personCommand.CommandType = CommandType.Text;
                        personCommand.CommandText =
                            String.Format("INSERT OR IGNORE INTO {0} (regular_trip_id, person_id) " +
                                          "VALUES (@regularTripId, @personId)",
                                          DatabaseManager.TableRegularTripCrewBinder);

                        foreach (RegularTrip regularTrip in items)
                        {
                            using (var transaction = db.BeginTransaction())
                            {
                                tripCommand.Parameters.Clear();
                                tripCommand.Parameters.Add(new SQLiteParameter("@boatId", regularTrip.BoatId));
                                tripCommand.Parameters.Add(new SQLiteParameter("@captainId", regularTrip.CaptainId));
                                tripCommand.Parameters.Add(new SQLiteParameter("@logbookId", regularTrip.LogbookId));
                                tripCommand.Parameters.Add(new SQLiteParameter("@createdById", regularTrip.CreatedById));
                                tripCommand.Parameters.Add(new SQLiteParameter("@departureTime", regularTrip.DepartureTime));
                                tripCommand.Parameters.Add(new SQLiteParameter("@arrivalTime", regularTrip.ArrivalTime));
                                tripCommand.Parameters.Add(new SQLiteParameter("@purposeAndArea", regularTrip.PurposeAndArea));
                                tripCommand.Parameters.Add(new SQLiteParameter("@weatherConditions",
                                                                           regularTrip.WeatherConditions));
                                tripRowsInserted = tripCommand.ExecuteNonQuery();

                                regularTrip.RegularTripId = db.LastInsertRowId;

                                // Link to crew
                                crewRowsExpected = 0;

                                if (regularTrip.Crew != null)
                                {
                                    crewRowsExpected = regularTrip.Crew.Count;
                                    crewRowsInserted = 0;

                                    foreach (Person person in regularTrip.Crew)
                                    {
                                        personCommand.Parameters.Clear();
                                        personCommand.Parameters.Add(new SQLiteParameter("@regularTripId",
                                                                                          regularTrip.RegularTripId));
                                        personCommand.Parameters.Add(new SQLiteParameter("@personId",
                                                                                          person.PersonId));
                                        crewRowsInserted += personCommand.ExecuteNonQuery();
                                    }
                                }

                                // Verify that everything was inserted correctly
                                if (crewRowsInserted == crewRowsExpected)
                                {
                                    transaction.Commit();
                                    insertedRows += tripRowsInserted;
                                }
                                else
                                {
                                    transaction.Rollback();
                                }
                            }
                        }
                    }
                }

                db.Close();
            }

            return insertedRows == items.Length;
        }

        public bool Update(params RegularTrip[] items)
        {
            int updatedRows = 0;
            int tripRowsUpdated = 0;
            int crewRowsInserted = 0;
            int crewRowsExpected = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand tripCommand = db.CreateCommand())
                {
                    using (var personCommand = db.CreateCommand())
                    {
                        tripCommand.CommandType = CommandType.Text;
                        tripCommand.CommandText =
                            String.Format("UPDATE {0} " +
                                          "SET departure_time = @departureTime, arrival_time = @arrivalTime, " +
                                          "boat_id = @boatId, captain_id = @captainId, logbook_id = @logbookId, " +
                                          "purpose_and_area = @purposeAndArea, created_by_id = @createdById, " +
                                          "weather_conditions = @weatherConditions " +
                                          "WHERE regular_trip_id = @regularTripId ",
                                          DatabaseManager.TableRegularTrips);

                        personCommand.CommandType = CommandType.Text;
                        personCommand.CommandText =
                            String.Format("INSERT OR IGNORE INTO {0} (regular_trip_id, person_id) " +
                                          "VALUES (@regularTripId, @personId)",
                                          DatabaseManager.TableRegularTripCrewBinder);

                        foreach (RegularTrip regularTrip in items)
                        {
                            // Remove existing crew entries
                            using (SQLiteCommand deleteCommand = db.CreateCommand())
                            {
                                deleteCommand.CommandType = CommandType.Text;
                                deleteCommand.CommandText =
                                    String.Format("DELETE FROM {0} " +
                                                  "WHERE regular_trip_id = @regularTripId",
                                                  DatabaseManager.TableRegularTripCrewBinder);
                                deleteCommand.Parameters.Add(new SQLiteParameter("@regularTripId", regularTrip.RegularTripId));
                                deleteCommand.ExecuteNonQuery();
                            }

                            using (var transaction = db.BeginTransaction())
                            {
                                tripCommand.Parameters.Clear();
                                tripCommand.Parameters.Add(new SQLiteParameter("@regularTripId",
                                                                               regularTrip.RegularTripId));
                                tripCommand.Parameters.Add(new SQLiteParameter("@boatId", regularTrip.BoatId));
                                tripCommand.Parameters.Add(new SQLiteParameter("@captainId", regularTrip.CaptainId));
                                tripCommand.Parameters.Add(new SQLiteParameter("@logbookId", regularTrip.LogbookId));
                                tripCommand.Parameters.Add(new SQLiteParameter("@createdById", regularTrip.CreatedById));
                                tripCommand.Parameters.Add(new SQLiteParameter("@departureTime",
                                                                               regularTrip.DepartureTime));
                                tripCommand.Parameters.Add(new SQLiteParameter("@arrivalTime", regularTrip.ArrivalTime));
                                tripCommand.Parameters.Add(new SQLiteParameter("@purposeAndArea",
                                                                               regularTrip.PurposeAndArea));
                                tripCommand.Parameters.Add(new SQLiteParameter("@weatherConditions",
                                                                               regularTrip.WeatherConditions));
                                tripRowsUpdated = tripCommand.ExecuteNonQuery();

                                // Link to crew
                                crewRowsExpected = 0;

                                if (regularTrip.Crew != null)
                                {
                                    crewRowsExpected = regularTrip.Crew.Count;
                                    crewRowsInserted = 0;

                                    foreach (Person person in regularTrip.Crew)
                                    {
                                        personCommand.Parameters.Clear();
                                        personCommand.Parameters.Add(new SQLiteParameter("@regularTripId",
                                                                                          regularTrip.RegularTripId));
                                        personCommand.Parameters.Add(new SQLiteParameter("@personId",
                                                                                          person.PersonId));
                                        crewRowsInserted += personCommand.ExecuteNonQuery();
                                    }
                                }

                                // Verify that everything was inserted correctly
                                if (crewRowsInserted == crewRowsExpected)
                                {
                                    transaction.Commit();
                                    updatedRows += tripRowsUpdated;
                                }
                                else
                                {
                                    transaction.Rollback();
                                }
                            }
                        }
                    }
                }

                db.Close();
            }

            return updatedRows == items.Length;
        }

        public bool Delete(params RegularTrip[] items)
        {
            int deletedRows = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("DELETE FROM {0} " +
                                      "WHERE regular_trip_id = @regularTripId ",
                                      DatabaseManager.TableRegularTrips);

                    foreach (RegularTrip regularTrip in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@regularTripId", regularTrip.RegularTripId));
                        deletedRows += command.ExecuteNonQuery();

                        using (SQLiteCommand deleteCommand = db.CreateCommand())
                        {
                            deleteCommand.CommandType = CommandType.Text;
                            deleteCommand.CommandText =
                                String.Format("DELETE FROM {0} " +
                                              "WHERE regular_trip_id = @regularTripId",
                                              DatabaseManager.TableRegularTripCrewBinder);
                            deleteCommand.Parameters.Add(new SQLiteParameter("@regularTripId", regularTrip.RegularTripId));
                            deleteCommand.ExecuteNonQuery();
                        }
                    }
                }

                db.Close();
            }

            return deletedRows == items.Length;
        }

        public IEnumerable<RegularTrip> GetAll()
        {
            var regularTrips = new List<RegularTrip>();

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0}", DatabaseManager.TableRegularTrips);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            regularTrips.Add(
                                             new RegularTrip
                                             {
                                                 RegularTripId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("regular_trip_id")),
                                                 ArrivalTime = reader.GetDateTime(reader.GetOrdinal("arrival_time")),
                                                 DepartureTime = reader.GetDateTime(reader.GetOrdinal("departure_time")),
                                                 PurposeAndArea =
                                                     DatabaseManager.ReadString(reader, reader.GetOrdinal("purpose_and_area")),
                                                 WeatherConditions =
                                                     DatabaseManager.ReadString(reader, reader.GetOrdinal("weather_conditions")),
                                                 BoatId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("boat_id")),
                                                 CaptainId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("captain_id")),
                                                 LogbookId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("logbook_id")),
                                                 CreatedById = DatabaseManager.ReadInt(reader, reader.GetOrdinal("created_by_id"))
                                             });
                        }
                    }
                }

                db.Close();
            }

            return regularTrips;
        }

        public RegularTrip GetOne(long itemId)
        {
            RegularTrip regularTrip = null;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0} " +
                                      "WHERE regular_trip_id = @regularTripId " +
                                      "LIMIT 1",
                                      DatabaseManager.TableRegularTrips); // TODO Fill this out
                    command.Parameters.Add(new SQLiteParameter("@regularTripId", itemId));

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            regularTrip =
                                new RegularTrip
                                {
                                    RegularTripId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("regular_trip_id")),
                                    ArrivalTime = reader.GetDateTime(reader.GetOrdinal("arrival_time")),
                                    DepartureTime = reader.GetDateTime(reader.GetOrdinal("departure_time")),
                                    PurposeAndArea = DatabaseManager.ReadString(reader, reader.GetOrdinal("purpose_and_area")),
                                    WeatherConditions = DatabaseManager.ReadString(reader, reader.GetOrdinal("weather_conditions")),
                                    BoatId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("boat_id")),
                                    CaptainId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("captain_id")),
                                    LogbookId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("logbook_id")),
                                    CreatedById = DatabaseManager.ReadInt(reader, reader.GetOrdinal("created_by_id"))
                                };
                        }
                    }
                }

                db.Close();
            }

            return regularTrip;
        }

        public void LoadData(RegularTrip item)
        {
            // Load Captain
            var personDal = DalLocator.PersonDal;

            if (item.CaptainId > 0) { item.Captain = personDal.GetOne(item.CaptainId); }

            // Load CreatedBy
            if (item.CreatedById > 0) { item.CreatedBy = personDal.GetOne(item.CreatedById); }

            // Load Boat
            var boatDal = DalLocator.BoatDal;

            if (item.BoatId > 0) { item.Boat = boatDal.GetOne(item.BoatId); }

            // Load Logbook
            var logbookDal = DalLocator.LogbookDal;

            if (item.LogbookId > 0) { item.Logbook = logbookDal.GetOne(item.LogbookId); }

            // Load Crew
            using (var db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (var command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT person_id FROM {0} WHERE regular_trip_id = @regularTripId",
                                      DatabaseManager.TableRegularTripCrewBinder);
                    command.Parameters.Add(new SQLiteParameter("@regularTripId", item.RegularTripId));

                    item.Crew = new List<Person>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long personId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("person_id"));

                            if (personId > 0)
                            {
                                item.Crew.Add(personDal.GetOne(personId));
                            }
                        }
                    }
                }

                db.Close();
            }
        }

        public void LoadData(IEnumerable<RegularTrip> items)
        {
            foreach (var item in items)
            {
                LoadData(item);
            }
        }

        public bool CanMakeReservation(Boat boat, DateTime departureTime, DateTime arrivalTime)
        {
            var reservations =
                this.GetAll(
                            trip =>
                            trip.BoatId == boat.BoatId && trip.DepartureTime <= arrivalTime
                            && trip.ArrivalTime >= departureTime);

            return !reservations.Any();
        }

        public bool CanMakeReservation(RegularTrip trip)
        {
            if (trip.Boat == null) { return false; }

            return CanMakeReservation(trip.Boat, trip.DepartureTime, trip.ArrivalTime);
        }

        public IEnumerable<RegularTrip> GetAll(Func<RegularTrip, bool> predicate)
        {
            var regularTrips = this.GetAll().ToArray();

            LoadData(regularTrips);

            return regularTrips.Where(predicate);
        }
    }
}
