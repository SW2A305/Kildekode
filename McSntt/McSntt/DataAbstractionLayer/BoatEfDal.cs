using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer
{
    public class BoatEfDal : IBoatDal
    {
        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Create(params Boat[] items)
        {
            return this.CreateOrUpdate(items);
        }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Update(params Boat[] items)
        {
            return this.CreateOrUpdate(items);
        }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Delete(params Boat[] items)
        {
            using (var db = new McSntttContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (Boat item in items)
                        {
                            db.Boats.Remove(item);
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
        public IEnumerable<Boat> GetAll()
        {
            using (var db = new McSntttContext())
            {
                db.Boats.Load();
                return db.Boats.Include("SailTrips").ToList();
            }
        }

        public Boat GetOne(int itemId)
        {
            using (var db = new McSntttContext())
            {
                return db.Boats.Find(itemId);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private bool CreateOrUpdate(params Boat[] items)
        {
            using (var db = new McSntttContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (Boat item in items)
                        {
                            db.Boats.AddOrUpdate(b => b.BoatId, item);
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
