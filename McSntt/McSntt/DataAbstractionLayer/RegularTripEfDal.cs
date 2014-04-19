using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer
{
    public class RegularTripEfDal : IRegularTripDal
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Create(params RegularTrip[] items)
        {
            return this.CreateOrUpdate(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Update(params RegularTrip[] items)
        {
            return this.CreateOrUpdate(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Delete(params RegularTrip[] items)
        {
            using (var db = new McSntttContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.RegularTrips.RemoveRange(items);

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
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RegularTrip> GetAll()
        {
            using (var db = new McSntttContext())
            {
                db.RegularTrips.Load();

                return db.RegularTrips.Local;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regularTrips"></param>
        /// <returns></returns>
        private bool CreateOrUpdate(params RegularTrip[] regularTrips)
        {
            using (var db = new McSntttContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.RegularTrips.AddOrUpdate(regularTrips);

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