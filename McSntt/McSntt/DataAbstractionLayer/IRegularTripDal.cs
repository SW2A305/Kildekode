using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer
{
    public interface IRegularTripDal : IGenericDal<RegularTrip>
    {
        bool CanMakeReservation(Boat boat, DateTime departureTime, DateTime arrivalTime);
        bool CanMakeReservation(RegularTrip trip);
    }
}
