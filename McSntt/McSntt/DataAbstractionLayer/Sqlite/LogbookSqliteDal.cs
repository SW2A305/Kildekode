using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Sqlite
{
    public class LogbookSqliteDal : ILogbookDal
    {
        public bool Create(params Logbook[] items)
        {
            int insertedRows = 0;
            int logbookRowsInserted = 0;
            int crewRowsInserted = 0;
            int crewRowsExpected = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand logbookCommand = db.CreateCommand())
                {
                    using (var personCommand = db.CreateCommand())
                    {
                        logbookCommand.CommandType = CommandType.Text;
                        logbookCommand.CommandText =
                            String.Format("INSERT INTO {0} (actual_departure_time, actual_arrival_time, " +
                                          "damage_inflicted, damage_description, answer_from_boat_chief, " +
                                          "filed_by_id) " +
                                          "VALUES (@actualDepartureTime, @actualArrivalTime, @damageInflicted, " +
                                          "@damageDescription, @answerFromBoatChief, @filedById)",
                                          DatabaseManager.TableLogbooks);

                        personCommand.CommandType = CommandType.Text;
                        personCommand.CommandText =
                            String.Format("INSERT OR IGNORE INTO {0} (logbook_id, person_id) " +
                                          "VALUES (@logbookId, @personId)",
                                          DatabaseManager.TableLogbookActualCrewBinder);

                        foreach (Logbook logbook in items)
                        {
                            using (var transaction = db.BeginTransaction())
                            {
                                logbookCommand.Parameters.Clear();
                                logbookCommand.Parameters.Add(new SQLiteParameter("@filedById", logbook.FiledById));
                                logbookCommand.Parameters.Add(new SQLiteParameter("@actualDepartureTime",
                                                                                  logbook.ActualDepartureTime));
                                logbookCommand.Parameters.Add(new SQLiteParameter("@actualArrivalTime",
                                                                                  logbook.ActualArrivalTime));
                                logbookCommand.Parameters.Add(new SQLiteParameter("@damageInflicted",
                                                                                  logbook.DamageInflicted));
                                logbookCommand.Parameters.Add(new SQLiteParameter("@damageDescription",
                                                                                  logbook.DamageDescription));
                                logbookCommand.Parameters.Add(new SQLiteParameter("@answerFromBoatChief",
                                                                                  logbook.AnswerFromBoatChief));
                                logbookRowsInserted = logbookCommand.ExecuteNonQuery();

                                logbook.LogbookId = db.LastInsertRowId;

                                // Link to participants
                                crewRowsExpected = 0;

                                if (logbook.ActualCrew != null)
                                {
                                    crewRowsExpected = logbook.ActualCrew.Count;
                                    crewRowsInserted = 0;

                                    foreach (Person person in logbook.ActualCrew)
                                    {
                                        personCommand.Parameters.Clear();
                                        personCommand.Parameters.Add(new SQLiteParameter("@logbookId",
                                                                                          logbook.LogbookId));
                                        personCommand.Parameters.Add(new SQLiteParameter("@personId",
                                                                                          person.PersonId));
                                        crewRowsInserted += personCommand.ExecuteNonQuery();
                                    }
                                }

                                // Verify that everything was inserted correctly
                                if (crewRowsInserted == crewRowsExpected)
                                {
                                    transaction.Commit();
                                    insertedRows += logbookRowsInserted;
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

        public bool Update(params Logbook[] items)
        {
            int updatedRows = 0;
            int logbookRowsUpdated = 0;
            int crewRowsInserted = 0;
            int crewRowsExpected = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    using (var personCommand = db.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText =
                            String.Format("UPDATE {0} " +
                                          "SET  actual_departure_time = @actualDepartureTime, " +
                                          "actual_arrival_time = @actualArrivalTime, " +
                                          "damage_inflicted = @damageInflicted, " +
                                          "damage_description = @damageDescription, " +
                                          "answer_from_boat_chief = @answerFromBoatChief, " +
                                          "filed_by_id = @filedById " +
                                          "WHERE logbook_id = @logbookId",
                                          DatabaseManager.TableLogbooks);

                        personCommand.CommandType = CommandType.Text;
                        personCommand.CommandText =
                            String.Format("INSERT OR IGNORE INTO {0} (logbook_id, person_id) " +
                                          "VALUES (@logbookId, @personId)",
                                          DatabaseManager.TableLogbookActualCrewBinder);

                        foreach (Logbook logbook in items)
                        {
                            using (var transaction = db.BeginTransaction())
                            {
                                command.Parameters.Clear();
                                command.Parameters.Add(new SQLiteParameter("@logbookId", logbook.LogbookId));
                                command.Parameters.Add(new SQLiteParameter("@filedById", logbook.FiledById));
                                command.Parameters.Add(new SQLiteParameter("@actualDepartureTime",
                                                                           logbook.ActualDepartureTime));
                                command.Parameters.Add(new SQLiteParameter("@actualArrivalTime",
                                                                           logbook.ActualArrivalTime));
                                command.Parameters.Add(new SQLiteParameter("@damageInflicted", logbook.DamageInflicted));
                                command.Parameters.Add(new SQLiteParameter("@damageDescription",
                                                                           logbook.DamageDescription));
                                command.Parameters.Add(new SQLiteParameter("@answerFromBoatChief",
                                                                           logbook.AnswerFromBoatChief));
                                logbookRowsUpdated = command.ExecuteNonQuery();

                                // Link to actual crew, removing existing ones first
                                using (SQLiteCommand deleteCommand = db.CreateCommand())
                                {
                                    deleteCommand.CommandType = CommandType.Text;
                                    deleteCommand.CommandText =
                                        String.Format("DELETE FROM {0} " +
                                                      "WHERE logbook_id = @logbookId",
                                                      DatabaseManager.TableLogbookActualCrewBinder);
                                    deleteCommand.Parameters.Add(new SQLiteParameter("@logbookId", logbook.LogbookId));
                                    deleteCommand.ExecuteNonQuery();
                                }

                                crewRowsExpected = 0;

                                if (logbook.ActualCrew != null)
                                {
                                    crewRowsExpected = logbook.ActualCrew.Count;
                                    crewRowsInserted = 0;

                                    foreach (Person person in logbook.ActualCrew)
                                    {
                                        personCommand.Parameters.Clear();
                                        personCommand.Parameters.Add(new SQLiteParameter("@logbookId",
                                                                                         logbook.LogbookId));
                                        personCommand.Parameters.Add(new SQLiteParameter("@personId",
                                                                                         person.PersonId));
                                        crewRowsInserted += personCommand.ExecuteNonQuery();
                                    }
                                }

                                // Verify that everything was inserted correctly
                                if (crewRowsInserted == crewRowsExpected)
                                {
                                    transaction.Commit();
                                    updatedRows += logbookRowsUpdated;
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

        public bool Delete(params Logbook[] items)
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
                                      "WHERE logbook_id = @logbookId",
                                      DatabaseManager.TableLogbooks); // TODO Fill this out

                    foreach (Logbook logbook in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@logbookId", logbook.LogbookId));
                        deletedRows += command.ExecuteNonQuery();

                        using (SQLiteCommand deleteCommand = db.CreateCommand())
                        {
                            deleteCommand.CommandType = CommandType.Text;
                            deleteCommand.CommandText =
                                String.Format("DELETE FROM {0} " +
                                              "WHERE logbook_id = @logbookId",
                                              DatabaseManager.TableLogbookActualCrewBinder);
                            deleteCommand.Parameters.Add(new SQLiteParameter("@logbookId", logbook.LogbookId));
                            deleteCommand.ExecuteNonQuery();
                        }
                    }
                }

                db.Close();
            }

            return deletedRows == items.Length;
        }

        public IEnumerable<Logbook> GetAll()
        {
            var logbooks  = new List<Logbook>();

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0}", DatabaseManager.TableLogbooks);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            logbooks.
                                Add(
                                    new Logbook
                                    {
                                        LogbookId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("logbook_id")),
                                        ActualArrivalTime = reader.GetDateTime(reader.GetOrdinal("actual_arrival_time")),
                                        ActualDepartureTime =
                                            reader.GetDateTime(reader.GetOrdinal("actual_departure_time")),
                                        AnswerFromBoatChief =
                                            DatabaseManager.ReadString(reader, reader.GetOrdinal("answer_from_boat_chief")),
                                        DamageDescription = DatabaseManager.ReadString(reader, reader.GetOrdinal("damage_description")),
                                        DamageInflicted = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("damage_inflicted")),
                                        FiledById = DatabaseManager.ReadInt(reader, reader.GetOrdinal("filed_by_id"))
                                    });
                        }
                    }
                }

                db.Close();
            }

            return logbooks
            ;
        }

        public Logbook GetOne(long itemId)
        {
            Logbook logbook = null;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0} " +
                                      "WHERE logbook_id = @logbookId " +
                                      "LIMIT 1",
                                      DatabaseManager.TableLogbooks);
                    command.Parameters.Add(new SQLiteParameter("@logbookId", itemId));

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            logbook =
                                new Logbook
                                {
                                    LogbookId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("logbook_id")),
                                    ActualArrivalTime = reader.GetDateTime(reader.GetOrdinal("actual_arrival_time")),
                                    ActualDepartureTime = reader.GetDateTime(reader.GetOrdinal("actual_departure_time")),
                                    AnswerFromBoatChief = DatabaseManager.ReadString(reader, reader.GetOrdinal("answer_from_boat_chief")),
                                    DamageDescription = DatabaseManager.ReadString(reader, reader.GetOrdinal("damage_description")),
                                    DamageInflicted = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("damage_inflicted")),
                                    FiledById = DatabaseManager.ReadInt(reader, reader.GetOrdinal("filed_by_id"))
                                };
                        }
                    }
                }

                db.Close();
            }

            return logbook;
        }

        public void LoadData(Logbook item)
        {
            // Load FiledBy
            var memberDal = DalLocator.SailClubMemberDal;
            
            if (item.FiledById > 0) { item.FiledBy = memberDal.GetOne(item.FiledById); }

            // Load ActualCrew
            var personDal = DalLocator.PersonDal;

            using (var db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (var command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT person_id FROM {0} WHERE logbook_id = @logbookId",
                                      DatabaseManager.TableLogbookActualCrewBinder);
                    command.Parameters.Add(new SQLiteParameter("@logbookId", item.LogbookId));

                    item.ActualCrew = new List<Person>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long personId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("person_id"));

                            if (personId > 0)
                            {
                                item.ActualCrew.Add(personDal.GetOne(personId));
                            }
                        }
                    }
                }

                db.Close();
            }
        }

        public void LoadData(IEnumerable<Logbook> items)
        {
            foreach (var item in items)
            {
                LoadData(item);
            }
        }

        public IEnumerable<Logbook> GetAll(Func<Logbook, bool> predicate)
        {
            var logbooks  = this.GetAll().ToArray();

            LoadData(logbooks);

            return logbooks.Where(predicate);
        }
    }
}