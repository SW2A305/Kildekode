using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
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
            return !items.Any(regularTrip => regularTrip.RegularTripId > 0 && !CanMakeReservation(regularTrip))
                   && CreateOrUpdate(items);
        }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Update(params RegularTrip[] items)
        {
            return CreateOrUpdate(items);
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

                return
                    db.RegularTrips
                        .Include("Boat")
                        .Include("Logbook")
                        .Include("Captain")
                        .Include("Crew")
                        .ToList();
            }
        }

        public RegularTrip GetOne(int itemId)
        {
            using (var db = new McSntttContext())
            {
                return
                    db.RegularTrips
                        .Include("Boat")
                        .Include("Logbook")
                        .Include("Captain")
                        .Include("Crew")
                        .FirstOrDefault(trip => trip.RegularTripId == itemId);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<RegularTrip> GetRegularTrips(Func<RegularTrip, bool> predicate)
        {
            using (var db = new McSntttContext())
            {
                var trips = db.RegularTrips.Include("Boat")
                    .Include("Logbook")
                    .Include("Captain")
                    .Include("Crew")
                    .AsEnumerable();

                return
                    trips.Where(predicate).ToList();
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
                IQueryable<RegularTrip> query =
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
        public IEnumerable<RegularTrip> GetReservationsForBoat(Boat boat, DateTime? fromDateTime, DateTime? toDateTime)
        {
            return
                GetRegularTrips(
                    trip =>
                        trip.Boat.BoatId == boat.BoatId && trip.DepartureTime <= toDateTime
                        && trip.ExpectedArrivalTime >= fromDateTime);
        }

        /// <summary>
        /// </summary>
        /// <param name="boat"></param>
        /// <param name="departureTime"></param>
        /// <param name="expectedArrivalTime"></param>
        /// <returns></returns>
        public bool CanMakeReservation(Boat boat, DateTime? departureTime, DateTime? expectedArrivalTime)
        {
            IEnumerable<RegularTrip> reservations = GetReservationsForBoat(boat, departureTime, expectedArrivalTime);

            return !reservations.Any();
        }

        /// <summary>
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