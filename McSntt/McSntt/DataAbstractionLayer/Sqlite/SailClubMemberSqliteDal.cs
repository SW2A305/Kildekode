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
                        command.Parameters.Add(new SQLiteParameter("@position", (int)sailClubMember.Position));
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
                        command.Parameters.Add(new SQLiteParameter("@position", (int)sailClubMember.Position));
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

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sailClubMembers.
                                Add(
                                    new SailClubMember
                                    {
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
                                        Position = (SailClubMember.Positions) DatabaseManager.ReadInt(reader, reader.GetOrdinal("position"))
                                    });
                        }
                    }
                }

                db.Close();
            }

            return sailClubMembers;
        }

        public SailClubMember GetOne(long itemId)
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
                                      DatabaseManager.TableSailClubMembers,
                                      DatabaseManager.TablePersons);
                    command.Parameters.Add(new SQLiteParameter("@sailClubMemberId", itemId));

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            sailClubMember =
                                new SailClubMember
                                {
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
                                    Position = (SailClubMember.Positions) DatabaseManager.ReadInt(reader, reader.GetOrdinal("position"))
                                };
                        }
                    }
                }

                db.Close();
            }

            return sailClubMember;
        }

        public void LoadData(SailClubMember item) {}

        public void LoadData(IEnumerable<SailClubMember> items) {}

        public IEnumerable<SailClubMember> GetAll(Func<SailClubMember, bool> predicate)
        {
            var sailClubMembers = this.GetAll().ToArray();

            LoadData(sailClubMembers);

            return sailClubMembers.Where(predicate);
        }
    }
}
