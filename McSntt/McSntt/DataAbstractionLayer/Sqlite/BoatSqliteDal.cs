using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Sqlite
{
    public class BoatSqliteDal : IBoatDal
    {
        public bool Create(params Boat[] items)
        {
            int insertedRows = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("INSERT INTO {0} (type, nickname, image_path, operational) " +
                                      "VALUES (@type, @nickname, @imagePath, @operational)",
                                      DatabaseManager.TableBoats);

                    foreach (Boat boat in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@type", boat.Type));
                        command.Parameters.Add(new SQLiteParameter("@nickname", boat.NickName));
                        command.Parameters.Add(new SQLiteParameter("@imagePath", boat.ImagePath));
                        command.Parameters.Add(new SQLiteParameter("@operational", boat.Operational));
                        insertedRows += command.ExecuteNonQuery();
                    }
                }

                db.Close();
            }

            return insertedRows == items.Length;
        }

        public bool Update(params Boat[] items)
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
                                      "SET type = @type, nickname = @nickname, image_path = @imagePath, " +
                                      "operational = @operational " +
                                      "WHERE boat_id = @boatId",
                                      DatabaseManager.TableBoats);

                    foreach (Boat boat in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@boatId", boat.BoatId));
                        command.Parameters.Add(new SQLiteParameter("@type", boat.Type));
                        command.Parameters.Add(new SQLiteParameter("@nickname", boat.NickName));
                        command.Parameters.Add(new SQLiteParameter("@imagePath", boat.ImagePath));
                        command.Parameters.Add(new SQLiteParameter("@operational", boat.Operational));
                        updatedRows += command.ExecuteNonQuery();
                    }
                }

                db.Close();
            }

            return updatedRows == items.Length;
        }

        public bool Delete(params Boat[] items)
        {
            int deletedRows = 0;

            using (var db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (var command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("DELETE FROM {0} " +
                                      "WHERE boat_id = @boatId",
                                      DatabaseManager.TableBoats);

                    foreach (var boat in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@boatId", boat.BoatId));
                        deletedRows += command.ExecuteNonQuery();
                    }
                }

                db.Close();
            }

            return deletedRows == items.Length;
        }

        public IEnumerable<Boat> GetAll()
        {
            var boats = new List<Boat>();

            using (var db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (var command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0}", DatabaseManager.TableBoats);

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        boats.Add(new Boat()
                        {
                            BoatId = reader.GetInt32(reader.GetOrdinal("boat_id")),
                            ImagePath = reader.GetString(reader.GetOrdinal("image_path")),
                            NickName = reader.GetString(reader.GetOrdinal("nickname")),
                            Operational = reader.GetBoolean(reader.GetOrdinal("operational"))
                        });
                    }
                }

                db.Close();
            }

            return boats;
        }

        public Boat GetOne(int itemId)
        {
            Boat boat = null;

            using (var db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (var command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0} " +
                                      "WHERE boat_id = @boatId " +
                                      "LIMIT 1",
                                      DatabaseManager.TableBoats);
                    command.Parameters.Add(new SQLiteParameter("@boatId", itemId));

                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        boat = new Boat
                               {
                                   BoatId = reader.GetInt32(reader.GetOrdinal("boat_id")),
                                   ImagePath = reader.GetString(reader.GetOrdinal("image_path")),
                                   NickName = reader.GetString(reader.GetOrdinal("nickname")),
                                   Operational = reader.GetBoolean(reader.GetOrdinal("operational"))
                               };
                    }
                }

                db.Close();
            }

            return boat;
        }
    }
}
