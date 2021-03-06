﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Sqlite
{
    public class PersonSqliteDal : IPersonDal
    {
        #region IPersonDal Members
        public bool Create(params Person[] items)
        {
            int insertedRows = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("INSERT INTO {0} (first_name, last_name, address, postcode, cityname, " +
                                      "date_of_birth, boat_driver, gender, phone_number, email) " +
                                      "VALUES (@firstName, @lastName, @address, @postcode, @cityname, @dateOfBirth, " +
                                      "@boatDriver, @gender, @phoneNumber, @email)",
                                      DatabaseManager.TablePersons);

                    foreach (Person person in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@firstName", person.FirstName));
                        command.Parameters.Add(new SQLiteParameter("@lastName", person.LastName));
                        command.Parameters.Add(new SQLiteParameter("@address", person.Address));
                        command.Parameters.Add(new SQLiteParameter("@postcode", person.Postcode));
                        command.Parameters.Add(new SQLiteParameter("@cityname", person.Cityname));
                        command.Parameters.Add(new SQLiteParameter("@dateOfBirth", person.DateOfBirth));
                        command.Parameters.Add(new SQLiteParameter("@boatDriver", person.BoatDriver));
                        command.Parameters.Add(new SQLiteParameter("@gender", (int) person.Gender));
                        command.Parameters.Add(new SQLiteParameter("@phoneNumber", person.PhoneNumber));
                        command.Parameters.Add(new SQLiteParameter("@email", person.Email));
                        insertedRows += command.ExecuteNonQuery();

                        person.PersonId = db.LastInsertRowId;
                    }
                }

                db.Close();
            }

            return insertedRows == items.Length;
        }

        public bool Update(params Person[] items)
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
                                      "SET first_name = @firstName, last_name = @lastName, address = @address, " +
                                      "postcode = @postcode, cityname = @cityname, date_of_birth = @dateOfBirth, " +
                                      "boat_driver = @boatDriver, gender = @gender, phone_number = @phoneNumber, " +
                                      "email = @email " +
                                      "WHERE person_id = @personId",
                                      DatabaseManager.TablePersons);

                    foreach (Person person in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@personId", person.PersonId));
                        command.Parameters.Add(new SQLiteParameter("@firstName", person.FirstName));
                        command.Parameters.Add(new SQLiteParameter("@lastName", person.LastName));
                        command.Parameters.Add(new SQLiteParameter("@address", person.Address));
                        command.Parameters.Add(new SQLiteParameter("@postcode", person.Postcode));
                        command.Parameters.Add(new SQLiteParameter("@cityname", person.Cityname));
                        command.Parameters.Add(new SQLiteParameter("@dateOfBirth", person.DateOfBirth));
                        command.Parameters.Add(new SQLiteParameter("@boatDriver", person.BoatDriver));
                        command.Parameters.Add(new SQLiteParameter("@gender", (int) person.Gender));
                        command.Parameters.Add(new SQLiteParameter("@phoneNumber", person.PhoneNumber));
                        command.Parameters.Add(new SQLiteParameter("@email", person.Email));
                        updatedRows += command.ExecuteNonQuery();
                    }
                }

                db.Close();
            }

            return updatedRows == items.Length;
        }

        public bool Delete(params Person[] items)
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
                                      "WHERE person_id = @personId",
                                      DatabaseManager.TablePersons);

                    foreach (Person person in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@personId", person.PersonId));
                        deletedRows += command.ExecuteNonQuery();
                    }
                }

                db.Close();
            }

            return deletedRows == items.Length;
        }

        public IEnumerable<Person> GetAll()
        {
            var persons = new List<Person>();

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0}", DatabaseManager.TablePersons);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            persons.
                                Add(
                                    new Person
                                    {
                                        PersonId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("person_id")),
                                        FirstName = DatabaseManager.ReadString(reader, reader.GetOrdinal("first_name")),
                                        LastName = DatabaseManager.ReadString(reader, reader.GetOrdinal("last_name")),
                                        Address = DatabaseManager.ReadString(reader, reader.GetOrdinal("address")),
                                        Postcode = DatabaseManager.ReadString(reader, reader.GetOrdinal("postcode")),
                                        Cityname = DatabaseManager.ReadString(reader, reader.GetOrdinal("cityname")),
                                        BoatDriver =
                                            DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("boat_driver")),
                                        DateOfBirth =
                                            DatabaseManager.ReadString(reader, reader.GetOrdinal("date_of_birth")),
                                        Gender = (Gender) DatabaseManager.ReadInt(reader, reader.GetOrdinal("gender")),
                                        Email = DatabaseManager.ReadString(reader, reader.GetOrdinal("email")),
                                        PhoneNumber =
                                            DatabaseManager.ReadString(reader, reader.GetOrdinal("phone_number"))
                                    });
                        }
                    }
                }

                db.Close();
            }

            return persons
                ;
        }

        public Person GetOne(long itemId)
        {
            Person person = null;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0} " +
                                      "WHERE person_id = @personId " +
                                      "LIMIT 1",
                                      DatabaseManager.TablePersons);
                    command.Parameters.Add(new SQLiteParameter("@personId", itemId));

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            person =
                                new Person
                                {
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
                                    PhoneNumber = DatabaseManager.ReadString(reader, reader.GetOrdinal("phone_number"))
                                };
                        }
                    }
                }

                db.Close();
            }

            return person;
        }

        public void LoadData(Person item) { }

        public void LoadData(IEnumerable<Person> items) { }

        public IEnumerable<Person> GetAll(Func<Person, bool> predicate)
        {
            Person[] persons = this.GetAll().ToArray();

            LoadData(persons);

            return persons.Where(predicate);
        }
        #endregion
    }
}
