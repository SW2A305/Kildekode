using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Sqlite
{
    public class LectureSqliteDal : ILectureDal
    {
        public bool Create(params Lecture[] items)
        {
            int insertedRows = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("INSERT INTO {0} (team_id, date_of_lecture, rope_works, navigation, " +
                                      "motor, drabant, gaffelrigger, night) " +
                                      "VALUES (@teamId, @dateOfLecture, @ropeWorks, @navigation, @motor, " +
                                      "@drabant, @gaffelrigger, @night)",
                                      DatabaseManager.TableLectures);

                    foreach (Lecture lecture in items)
                    {
                        command.Parameters.Clear();
                        // TODO Make sure Team is set, and extract ID.
                        command.Parameters.Add(new SQLiteParameter("@dateOfLecture", lecture.DateOfLecture));
                        command.Parameters.Add(new SQLiteParameter("@ropeWorks", lecture.RopeWorksLecture));
                        command.Parameters.Add(new SQLiteParameter("@navigation", lecture.Navigation));
                        command.Parameters.Add(new SQLiteParameter("@motor", lecture.Motor));
                        command.Parameters.Add(new SQLiteParameter("@drabant", lecture.Drabant));
                        command.Parameters.Add(new SQLiteParameter("@gaffelrigger", lecture.Gaffelrigger));
                        command.Parameters.Add(new SQLiteParameter("@night", lecture.Night));
                        insertedRows += command.ExecuteNonQuery();
                    }
                }

                db.Close();
            }

            return insertedRows == items.Length;
        }

        public bool Update(params Lecture[] items)
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
                                      "SET  team_id = @teamId, date_of_lecture = @dateOfLecture, " +
                                      "rope_works = @ropeWorks, navigation = @navigation, motor = @motor, " +
                                      "drabant = @motor, gaffelrigger = @gaffelrigger, night = @night" +
                                      "WHERE lecture_id = @lectureId",
                                      DatabaseManager.TableLectures);

                    foreach (Lecture lecture in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@lectureId", lecture.LectureId));
                        // TODO Make sure Team is set, and extract ID.
                        command.Parameters.Add(new SQLiteParameter("@dateOfLecture", lecture.DateOfLecture));
                        command.Parameters.Add(new SQLiteParameter("@ropeWorks", lecture.RopeWorksLecture));
                        command.Parameters.Add(new SQLiteParameter("@navigation", lecture.Navigation));
                        command.Parameters.Add(new SQLiteParameter("@motor", lecture.Motor));
                        command.Parameters.Add(new SQLiteParameter("@drabant", lecture.Drabant));
                        command.Parameters.Add(new SQLiteParameter("@gaffelrigger", lecture.Gaffelrigger));
                        command.Parameters.Add(new SQLiteParameter("@night", lecture.Night));
                        updatedRows += command.ExecuteNonQuery();
                    }
                }

                db.Close();
            }

            return updatedRows == items.Length;
        }

        public bool Delete(params Lecture[] items)
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
                                      "WHERE lecture_id = @lectureId",
                                      DatabaseManager.TableLectures);

                    foreach (Lecture lecture in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@lectureId", lecture.LectureId));
                        deletedRows += command.ExecuteNonQuery();
                    }
                }

                db.Close();
            }

            return deletedRows == items.Length;
        }

        public IEnumerable<Lecture> GetAll()
        {
            var lectures  = new List<Lecture>();

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0}", DatabaseManager.TableLectures);

                    SQLiteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                    lectures.
                        Add(
                            new Lecture
                            {
                                LectureId = reader.GetInt32(reader.GetOrdinal("lecture_id")),
                                DateOfLecture = reader.GetDateTime(reader.GetOrdinal("date_of_lecture")),
                                Drabant = reader.GetBoolean(reader.GetOrdinal("drabant")),
                                Gaffelrigger = reader.GetBoolean(reader.GetOrdinal("gaffelrigger")),
                                Motor = reader.GetBoolean(reader.GetOrdinal("motor")),
                                Navigation = reader.GetBoolean(reader.GetOrdinal("navigation")),
                                Night = reader.GetBoolean(reader.GetOrdinal("night")),
                                RopeWorksLecture = reader.GetBoolean(reader.GetOrdinal("rope_works"))
                                // TODO Fetch Team
                            });
                    }
                }

                db.Close();
            }

            return lectures
            ;
        }

        public Lecture GetOne(int itemId)
        {
            Lecture lecture = null;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0} " +
                                      "WHERE lecture_id = @lectureId " +
                                      "LIMIT 1",
                                      DatabaseManager.TableLectures);
                    command.Parameters.Add(new SQLiteParameter("@lectureId", itemId));

                    SQLiteDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        lecture =
                            new Lecture
                            {
                                LectureId = reader.GetInt32(reader.GetOrdinal("lecture_id")),
                                DateOfLecture = reader.GetDateTime(reader.GetOrdinal("date_of_lecture")),
                                Drabant = reader.GetBoolean(reader.GetOrdinal("drabant")),
                                Gaffelrigger = reader.GetBoolean(reader.GetOrdinal("gaffelrigger")),
                                Motor = reader.GetBoolean(reader.GetOrdinal("motor")),
                                Navigation = reader.GetBoolean(reader.GetOrdinal("navigation")),
                                Night = reader.GetBoolean(reader.GetOrdinal("night")),
                                RopeWorksLecture = reader.GetBoolean(reader.GetOrdinal("rope_works"))
                                // TODO Fetch Team
                            };
                    }
                }

                db.Close();
            }

            return lecture;
        }

        public IEnumerable<Lecture> GetAll(Func<Lecture, bool> predicate) { return this.GetAll(predicate, true); }

        public IEnumerable<Lecture> GetAll(Func<Lecture, bool> predicate, bool fetchChildData)
        {
            IEnumerable<Lecture> lectures  = this.GetAll().Where(predicate);

            if (fetchChildData)
            {
                foreach (Lecture lecture in lectures)
                {
                    // TODO Fill this out
                }
            }

            return lectures;
        }
    }
}
