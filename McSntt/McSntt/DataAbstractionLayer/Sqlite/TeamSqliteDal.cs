using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Sqlite
{
    public class TeamSqliteDal : ITeamDal
    {
        public bool Create(params Team[] items)
        {
            int insertedRows = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("INSERT INTO {0} (name, level, teacher_id) " +
                                      "VALUES (@name, @level, @teacherId)",
                                      DatabaseManager.TableTeams);

                    foreach (Team team in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@name", team.Name));
                        command.Parameters.Add(new SQLiteParameter("@level", (int)team.Level));
                        command.Parameters.Add(new SQLiteParameter("@teacherId", team.TeacherId));
                        insertedRows += command.ExecuteNonQuery();

                        team.TeamId = db.LastInsertRowId;
                    }
                }

                db.Close();
            }

            return insertedRows == items.Length;
        }

        public bool Update(params Team[] items)
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
                                      "SET name = @name, level = @level, teacher_id = @teacherId " +
                                      "WHERE team_id = @teamId ",
                                      DatabaseManager.TableTeams);

                    foreach (Team team in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@teamId", team.TeamId));
                        command.Parameters.Add(new SQLiteParameter("@name", team.Name));
                        command.Parameters.Add(new SQLiteParameter("@level", (int)team.Level));
                        command.Parameters.Add(new SQLiteParameter("@teacherId", team.TeacherId));
                        updatedRows += command.ExecuteNonQuery();
                    }
                }

                db.Close();
            }

            return updatedRows == items.Length;
        }

        public bool Delete(params Team[] items)
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
                                      "WHERE team_id = @teamId ",
                                      DatabaseManager.TableTeams);

                    foreach (Team team in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@teamId", team.TeamId));
                        deletedRows += command.ExecuteNonQuery();
                    }
                }

                db.Close();
            }

            return deletedRows == items.Length;
        }

        public IEnumerable<Team> GetAll()
        {
            var teams = new List<Team>();

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0}", DatabaseManager.TableTeams);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            teams.Add(
                                      new Team
                                      {
                                          TeamId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("team_id")),
                                          Name = DatabaseManager.ReadString(reader, reader.GetOrdinal("name")),
                                          Level = (Team.ClassLevel) DatabaseManager.ReadInt(reader, reader.GetOrdinal("level")),
                                          TeacherId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("teacher_id"))
                                      });
                        }
                    }
                }

                db.Close();
            }

            return teams;
        }

        public Team GetOne(long itemId)
        {
            Team team = null;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0} " +
                                      "WHERE team_id = @teamId " +
                                      "LIMIT 1",
                                      DatabaseManager.TableTeams); // TODO Fill this out
                    command.Parameters.Add(new SQLiteParameter("@teamId", itemId));

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            team =
                                new Team
                                {
                                    TeamId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("team_id")),
                                    Name = DatabaseManager.ReadString(reader, reader.GetOrdinal("name")),
                                    Level = (Team.ClassLevel)DatabaseManager.ReadInt(reader, reader.GetOrdinal("level")),
                                    TeacherId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("teacher_id"))
                                };
                        }
                    }
                }

                db.Close();
            }

            return team;
        }

        public void LoadData(Team item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Team> GetAll(Func<Team, bool> predicate)
        {
            IEnumerable<Team> teams = this.GetAll().Where(predicate);

            return teams;
        }
    }
}
