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
            var teamDal = DalLocator.TeamDal;
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

                        // Store team, if applicable
                        long teamId = 0;

                        if (studentMember.AssociatedTeam != null)
                        {
                            if (studentMember.AssociatedTeam.TeamId > 0) { teamDal.Update(studentMember.AssociatedTeam); }
                            else
                            { teamDal.Create(studentMember.AssociatedTeam); }

                            teamId = studentMember.AssociatedTeam.TeamId;
                        }

                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@sailClubMemberId", studentMember.SailClubMemberId));
                        command.Parameters.Add(new SQLiteParameter("@teamId", teamId));
                        command.Parameters.Add(new SQLiteParameter("@ropeWorks", studentMember.RopeWorks));
                        command.Parameters.Add(new SQLiteParameter("@navigation", studentMember.Navigation));
                        command.Parameters.Add(new SQLiteParameter("@motor", studentMember.Motor));
                        command.Parameters.Add(new SQLiteParameter("@drabant", studentMember.Drabant));
                        command.Parameters.Add(new SQLiteParameter("@gaffelrigger", studentMember.Gaffelrigger));
                        command.Parameters.Add(new SQLiteParameter("@night", studentMember.Night));
                        insertedRows += command.ExecuteNonQuery();

                        studentMember.StudentMemberId = db.LastInsertRowId;
                    }
                }

                db.Close();
            }

            return insertedRows == items.Length;
        }

        public bool Update(params StudentMember[] items)
        {
            var sailClubMemberDal = DalLocator.SailClubMemberDal;
            var teamDal = DalLocator.TeamDal;
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
                        // Update the 'SailClubMember' part first, only proceeding if we succeed
                        if (!sailClubMemberDal.Update(studentMember)) { continue; }

                        // Store team, if applicable
                        long teamId = 0;

                        if (studentMember.AssociatedTeam != null)
                        {
                            if (studentMember.AssociatedTeam.TeamId > 0) { teamDal.Update(studentMember.AssociatedTeam); }
                            else
                            { teamDal.Create(studentMember.AssociatedTeam); }

                            teamId = studentMember.AssociatedTeam.TeamId;
                        }

                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@studentMemberId", studentMember.StudentMemberId));
                        command.Parameters.Add(new SQLiteParameter("@teamId", teamId));
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
            var sailClubMemberDal = DalLocator.SailClubMemberDal;
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
                        // Delete the 'Person' part first, only proceeding if we succeed
                        if (!sailClubMemberDal.Delete(studentMember)) { continue; }

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

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            studentMembers.
                                Add(
                                    new StudentMember
                                    {
                                        StudentMemberId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("student_member_id")),
                                        SailClubMemberId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("sail_club_member_id")),
                                        PersonId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("person_id")),
                                        FirstName = DatabaseManager.ReadString(reader, reader.GetOrdinal("first_name")),
                                        LastName = DatabaseManager.ReadString(reader, reader.GetOrdinal("last_name")),
                                        Address = DatabaseManager.ReadString(reader, reader.GetOrdinal("address")),
                                        Postcode = DatabaseManager.ReadString(reader, reader.GetOrdinal("postcode")),
                                        Cityname = DatabaseManager.ReadString(reader, reader.GetOrdinal("cityname")),
                                        BoatDriver = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("boat_driver")),
                                        DateOfBirth = DatabaseManager.ReadString(reader, reader.GetOrdinal("date_of_birth")),
                                        Gender = (Gender) DatabaseManager.ReadInt(reader, reader.GetOrdinal("gender")),
                                        Email = DatabaseManager.ReadString(reader, reader.GetOrdinal("email")),
                                        PhoneNumber = DatabaseManager.ReadString(reader, reader.GetOrdinal("phone_number")),
                                        Username = DatabaseManager.ReadString(reader, reader.GetOrdinal("username")),
                                        PasswordHash = DatabaseManager.ReadString(reader, reader.GetOrdinal("password_hash")),
                                        Position = (SailClubMember.Positions) DatabaseManager.ReadInt(reader, reader.GetOrdinal("position")),
                                        Drabant = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("drabant")),
                                        Gaffelrigger = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("gaffelrigger")),
                                        Motor = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("motor")),
                                        Navigation = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("navigation")),
                                        Night = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("night")),
                                        RopeWorks = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("rope_works")),
                                        // TODO Fetch Team
                                    });
                        }
                    }
                }

                db.Close();
            }

            return studentMembers
            ;
        }

        public StudentMember GetOne(long itemId)
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

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            studentMember =
                                new StudentMember
                                {
                                    StudentMemberId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("student_member_id")),
                                    SailClubMemberId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("sail_club_member_id")),
                                    PersonId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("person_id")),
                                    FirstName = DatabaseManager.ReadString(reader, reader.GetOrdinal("first_name")),
                                    LastName = DatabaseManager.ReadString(reader, reader.GetOrdinal("last_name")),
                                    Address = DatabaseManager.ReadString(reader, reader.GetOrdinal("address")),
                                    Postcode = DatabaseManager.ReadString(reader, reader.GetOrdinal("postcode")),
                                    Cityname = DatabaseManager.ReadString(reader, reader.GetOrdinal("cityname")),
                                    BoatDriver = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("boat_driver")),
                                    DateOfBirth = DatabaseManager.ReadString(reader, reader.GetOrdinal("date_of_birth")),
                                    Gender = (Gender) DatabaseManager.ReadInt(reader, reader.GetOrdinal("gender")),
                                    Email = DatabaseManager.ReadString(reader, reader.GetOrdinal("email")),
                                    PhoneNumber = DatabaseManager.ReadString(reader, reader.GetOrdinal("phone_number")),
                                    Username = DatabaseManager.ReadString(reader, reader.GetOrdinal("username")),
                                    PasswordHash = DatabaseManager.ReadString(reader, reader.GetOrdinal("password_hash")),
                                    Position = (SailClubMember.Positions) DatabaseManager.ReadInt(reader, reader.GetOrdinal("position")),
                                    Drabant = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("drabant")),
                                    Gaffelrigger = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("gaffelrigger")),
                                    Motor = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("motor")),
                                    Navigation = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("navigation")),
                                    Night = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("night")),
                                    RopeWorks = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("rope_works"))
                                    // TODO Fetch Team
                                };
                        }
                    }
                }

                db.Close();
            }

            return studentMember;
        }

        public void LoadData(StudentMember item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StudentMember> GetAll(Func<StudentMember, bool> predicate)
        {
            IEnumerable<StudentMember> studentMembers  = this.GetAll().Where(predicate);

            return studentMembers;
        }
    }
}
