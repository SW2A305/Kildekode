using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer
{
    public class PersonEfDal : IPersonDal
    {
        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Create(params Person[] items)
        {
            return this.CreateOrUpdate(items);
        }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Update(params Person[] items)
        {
            return this.CreateOrUpdate(items);
        }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Delete(params Person[] items)
        {
            using (var db = new McSntttContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (Person item in items)
                        {
                            db.Persons.Remove(item);
                        }

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Person> GetAll()
        {
            using (var db = new McSntttContext())
            {
                return
                    db.Persons.Include("PartOfCrewOn")
                      .Include("CaptainOn")
                      .Include("ParticipatingInEvents")
                      .Include("PartOfActualCrewOn")
                      .ToList();
            }
        }

        public Person GetOne(int itemId)
        {
            using (var db = new McSntttContext())
            {
                return db.Persons.Find(itemId);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private bool CreateOrUpdate(params Person[] items)
        {
            using (var db = new McSntttContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (Person item in items)
                        {
                            db.Persons.AddOrUpdate(p => p.PersonId, item);
                        }

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
