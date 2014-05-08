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
                        command.Parameters.Add(new SQLiteParameter("@boatId", regularTrip.BoatId));
                        command.Parameters.Add(new SQLiteParameter("@captainId", regularTrip.CaptainId));
                        command.Parameters.Add(new SQLiteParameter("@logbookId", regularTrip.LogbookId));
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
                        command.Parameters.Add(new SQLiteParameter("@boatId", regularTrip.BoatId));
                        command.Parameters.Add(new SQLiteParameter("@captainId", regularTrip.CaptainId));
                        command.Parameters.Add(new SQLiteParameter("@logbookId", regularTrip.LogbookId));
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
                                                 ExpectedArrivalTime =
                                                     reader.GetDateTime(reader.GetOrdinal("expected_arrival_time")),
                                                 PurposeAndArea =
                                                     DatabaseManager.ReadString(reader, reader.GetOrdinal("purpose_and_area")),
                                                 WeatherConditions =
                                                     DatabaseManager.ReadString(reader, reader.GetOrdinal("weather_conditions")),
                                                 BoatId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("boat_id")),
                                                 CaptainId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("captain_id")),
                                                 LogbookId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("logbook_id"))
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
                                    ExpectedArrivalTime = reader.GetDateTime(reader.GetOrdinal("expected_arrival_time")),
                                    PurposeAndArea = DatabaseManager.ReadString(reader, reader.GetOrdinal("purpose_and_area")),
                                    WeatherConditions = DatabaseManager.ReadString(reader, reader.GetOrdinal("weather_conditions")),
                                    BoatId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("boat_id")),
                                    CaptainId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("captain_id")),
                                    LogbookId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("logbook_id"))
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
            throw new NotImplementedException();
        }

        public void LoadData(IEnumerable<RegularTrip> items)
        {
            foreach (var item in items)
            {
                LoadData(item);
            }
        }

        public IEnumerable<RegularTrip> GetAll(Func<RegularTrip, bool> predicate)
        {
            IEnumerable<RegularTrip> regularTrips = this.GetAll().Where(predicate);

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
