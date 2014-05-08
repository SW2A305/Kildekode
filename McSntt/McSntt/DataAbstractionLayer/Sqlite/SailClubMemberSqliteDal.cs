using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Sqlite
{
    public class SailClubMemberSqliteDal : ISailClubMemberDal
    {
        public bool Create(params SailClubMember[] items)
        {
            IPersonDal personDal = DalLocator.PersonDal;
            int insertedRows = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("INSERT INTO {0} (person_id, position, username, password_hash) " +
                                      "VALUES (@personId, @position, @username, @passwordHash)",
                                      DatabaseManager.TableSailClubMembers);

                    foreach (SailClubMember sailClubMember in items)
                    {
                        // Store the 'Person' part first, only proceeding if we succeed
                        if (!personDal.Create(sailClubMember)) { continue; }

                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@personId", sailClubMember.PersonId));
                        command.Parameters.Add(new SQLiteParameter("@position", sailClubMember.Position));
                        command.Parameters.Add(new SQLiteParameter("@username", sailClubMember.Username));
                        command.Parameters.Add(new SQLiteParameter("@passwordHash", sailClubMember.PasswordHash));
                        insertedRows += command.ExecuteNonQuery();

                        sailClubMember.SailClubMemberId = db.LastInsertRowId;
                    }
                }

                db.Close();
            }

            return insertedRows == items.Length;
        }

        public bool Update(params SailClubMember[] items)
        {
            IPersonDal personDal = DalLocator.PersonDal;
            int updatedRows = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("UPDATE {0} " +
                                      "SET person_id = @personId, position = @position, username = @username, " +
                                      "password_hash = @passwordHash " +
                                      "WHERE sail_club_member_id = @sailClubMemberId",
                                      DatabaseManager.TableSailClubMembers);

                    foreach (SailClubMember sailClubMember in items)
                    {
                        // Update the 'Person' part first, only proceeding if we succeed
                        if (!personDal.Update(sailClubMember)) { continue; }

                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@sailClubMemberId", sailClubMember.SailClubMemberId));
                        command.Parameters.Add(new SQLiteParameter("@personId", sailClubMember.PersonId));
                        command.Parameters.Add(new SQLiteParameter("@position", sailClubMember.Position));
                        command.Parameters.Add(new SQLiteParameter("@username", sailClubMember.Username));
                        command.Parameters.Add(new SQLiteParameter("@passwordHash", sailClubMember.PasswordHash));
                        updatedRows += command.ExecuteNonQuery();
                    }
                }

                db.Close();
            }

            return updatedRows == items.Length;
        }

        public bool Delete(params SailClubMember[] items)
        {
            IPersonDal personDal = DalLocator.PersonDal;
            int deletedRows = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("DELETE FROM {0} " +
                                      "WHERE sail_club_member_id = @sailClubMemberId",
                                      DatabaseManager.TableSailClubMembers);

                    foreach (SailClubMember sailClubMember in items)
                    {
                        // Delete the 'Person' part first, only proceeding if we succeed
                        if (!personDal.Delete(sailClubMember)) { continue; }

                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@sailClubMemberId", sailClubMember.SailClubMemberId));
                        deletedRows += command.ExecuteNonQuery();
                    }
                }

                db.Close();
            }

            return deletedRows == items.Length;
        }

        public IEnumerable<SailClubMember> GetAll()
        {
            var sailClubMembers = new List<SailClubMember>();

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0}, {1} WHERE {0}.person_id = {1}.person_id",
                                      DatabaseManager.TableSailClubMembers, DatabaseManager.TablePersons);

                    SQLiteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        sailClubMembers.
                            Add(
                                new SailClubMember
                                {
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
                                        reader.GetFieldValue<SailClubMember.Positions>(reader.GetOrdinal("position"))
                                });
                    }
                }

                db.Close();
            }

            return sailClubMembers;
        }

        public SailClubMember GetOne(int itemId)
        {
            SailClubMember sailClubMember = null;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0}, {1} " +
                                      "WHERE sail_club_member_id = @sailClubMemberId " +
                                      "AND {0}.person_id = {1}.person_id " +
                                      "LIMIT 1",
                                      DatabaseManager.TableSailClubMembers);
                    command.Parameters.Add(new SQLiteParameter("@sailClubMemberId", itemId));

                    SQLiteDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        sailClubMember =
                            new SailClubMember
                            {
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
                                    reader.GetFieldValue<SailClubMember.Positions>(reader.GetOrdinal("position"))
                            };
                    }
                }

                db.Close();
            }

            return sailClubMember;
        }

        public IEnumerable<SailClubMember> GetAll(Func<SailClubMember, bool> predicate)
        {
            return this.GetAll(predicate, true);
        }

        public IEnumerable<SailClubMember> GetAll(Func<SailClubMember, bool> predicate, bool fetchChildData)
        {
            IEnumerable<SailClubMember> sailClubMembers = this.GetAll().Where(predicate);

            if (fetchChildData)
            {
                foreach (SailClubMember sailClubMember in sailClubMembers)
                {
                    // TODO Fill this out
                }
            }

            return sailClubMembers
                ;
        }
    }
}
