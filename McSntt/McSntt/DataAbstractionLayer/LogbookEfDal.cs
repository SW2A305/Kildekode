using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer
{
    public class LogbookEfDal : ILogbookDal
    {
        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Create(params Logbook[] items)
        {
            return this.CreateOrUpdate(items);
        }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Update(params Logbook[] items)
        {
            return this.CreateOrUpdate(items);
        }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Delete(params Logbook[] items)
        {
            using (var db = new McSntttContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (Logbook item in items)
                        {
                            db.Logbooks.Remove(item);
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
        public IEnumerable<Logbook> GetAll()
        {
            using (var db = new McSntttContext())
            {
                return db.Logbooks.Local;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private bool CreateOrUpdate(params Logbook[] items)
        {
            using (var db = new McSntttContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (Logbook item in items)
                        {
                            db.Logbooks.AddOrUpdate(b => b.LogbookId, item);
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
