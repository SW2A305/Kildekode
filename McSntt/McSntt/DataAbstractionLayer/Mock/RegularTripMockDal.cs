using System;
using System.Collections.Generic;
using System.Linq;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Mock
{
    public class RegularTripMockDal : IRegularTripDal
    {
        private static Dictionary<long, RegularTrip> _regularTrips;

        public RegularTripMockDal(bool useForTests = false)
        {
            if (useForTests || _regularTrips == null) { _regularTrips = new Dictionary<long, RegularTrip>(); }
        }

        public bool Create(params RegularTrip[] items)
        {
            foreach (RegularTrip regularTrip in items)
            {
                regularTrip.RegularTripId = this.GetHighestId() + 1;
                _regularTrips.Add(regularTrip.RegularTripId, regularTrip);
            }

            return true;
        }

        public bool CreateWithId(RegularTrip regularTrip)
        {
            if (regularTrip.RegularTripId <= 0) { return false; }

            _regularTrips.Add(regularTrip.RegularTripId, regularTrip);

            return true;
        }

        public bool Update(params RegularTrip[] items)
        {
            foreach (RegularTrip regularTrip in items)
            {
                if (regularTrip.RegularTripId > 0 && _regularTrips.ContainsKey(regularTrip.RegularTripId)) {
                    _regularTrips[regularTrip.RegularTripId] = regularTrip;
                }
            }

            return true;
        }

        public bool Delete(params RegularTrip[] items)
        {
            foreach (RegularTrip regularTrip in items) {
                if (regularTrip.RegularTripId > 0) { _regularTrips.Remove(regularTrip.RegularTripId); }
            }

            return true;
        }

        public IEnumerable<RegularTrip> GetAll() { return _regularTrips.Values; }

        public IEnumerable<RegularTrip> GetAll(Func<RegularTrip, bool> predicate)
        {
            return _regularTrips.Values.Where(predicate);
        }

        public RegularTrip GetOne(long itemId)
        {
            if (_regularTrips.ContainsKey(itemId)) { return _regularTrips[itemId]; }

            return null;
        }

        public void LoadData(RegularTrip item)
        {
            /* Not applicable */
        }

        public void LoadData(IEnumerable<RegularTrip> items)
        {
            /* Not applicable */
        }

        private long GetHighestId()
        {
            if (_regularTrips.Count == 0) { return 0; }

            return _regularTrips.Max(regularTrip => regularTrip.Key);
        }
    }
}
