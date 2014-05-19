using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Sqlite
{
    public class BoatSqliteDal : IBoatDal
    {
        #region IBoatDal Members
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
                        command.Parameters.Add(new SQLiteParameter("@type", (int) boat.Type));
                        command.Parameters.Add(new SQLiteParameter("@nickname", boat.NickName));
                        command.Parameters.Add(new SQLiteParameter("@imagePath", boat.ImagePath));
                        command.Parameters.Add(new SQLiteParameter("@operational", boat.Operational));
                        insertedRows += command.ExecuteNonQuery();

                        boat.BoatId = db.LastInsertRowId;
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
                        command.Parameters.Add(new SQLiteParameter("@boatId", (int) boat.BoatId));
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

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("DELETE FROM {0} " +
                                      "WHERE boat_id = @boatId",
                                      DatabaseManager.TableBoats);

                    foreach (Boat boat in items)
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

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0}", DatabaseManager.TableBoats);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            boats.Add(new Boat
                                      {
                                          BoatId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("boat_id")),
                                          ImagePath =
                                              DatabaseManager.ReadString(reader, reader.GetOrdinal("image_path")),
                                          NickName = DatabaseManager.ReadString(reader, reader.GetOrdinal("nickname")),
                                          Operational =
                                              DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("operational")),
                                          Type = (BoatType) DatabaseManager.ReadInt(reader, reader.GetOrdinal("type"))
                                      });
                        }
                    }
                }

                db.Close();
            }

            return boats;
        }

        public Boat GetOne(long itemId)
        {
            Boat boat = null;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0} " +
                                      "WHERE boat_id = @boatId " +
                                      "LIMIT 1",
                                      DatabaseManager.TableBoats);
                    command.Parameters.Add(new SQLiteParameter("@boatId", itemId));

                    SQLiteDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        boat = new Boat
                               {
                                   BoatId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("boat_id")),
                                   ImagePath = DatabaseManager.ReadString(reader, reader.GetOrdinal("image_path")),
                                   NickName = DatabaseManager.ReadString(reader, reader.GetOrdinal("nickname")),
                                   Operational = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("operational")),
                                   Type = (BoatType) DatabaseManager.ReadInt(reader, reader.GetOrdinal("type")),
                               };
                    }
                }

                db.Close();
            }

            return boat;
        }

        public void LoadData(Boat item) { }

        public void LoadData(IEnumerable<Boat> items) { }

        public IEnumerable<Boat> GetAll(Func<Boat, bool> predicate)
        {
            Boat[] boats = this.GetAll().ToArray();

            LoadData(boats);

            return boats.Where(predicate);
        }
        #endregion
    }
}
