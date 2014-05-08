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

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("INSERT INTO {0} (actual_departure_time, actual_arrival_time, " +
                                      "damage_inflicted, damage_description, answer_from_boat_chief, " +
                                      "filed_by_id) " +
                                      "VALUES (@actualDepartureTime, @actualArrivalTime, @damageInflicted, " +
                                      "@damageDescription, @answerFromBoatChief, @filedById)",
                                      DatabaseManager.TableLogbooks);

                    foreach (Logbook logbook in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@filedById", logbook.FiledById));
                        command.Parameters.Add(new SQLiteParameter("@actualDepartureTime", logbook.ActualDepartureTime));
                        command.Parameters.Add(new SQLiteParameter("@actualArrivalTime", logbook.ActualArrivalTime));
                        command.Parameters.Add(new SQLiteParameter("@damageInflicted", logbook.DamageInflicted));
                        command.Parameters.Add(new SQLiteParameter("@damageDescription", logbook.DamageDescription));
                        command.Parameters.Add(new SQLiteParameter("@answerFromBoatChief", logbook.AnswerFromBoatChief));
                        insertedRows += command.ExecuteNonQuery();

                        logbook.LogbookId = db.LastInsertRowId;
                    }
                }

                db.Close();
            }

            return insertedRows == items.Length;
        }

        public bool Update(params Logbook[] items)
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
                                      "SET  actual_departure_time = @actualDepartureTime, " +
                                      "actual_arrival_time = @actualArrivalTime, " +
                                      "damage_inflicted = @damageInflicted, " +
                                      "damage_description = @damageDescription, " +
                                      "answer_from_boat_chief = @answerFromBoatChief, " +
                                      "filed_by_id = @filedById " +
                                      "WHERE logbook_id = @logbookId",
                                      DatabaseManager.TableLogbooks);

                    foreach (Logbook logbook in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@logbookId", logbook.LogbookId));
                        command.Parameters.Add(new SQLiteParameter("@filedById", logbook.FiledById));
                        command.Parameters.Add(new SQLiteParameter("@actualDepartureTime", logbook.ActualDepartureTime));
                        command.Parameters.Add(new SQLiteParameter("@actualArrivalTime", logbook.ActualArrivalTime));
                        command.Parameters.Add(new SQLiteParameter("@damageInflicted", logbook.DamageInflicted));
                        command.Parameters.Add(new SQLiteParameter("@damageDescription", logbook.DamageDescription));
                        command.Parameters.Add(new SQLiteParameter("@answerFromBoatChief", logbook.AnswerFromBoatChief));
                        updatedRows += command.ExecuteNonQuery();
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
            throw new NotImplementedException();
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
            IEnumerable<Logbook> logbooks  = this.GetAll().Where(predicate);

            return logbooks;
        }
    }
}