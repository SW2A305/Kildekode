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

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("INSERT INTO {0} (departure_time, arrival_time, boat_id, captain_id, " +
                                      "expected_arrival_time, purpose_and_area, weather_conditions, logbook_id) " +
                                      "VALUES (@departureTime, @arrivalTime, @boatId, @captainId, " +
                                      "@expectedArrivalTime, @purposeAndArea, @weatherConditions, @logbookId)",
                                      DatabaseManager.TableRegularTrips);

                    foreach (RegularTrip regularTrip in items)
                    {
                        command.Parameters.Clear();
                        // TODO Make sure "Boat" is set, and extract ID.
                        // TODO Make sure "Captain" is set, and extract ID.
                        // TODO Make sure "Logbook" is set, and extract ID.
                        command.Parameters.Add(new SQLiteParameter("@departureTime", regularTrip.DepartureTime));
                        command.Parameters.Add(new SQLiteParameter("@arrivalTime", regularTrip.ArrivalTime));
                        command.Parameters.Add(new SQLiteParameter("@expectedArrivalTime", regularTrip.ExpectedArrivalTime));
                        command.Parameters.Add(new SQLiteParameter("@purposeAndArea", regularTrip.PurposeAndArea));
                        command.Parameters.Add(new SQLiteParameter("@weatherConditions", regularTrip.WeatherConditions));
                        insertedRows += command.ExecuteNonQuery();

                        regularTrip.RegularTripId = db.LastInsertRowId;
                    }
                }

                db.Close();
            }

            return insertedRows == items.Length;
        }

        public bool Update(params RegularTrip[] items)
        {
            int updatedRows = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("UPDATE {0} " +
                                      "SET departure_time = @departureTime, arrival_time = @arrivalTime, " +
                                      "boat_id = @boatId, captain_id = @captainId, logbook_id = @logbookId, " +
                                      "expected_arrival_time = @expectedArrivalTime, " +
                                      "purpose_and_area = @purposeAndArea, weather_conditions = @weatherConditions " +
                                      "WHERE regular_trip_id = @regularTripId ",
                                      DatabaseManager.TableRegularTrips);

                    foreach (RegularTrip regularTrip in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@regularTripId", regularTrip.RegularTripId));
                        // TODO Make sure "Boat" is set, and extract ID.
                        // TODO Make sure "Captain" is set, and extract ID.
                        // TODO Make sure "Logbook" is set, and extract ID.
                        command.Parameters.Add(new SQLiteParameter("@departureTime", regularTrip.DepartureTime));
                        command.Parameters.Add(new SQLiteParameter("@arrivalTime", regularTrip.ArrivalTime));
                        command.Parameters.Add(new SQLiteParameter("@expectedArrivalTime", regularTrip.ExpectedArrivalTime));
                        command.Parameters.Add(new SQLiteParameter("@purposeAndArea", regularTrip.PurposeAndArea));
                        command.Parameters.Add(new SQLiteParameter("@weatherConditions", regularTrip.WeatherConditions));
                        updatedRows += command.ExecuteNonQuery();
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

                    SQLiteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                    regularTrips.Add(
                           new RegularTrip
                           {
                               RegularTripId = reader.GetInt32(reader.GetOrdinal("regular_trip_id")),
                               ArrivalTime = reader.GetDateTime(reader.GetOrdinal("arrival_time")),
                               DepartureTime = reader.GetDateTime(reader.GetOrdinal("departure_time")),
                               ExpectedArrivalTime = reader.GetDateTime(reader.GetOrdinal("expected_arrival_time")),
                               PurposeAndArea = reader.GetString(reader.GetOrdinal("purpose_and_area")),
                               WeatherConditions = reader.GetString(reader.GetOrdinal("weather_conditions"))
                               // TODO Fetch "Boat"
                               // TODO Fetch "Captain"
                               // TODO Fetch "Logbook"
                           });
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

                    SQLiteDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        regularTrip =
                            new RegularTrip
                            {
                                RegularTripId = reader.GetInt32(reader.GetOrdinal("regular_trip_id")),
                                ArrivalTime = reader.GetDateTime(reader.GetOrdinal("arrival_time")),
                                DepartureTime = reader.GetDateTime(reader.GetOrdinal("departure_time")),
                                ExpectedArrivalTime = reader.GetDateTime(reader.GetOrdinal("expected_arrival_time")),
                                PurposeAndArea = reader.GetString(reader.GetOrdinal("purpose_and_area")),
                                WeatherConditions = reader.GetString(reader.GetOrdinal("weather_conditions"))
                                // TODO Fetch "Boat"
                                // TODO Fetch "Captain"
                                // TODO Fetch "Logbook"
                            };
                    }
                }

                db.Close();
            }

            return regularTrip;
        }

        public IEnumerable<RegularTrip> GetAll(Func<RegularTrip, bool> predicate) { return this.GetAll(predicate, true); }

        public IEnumerable<RegularTrip> GetAll(Func<RegularTrip, bool> predicate, bool fetchChildData)
        {
            IEnumerable<RegularTrip> regularTrips = this.GetAll().Where(predicate);

            if (fetchChildData)
            {
                foreach (RegularTrip regularTrip in regularTrips)
                {
                    // TODO Fill this out
                }
            }

            return regularTrips;
        }

        public IEnumerable<RegularTrip> GetRegularTrips(Func<RegularTrip, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RegularTrip> GetReservationsForBoat(Boat boat, bool onlyFuture = true)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RegularTrip> GetReservationsForBoat(Boat boat, DateTime? fromDateTime, DateTime? toDateTime)
        {
            throw new NotImplementedException();
        }

        public bool CanMakeReservation(Boat boat, DateTime? departureTime, DateTime? expectedArrivalTime)
        {
            throw new NotImplementedException();
        }

        public bool CanMakeReservation(RegularTrip trip)
        {
            throw new NotImplementedException();
        }
    }
}
