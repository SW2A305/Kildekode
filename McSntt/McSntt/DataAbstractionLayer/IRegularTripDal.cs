using System.Collections.Generic;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer
{
    public interface IRegularTripDal : IGenericDal<RegularTrip>
    {
        IEnumerable<RegularTrip> GetRegularTrips();
    }
}
