using System;
using System.Collections.Generic;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer
{
    public interface IRegularTripDal : IGenericDal<RegularTrip>
    {
        IEnumerable<RegularTrip> GetRegularTrips(Predicate<RegularTrip> predicate);
        IEnumerable<RegularTrip> GetReservationsForBoat(Boat boat, bool onlyFuture = true);
        IEnumerable<RegularTrip> GetReservationsForBoat(Boat boat, DateTime? fromDateTime, DateTime? toDateTime);
        bool CanMakeReservation(Boat boat, DateTime? departureTime, DateTime? expectedArrivalTime);
        bool CanMakeReservation(RegularTrip trip);
    }
}
