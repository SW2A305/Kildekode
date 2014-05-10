using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using McSntt.Helpers;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Sqlite
{
    public class EventSqliteDal : IEventDal
    {
        public bool Create(params Event[] items)
        {
            int insertedRows = 0;
            int eventRowsInserted = 0;
            int participantsRowsInserted = 0;
            int participantsRowsExpected = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand eventCommand = db.CreateCommand())
                {
                    using (SQLiteCommand participantsCommand = db.CreateCommand())
                    {
                        eventCommand.CommandType = CommandType.Text;
                        eventCommand.CommandText =
                            String.Format("INSERT INTO {0} (event_date, event_title, sign_up_req, description, " +
                                          "sign_up_msg, created) " +
                                          "VALUES (@eventDate, @eventTitle, @signUpReq, @description, @signUpMsg, " +
                                          "@created)",
                                          DatabaseManager.TableEvents);

                        participantsCommand.CommandType = CommandType.Text;
                        participantsCommand.CommandText =
                            String.Format("INSERT OR IGNORE INTO {0} (event_id, person_id) " +
                                          "VALUES (@eventId, @personId)",
                                          DatabaseManager.TableEventParticipantsBinder);

                        foreach (Event eventItem in items)
                        {
                            using (SQLiteTransaction transaction = db.BeginTransaction())
                            {
                                eventCommand.Parameters.Clear();
                                eventCommand.Parameters.Add(new SQLiteParameter("@eventDate", eventItem.EventDate));
                                eventCommand.Parameters.Add(new SQLiteParameter("@eventTitle", eventItem.EventTitle));
                                eventCommand.Parameters.Add(new SQLiteParameter("@signUpReq", eventItem.SignUpReq));
                                eventCommand.Parameters.Add(new SQLiteParameter("@description", eventItem.Description));
                                eventCommand.Parameters.Add(new SQLiteParameter("@signUpMsg", eventItem.SignUpMsg));
                                eventCommand.Parameters.Add(new SQLiteParameter("@created", eventItem.Created));
                                eventRowsInserted = eventCommand.ExecuteNonQuery();

                                eventItem.EventId = db.LastInsertRowId;

                                // Link to participants
                                participantsRowsExpected = 0;

                                if (eventItem.Participants != null)
                                {
                                    participantsRowsExpected = eventItem.Participants.Count;
                                    participantsRowsInserted = 0;

                                    foreach (Person participant in eventItem.Participants)
                                    {
                                        participantsCommand.Parameters.Clear();
                                        participantsCommand.Parameters.Add(new SQLiteParameter("@eventId",
                                                                                               eventItem.EventId));
                                        participantsCommand.Parameters.Add(new SQLiteParameter("@personId",
                                                                                               participant.PersonId));
                                        participantsRowsInserted += participantsCommand.ExecuteNonQuery();
                                    }
                                }

                                // Verify that everything was inserted correctly
                                if (participantsRowsInserted == participantsRowsExpected)
                                {
                                    transaction.Commit();
                                    insertedRows += eventRowsInserted;
                                }
                                else
                                { transaction.Rollback(); }
                            }
                        }
                    }
                }

                db.Close();
            }

            return insertedRows == items.Length;
        }

        public bool Update(params Event[] items)
        {
            int updatedRows = 0;
            int eventRowsUpdated = 0;
            int participantsRowsInserted = 0;
            int participantsRowsExpected = 0;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand eventCommand = db.CreateCommand())
                {
                    using (SQLiteCommand participantsCommand = db.CreateCommand())
                    {
                        eventCommand.CommandType = CommandType.Text;
                        eventCommand.CommandText =
                            String.Format("UPDATE {0} " +
                                          "SET event_date = @eventDate, event_title = @eventTitle, " +
                                          "sign_up_req = @signUpReq, description = @description, " +
                                          "sign_up_msg = @signUpMsg, created = @created " +
                                          "WHERE event_id = @eventId",
                                          DatabaseManager.TableEvents);

                        participantsCommand.CommandType = CommandType.Text;
                        participantsCommand.CommandText =
                            String.Format("INSERT OR IGNORE INTO {0} (event_id, person_id) " +
                                          "VALUES (@eventId, @personId)",
                                          DatabaseManager.TableEventParticipantsBinder);

                        foreach (Event eventItem in items)
                        {
                            using (SQLiteTransaction transaction = db.BeginTransaction())
                            {
                                eventCommand.Parameters.Clear();
                                eventCommand.Parameters.Add(new SQLiteParameter("@eventId", eventItem.EventId));
                                eventCommand.Parameters.Add(new SQLiteParameter("@eventDate", eventItem.EventDate));
                                eventCommand.Parameters.Add(new SQLiteParameter("@eventTitle", eventItem.EventTitle));
                                eventCommand.Parameters.Add(new SQLiteParameter("@signUpReq", eventItem.SignUpReq));
                                eventCommand.Parameters.Add(new SQLiteParameter("@description", eventItem.Description));
                                eventCommand.Parameters.Add(new SQLiteParameter("@signUpMsg", eventItem.SignUpMsg));
                                eventCommand.Parameters.Add(new SQLiteParameter("@created", eventItem.Created));
                                eventRowsUpdated = eventCommand.ExecuteNonQuery();

                                // Link to participants, removing existing ones first
                                using (SQLiteCommand deleteCommand = db.CreateCommand())
                                {
                                    deleteCommand.CommandType = CommandType.Text;
                                    deleteCommand.CommandText =
                                        String.Format("DELETE FROM {0} " +
                                                      "WHERE event_id = @eventId",
                                                      DatabaseManager.TableEventParticipantsBinder);
                                    deleteCommand.Parameters.Add(new SQLiteParameter("@eventId", eventItem.EventId));
                                    deleteCommand.ExecuteNonQuery();
                                }

                                participantsRowsExpected = 0;

                                if (eventItem.Participants != null)
                                {
                                    participantsRowsExpected = eventItem.Participants.Count;
                                    participantsRowsInserted = 0;

                                    foreach (Person participant in eventItem.Participants)
                                    {
                                        participantsCommand.Parameters.Clear();
                                        participantsCommand.Parameters.Add(new SQLiteParameter("@eventId",
                                                                                               eventItem.EventId));
                                        participantsCommand.Parameters.Add(new SQLiteParameter("@personId",
                                                                                               participant.PersonId));
                                        participantsRowsInserted += participantsCommand.ExecuteNonQuery();
                                    }
                                }

                                // Verify that everything was inserted correctly
                                if (participantsRowsInserted == participantsRowsExpected)
                                {
                                    transaction.Commit();
                                    updatedRows += eventRowsUpdated;
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

        public bool Delete(params Event[] items)
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
                                      "WHERE event_id = @eventId",
                                      DatabaseManager.TableEvents);

                    foreach (Event eventItem in items)
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SQLiteParameter("@eventId", eventItem.EventId));
                        deletedRows += command.ExecuteNonQuery();

                        // Remove participants
                        using (SQLiteCommand deleteCommand = db.CreateCommand())
                        {
                            deleteCommand.CommandType = CommandType.Text;
                            deleteCommand.CommandText =
                                String.Format("DELETE FROM {0} " +
                                              "WHERE event_id = @eventId",
                                              DatabaseManager.TableEventParticipantsBinder);
                            deleteCommand.Parameters.Add(new SQLiteParameter("@eventId", eventItem.EventId));
                            deleteCommand.ExecuteNonQuery();
                        }
                    }
                }

                db.Close();
            }

            return deletedRows == items.Length;
        }

        public IEnumerable<Event> GetAll()
        {
            var events = new List<Event>();

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0}", DatabaseManager.TableEvents);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events.
                                Add(
                                    new Event
                                    {
                                        EventId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("event_id")),
                                        EventDate = reader.GetDateTime(reader.GetOrdinal("event_date")),
                                        EventTitle =
                                            DatabaseManager.ReadString(reader, reader.GetOrdinal("event_title")),
                                        Created = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("created")),
                                        Description =
                                            DatabaseManager.ReadString(reader, reader.GetOrdinal("description")),
                                        SignUpMsg = DatabaseManager.ReadString(reader, reader.GetOrdinal("sign_up_msg")),
                                        SignUpReq =
                                            DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("sign_up_req"))
                                    });
                        }
                    }
                }

                db.Close();
            }

            return events
                ;
        }

        public Event GetOne(long itemId)
        {
            Event eventItem = null;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT * FROM {0} " +
                                      "WHERE event_id = @eventId " +
                                      "LIMIT 1",
                                      DatabaseManager.TableEvents);
                    command.Parameters.Add(new SQLiteParameter("@eventId", itemId));

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            eventItem =
                                new Event
                                {
                                    EventId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("event_id")),
                                    EventDate = reader.GetDateTime(reader.GetOrdinal("event_date")),
                                    EventTitle = DatabaseManager.ReadString(reader, reader.GetOrdinal("event_title")),
                                    Created = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("created")),
                                    Description = DatabaseManager.ReadString(reader, reader.GetOrdinal("description")),
                                    SignUpMsg = DatabaseManager.ReadString(reader, reader.GetOrdinal("sign_up_msg")),
                                    SignUpReq = DatabaseManager.ReadBoolean(reader, reader.GetOrdinal("sign_up_req"))
                                };
                        }
                    }
                }

                db.Close();
            }

            return eventItem;
        }

        public void LoadData(Event item)
        {
            // Load Participants
            IPersonDal personDal = DalLocator.PersonDal;

            using (SQLiteConnection db = DatabaseManager.DbConnection)
            {
                db.Open();

                using (SQLiteCommand command = db.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        String.Format("SELECT person_id FROM {0} WHERE event_id = @eventId",
                                      DatabaseManager.TableEventParticipantsBinder);
                    command.Parameters.Add(new SQLiteParameter("@eventId", item.EventId));

                    item.Participants = new List<Person>();
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long personId = DatabaseManager.ReadInt(reader, reader.GetOrdinal("person_id"));

                            if (personId > 0) { item.Participants.Add(personDal.GetOne(personId)); }
                        }
                    }
                }

                db.Close();
            }
        }

        public void LoadData(IEnumerable<Event> items) { foreach (Event item in items) { LoadData(item); } }

        public IEnumerable<Event> GetAll(Func<Event, bool> predicate)
        {
            var events = this.GetAll().ToArray();

            LoadData(events);

            return events.Where(predicate);
        }
    }
}
