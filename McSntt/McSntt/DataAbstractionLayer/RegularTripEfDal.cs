using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer
{
    public class RegularTripEfDal : IRegularTripDal
    {
        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Create(params RegularTrip[] items)
        {
            return !items.Any(regularTrip => regularTrip.RegularTripId > 0 && !this.CanMakeReservation(regularTrip))
                   && this.CreateOrUpdate(items);
        }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Update(params RegularTrip[] items)
        {
            return this.CreateOrUpdate(items);
        }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Delete(params RegularTrip[] items)
        {
            using (var db = new McSntttContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
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
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<RegularTrip> GetRegularTrips(Predicate<RegularTrip> predicate)
        {
            using (var db = new McSntttContext())
            {
                var query =
                    from trip in db.RegularTrips
                    where predicate(trip)
                    select trip;

                return query;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="boat"></param>
        /// <param name="onlyFuture"></param>
        /// <returns></returns>
        public IEnumerable<RegularTrip> GetReservationsForBoat(Boat boat, bool onlyFuture = true)
        {
            using (var db = new McSntttContext())
            {
                var query =
                    from trip in db.RegularTrips
                    where trip.Boat.BoatId == boat.BoatId
                    select trip;

                if (onlyFuture)
                {
                    query =
                        query.Where(trip => trip.DepartureTime > DateTime.Now || trip.ExpectedArrivalTime > DateTime.Now);
                }

                return query;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="boat"></param>
        /// <param name="fromDateTime"></param>
        /// <param name="toDateTime"></param>
        /// <returns></returns>
        public IEnumerable<RegularTrip> GetReservationsForBoat(Boat boat, DateTime fromDateTime, DateTime toDateTime)
        {
            return
                this.GetRegularTrips(
                                     trip =>
                                     trip.Boat.BoatId == boat.BoatId && trip.DepartureTime <= toDateTime
                                     && trip.ExpectedArrivalTime >= fromDateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="boat"></param>
        /// <param name="departureTime"></param>
        /// <param name="expectedArrivalTime"></param>
        /// <returns></returns>
        public bool CanMakeReservation(Boat boat, DateTime departureTime, DateTime expectedArrivalTime)
        {
            var reservations = GetReservationsForBoat(boat, departureTime, expectedArrivalTime);

            return !reservations.Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trip"></param>
        /// <returns></returns>
        public bool CanMakeReservation(RegularTrip trip)
        {
            return CanMakeReservation(trip.Boat, trip.DepartureTime, trip.ExpectedArrivalTime);
        }

        /// <summary>
        /// </summary>
        /// <param name="regularTrips"></param>
        /// <returns></returns>
        private bool CreateOrUpdate(params RegularTrip[] regularTrips)
        {
            using (var db = new McSntttContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
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
