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
            int lectureRowsInserted = 0;
            int studentRowsInserted = 0;
            int studentRowsExpected = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand lectureCommand = db.CreateCommand())
                {
                    using (var studentCommand = db.CreateCommand())
                    {
                        lectureCommand.CommandType = CommandType.Text;
                        lectureCommand.CommandText =
                            String.Format("INSERT INTO {0} (team_id, date_of_lecture, rope_works, navigation, " +
                                          "motor, drabant, gaffelrigger, night) " +
                                          "VALUES (@teamId, @dateOfLecture, @ropeWorks, @navigation, @motor, " +
                                          "@drabant, @gaffelrigger, @night)",
                                          DatabaseManager.TableLectures);

                        studentCommand.CommandType = CommandType.Text;
                        studentCommand.CommandText =
                            String.Format("INSERT OR IGNORE INTO {0} (lecture_id, student_member_id) " +
                                          "VALUES (@lectureId, @studentMemberId)",
                                          DatabaseManager.TableLecturePresentMembersBinder);

                        foreach (Lecture lecture in items)
                        {
                            using (SQLiteTransaction transaction = db.BeginTransaction())
                            {
                                lectureCommand.Parameters.Clear();
                                lectureCommand.Parameters.Add(new SQLiteParameter("@teamId", lecture.TeamId));
                                lectureCommand.Parameters.Add(new SQLiteParameter("@dateOfLecture",
                                                                                  lecture.DateOfLecture));
                                lectureCommand.Parameters.Add(new SQLiteParameter("@ropeWorks", lecture.RopeWorksLecture));
                                lectureCommand.Parameters.Add(new SQLiteParameter("@navigation", lecture.Navigation));
                                lectureCommand.Parameters.Add(new SQLiteParameter("@motor", lecture.Motor));
                                lectureCommand.Parameters.Add(new SQLiteParameter("@drabant", lecture.Drabant));
                                lectureCommand.Parameters.Add(new SQLiteParameter("@gaffelrigger", lecture.Gaffelrigger));
                                lectureCommand.Parameters.Add(new SQLiteParameter("@night", lecture.Night));
                                lectureRowsInserted = lectureCommand.ExecuteNonQuery();

                                lecture.LectureId = db.LastInsertRowId;

                                // Link to present members
                                studentRowsExpected = 0;

                                if (lecture.PresentMembers != null)
                                {
                                    studentRowsExpected = lecture.PresentMembers.Count;
                                    studentRowsInserted = 0;

                                    foreach (StudentMember studentMember in lecture.PresentMembers)
                                    {
                                        studentCommand.Parameters.Clear();
                                        studentCommand.Parameters.Add(new SQLiteParameter("@lectureId",
                                                                                          lecture.LectureId));
                                        studentCommand.Parameters.Add(new SQLiteParameter("@studentMemberId",
                                                                                          studentMember.StudentMemberId));
                                        studentRowsInserted += studentCommand.ExecuteNonQuery();
                                    }
                                }

                                // Verify that everything was inserted correctly
                                if (studentRowsInserted == studentRowsExpected)
                                {
                                    transaction.Commit();
                                    insertedRows += lectureRowsInserted;
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

        public bool Update(params Lecture[] items)
        {
            int updatedRows = 0;
            int lectureRowsUpdated = 0;
            int studentRowsInserted = 0;
            int studentRowsExpected = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand lectureCommand = db.CreateCommand())
                {
                    using (var studentCommand = db.CreateCommand())
                    {
                        lectureCommand.CommandType = CommandType.Text;
                        lectureCommand.CommandText =
                            String.Format("UPDATE {0} " +
                                          "SET  team_id = @teamId, date_of_lecture = @dateOfLecture, " +
                                          "rope_works = @ropeWorks, navigation = @navigation, motor = @motor, " +
                                          "drabant = @motor, gaffelrigger = @gaffelrigger, night = @night" +
                                          "WHERE lecture_id = @lectureId",
                                          DatabaseManager.TableLectures);

                        studentCommand.CommandType = CommandType.Text;
                        studentCommand.CommandText =
                            String.Format("INSERT OR IGNORE INTO {0} (lecture_id, student_member_id) " +
                                          "VALUES (@lectureId, @studentMemberId)",
                                          DatabaseManager.TableLecturePresentMembersBinder);

                        foreach (Lecture lecture in items)
                        {
                            using (SQLiteTransaction transaction = db.BeginTransaction())
                            {
                                lectureCommand.Parameters.Clear();
                                lectureCommand.Parameters.Add(new SQLiteParameter("@lectureId", lecture.LectureId));
                                lectureCommand.Parameters.Add(new SQLiteParameter("@teamId", lecture.TeamId));
                                lectureCommand.Parameters.Add(new SQLiteParameter("@dateOfLecture", lecture.DateOfLecture));
                                lectureCommand.Parameters.Add(new SQLiteParameter("@ropeWorks", lecture.RopeWorksLecture));
                                lectureCommand.Parameters.Add(new SQLiteParameter("@navigation", lecture.Navigation));
                                lectureCommand.Parameters.Add(new SQLiteParameter("@motor", lecture.Motor));
                                lectureCommand.Parameters.Add(new SQLiteParameter("@drabant", lecture.Drabant));
                                lectureCommand.Parameters.Add(new SQLiteParameter("@gaffelrigger", lecture.Gaffelrigger));
                                lectureCommand.Parameters.Add(new SQLiteParameter("@night", lecture.Night));
                                updatedRows += lectureCommand.ExecuteNonQuery();

                                // Link to present members, removing existing ones first
                                using (SQLiteCommand deleteCommand = db.CreateCommand())
                                {
                                    deleteCommand.CommandType = CommandType.Text;
                                    deleteCommand.CommandText =
                                        String.Format("DELETE FROM {0} " +
                                                      "WHERE lecture_id = @lectureId",
                                                      DatabaseManager.TableLecturePresentMembersBinder);
                                    deleteCommand.Parameters.Add(new SQLiteParameter("@lectureId", lecture.LectureId));
                                    deleteCommand.ExecuteNonQuery();
                                }

                                studentRowsExpected = 0;

                                if (lecture.PresentMembers != null)
                                {
                                    studentRowsExpected = lecture.PresentMembers.Count;
                                    studentRowsInserted = 0;

                                    foreach (StudentMember studentMember in lecture.PresentMembers)
                                    {
                                        studentCommand.Parameters.Clear();
                                        studentCommand.Parameters.Add(new SQLiteParameter("@lectureId",
                                                                                          lecture.LectureId));
                                        studentCommand.Parameters.Add(new SQLiteParameter("@studentMemberId",
                                                                                          studentMember.StudentMemberId));
                                        studentRowsInserted += studentCommand.ExecuteNonQuery();
                                    }
                                }

                                // Verify that everything was inserted correctly
                                if (studentRowsInserted == studentRowsExpected)
                                {
                                    transaction.Commit();
                                    updatedRows += lectureRowsUpdated;
                                }
                                else
                                { transaction.Rollback(); }
                            }
                        }
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

                        using (SQLiteCommand deleteCommand = db.CreateCommand())
                        {
                            deleteCommand.CommandType = CommandType.Text;
                            deleteCommand.CommandText =
                                String.Format("DELETE FROM {0} " +
                                              "WHERE lecture_id = @lectureId",
                                              DatabaseManager.TableEventParticipantsBinder);
                            deleteCommand.Parameters.Add(new SQLiteParameter("@lectureId", lecture.LectureId));
                            deleteCommand.ExecuteNonQuery();
                        }
                    }
                }

                db.Close();
            }

            return deletedRows == items.Length;
        }

        public IEnumerable<Lecture> GetAll()
        {
            var lectures = new List<Lecture>();

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0}", DatabaseManager.TableLectures);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int teamId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("team_id"));

                            lectures.
                                Add(
                                    new Lecture
                                    {
                                        LectureId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("lecture_id")),
                                        DateOfLecture = reader.GetDateTime(reader.GetOrdinal("date_of_lecture")),
                                        Drabant = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("drabant")),
                                        Gaffelrigger = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("gaffelrigger")),
                                        Motor = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("motor")),
                                        Navigation = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("navigation")),
                                        Night = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("night")),
                                        RopeWorksLecture = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("rope_works")),
                                        TeamId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("team_id"))
                                    });
                        }
                    }
                }

                db.Close();
            }

            return lectures
                ;
        }

        public Lecture GetOne(long itemId)
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

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int teamId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("team_id"));

                            lecture =
                                new Lecture
                                {
                                    LectureId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("lecture_id")),
                                    DateOfLecture = reader.GetDateTime(reader.GetOrdinal("date_of_lecture")),
                                    Drabant = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("drabant")),
                                    Gaffelrigger = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("gaffelrigger")),
                                    Motor = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("motor")),
                                    Navigation = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("navigation")),
                                    Night = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("night")),
                                    RopeWorksLecture = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("rope_works")),
                                    TeamId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("team_id"))
                                };
                        }
                    }
                }

                db.Close();
            }

            return lecture;
        }

        public void LoadData(Lecture item)
        {
            // Load Team
            var teamDal = DalLocator.TeamDal;

            if (item.TeamId > 0) { item.Team = teamDal.GetOne(item.TeamId); }

            // Load PresentMembers
            var studentDal = DalLocator.StudentMemberDal;

            using (var db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (var command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT student_member_id FROM {0} WHERE lecture_id = @lectureId",
                                      DatabaseManager.TableLecturePresentMembersBinder);
                    command.Parameters.Add(new SQLiteParameter("@lectureId", item.LectureId));

                    item.PresentMembers = new List<StudentMember>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long studentMemberId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("student_member_id"));

                            if (studentMemberId > 0)
                            {
                                item.PresentMembers.Add(studentDal.GetOne(studentMemberId));
                            }
                        }
                    }
                }

                db.Close();
            }
        }

        public void LoadData(IEnumerable<Lecture> items)
        {
            foreach (var item in items)
            {
                LoadData(item);
            }
        }

        public IEnumerable<Lecture> GetAll(Func<Lecture, bool> predicate)
        {
            var lectures = this.GetAll().ToArray();

            LoadData(lectures);

            return lectures.Where(predicate);
        }
    }
}
