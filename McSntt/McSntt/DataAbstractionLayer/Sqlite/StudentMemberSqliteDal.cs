using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Sqlite
{
    public class StudentMemberSqliteDal : IStudentMemberDal
    {
        public bool Create(params StudentMember[] items)
        {
            var sailClubMemberDal = DalLocator.SailClubMemberDal;
            int insertedRows = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("INSERT INTO {0} (sail_club_member_id, team_id, rope_works, navigation, " +
                                      "motor, drabant, gaffelrigger, night) " +
                                      "VALUES (@sailClubMemberId, @teamId, @ropeWorks, @navigation, @motor, " +
                                      "@drabant, @gaffelrigger, @night)",
                                      DatabaseManager.TableStudentMembers);

                    foreach (StudentMember studentMember in items)
                    {
                        // Store the 'SailClubMember' part first, only proceeding if we succeed
                        if (!sailClubMemberDal.Create(studentMember)) { continue; }

                        command.Parameters.Clear();
                        // TODO Make sure Team is set, and extract ID.
                        command.Parameters.Add(new SQLiteParameter("@sailClubMemberId", studentMember.SailClubMemberId));
                        command.Parameters.Add(new SQLiteParameter("@ropeWorks", studentMember.RopeWorks));
                        command.Parameters.Add(new SQLiteParameter("@navigation", studentMember.Navigation));
                        command.Parameters.Add(new SQLiteParameter("@motor", studentMember.Motor));
                        command.Parameters.Add(new SQLiteParameter("@drabant", studentMember.Drabant));
                        command.Parameters.Add(new SQLiteParameter("@gaffelrigger", studentMember.Gaffelrigger));
                        command.Parameters.Add(new SQLiteParameter("@night", studentMember.Night));
                        insertedRows += command.ExecuteNonQuery();

                        studentMember.StudentMemberId = (int) db.LastInsertRowId;
                    }
                }

                db.Close();
            }

            return insertedRows == items.Length;
        }

        public bool Update(params StudentMember[] items)
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
                                      "SET sail_club_member_id = @sailClubMemberId, team_id = @teamId, " +
                                      "rope_works = @ropeWorks, navigation = @navigation, motor = @motor, " +
                                      "drabant = @drabant, gaffelrigger = @gaffelrigger, night = @night " +
                                      "WHERE student_member_id = @studentMemberId ",
                                      DatabaseManager.TableStudentMembers);

                    foreach (StudentMember studentMember in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@studentMemberId", studentMember.StudentMemberId));
                        // TODO Make sure Team is set, and extract ID.
                        command.Parameters.Add(new SQLiteParameter("@sailClubMemberId", studentMember.SailClubMemberId));
                        command.Parameters.Add(new SQLiteParameter("@ropeWorks", studentMember.RopeWorks));
                        command.Parameters.Add(new SQLiteParameter("@navigation", studentMember.Navigation));
                        command.Parameters.Add(new SQLiteParameter("@motor", studentMember.Motor));
                        command.Parameters.Add(new SQLiteParameter("@drabant", studentMember.Drabant));
                        command.Parameters.Add(new SQLiteParameter("@gaffelrigger", studentMember.Gaffelrigger));
                        command.Parameters.Add(new SQLiteParameter("@night", studentMember.Night));
                        updatedRows += command.ExecuteNonQuery();
                    }
                }

                db.Close();
            }

            return updatedRows == items.Length;
        }

        public bool Delete(params StudentMember[] items)
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
                                      "WHERE student_member_id = @studentMemberId",
                                      DatabaseManager.TableStudentMembers);

                    foreach (StudentMember studentMember in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@studentMemberId", studentMember.StudentMemberId));
                        deletedRows += command.ExecuteNonQuery();
                    }
                }

                db.Close();
            }

            return deletedRows == items.Length;
        }

        public IEnumerable<StudentMember> GetAll()
        {
            var studentMembers  = new List<StudentMember>();

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0}, {1}, {2} " +
                                      "WHERE {0}.sail_club_member_id = {1}.sail_club_member_id " +
                                      "AND {1}.person_id = {2}.person_id",
                                      DatabaseManager.TableStudentMembers, DatabaseManager.TableSailClubMembers, 
                                      DatabaseManager.TablePersons);

                    SQLiteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                    studentMembers.
                        Add(
                            new StudentMember
                            {
                                StudentMemberId = reader.GetInt32(reader.GetOrdinal("student_member_id")),
                                SailClubMemberId = reader.GetInt32(reader.GetOrdinal("sail_club_member_id")),
                                PersonId = reader.GetInt32(reader.GetOrdinal("person_id")),
                                FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                                LastName = reader.GetString(reader.GetOrdinal("last_name")),
                                Address = reader.GetString(reader.GetOrdinal("address")),
                                Postcode = reader.GetString(reader.GetOrdinal("postcode")),
                                Cityname = reader.GetString(reader.GetOrdinal("cityname")),
                                BoatDriver = reader.GetBoolean(reader.GetOrdinal("boat_driver")),
                                DateOfBirth = reader.GetString(reader.GetOrdinal("date_of_birth")),
                                Gender = reader.GetFieldValue<Gender>(reader.GetOrdinal("gender")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("phone_number")),
                                Username = reader.GetString(reader.GetOrdinal("username")),
                                PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                                Position =
                                    reader.GetFieldValue<SailClubMember.Positions>(reader.GetOrdinal("position")),
                                Drabant = reader.GetBoolean(reader.GetOrdinal("drabant")),
                                Gaffelrigger = reader.GetBoolean(reader.GetOrdinal("gaffelrigger")),
                                Motor = reader.GetBoolean(reader.GetOrdinal("motor")),
                                Navigation = reader.GetBoolean(reader.GetOrdinal("navigation")),
                                Night = reader.GetBoolean(reader.GetOrdinal("night")),
                                RopeWorks = reader.GetBoolean(reader.GetOrdinal("rope_works"))
                            });
                    }
                }

                db.Close();
            }

            return studentMembers
            ;
        }

        public StudentMember GetOne(int itemId)
        {
            StudentMember studentMember = null;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0}, {1}, {2} " +
                                      "WHERE {0}.sail_club_member_id = {1}.sail_club_member_id " +
                                      "AND {1}.person_id = {2}.person_id " +
                                      "AND student_member_id = @studentMemberId " +
                                      "LIMIT 1",
                                      DatabaseManager.TableStudentMembers, DatabaseManager.TableSailClubMembers, 
                                      DatabaseManager.TablePersons);
                    command.Parameters.Add(new SQLiteParameter("@studentMemberId", itemId));

                    SQLiteDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        studentMember =
                            new StudentMember
                            {
                                StudentMemberId = reader.GetInt32(reader.GetOrdinal("student_member_id")),
                                SailClubMemberId = reader.GetInt32(reader.GetOrdinal("sail_club_member_id")),
                                PersonId = reader.GetInt32(reader.GetOrdinal("person_id")),
                                FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                                LastName = reader.GetString(reader.GetOrdinal("last_name")),
                                Address = reader.GetString(reader.GetOrdinal("address")),
                                Postcode = reader.GetString(reader.GetOrdinal("postcode")),
                                Cityname = reader.GetString(reader.GetOrdinal("cityname")),
                                BoatDriver = reader.GetBoolean(reader.GetOrdinal("boat_driver")),
                                DateOfBirth = reader.GetString(reader.GetOrdinal("date_of_birth")),
                                Gender = reader.GetFieldValue<Gender>(reader.GetOrdinal("gender")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("phone_number")),
                                Username = reader.GetString(reader.GetOrdinal("username")),
                                PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                                Position =
                                    reader.GetFieldValue<SailClubMember.Positions>(reader.GetOrdinal("position")),
                                Drabant = reader.GetBoolean(reader.GetOrdinal("drabant")),
                                Gaffelrigger = reader.GetBoolean(reader.GetOrdinal("gaffelrigger")),
                                Motor = reader.GetBoolean(reader.GetOrdinal("motor")),
                                Navigation = reader.GetBoolean(reader.GetOrdinal("navigation")),
                                Night = reader.GetBoolean(reader.GetOrdinal("night")),
                                RopeWorks = reader.GetBoolean(reader.GetOrdinal("rope_works"))
                            };
                    }
                }

                db.Close();
            }

            return studentMember;
        }

        public IEnumerable<StudentMember> GetAll(Func<StudentMember, bool> predicate)
        {
            return this.GetAll(predicate, true);
        }

        public IEnumerable<StudentMember> GetAll(Func<StudentMember, bool> predicate, bool fetchChildData)
        {
            IEnumerable<StudentMember> studentMembers  = this.GetAll().Where(predicate);

            if (fetchChildData)
            {
                foreach (StudentMember studentMember in studentMembers)
                {
                    // TODO Fill this out
                }
            }

            return studentMembers
            ;
        }
    }
}
