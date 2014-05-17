using System;
using System.Collections.Generic;
using System.Linq;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Mock
{
    public class BoatMockDal : IBoatDal
    {
        private static Dictionary<long, Boat> _boats;

        public BoatMockDal(bool useForTests = false)
        {
            if (useForTests || _boats == null) { _boats = new Dictionary<long, Boat>(); }
        }

        public bool Create(params Boat[] items)
        {
            foreach (Boat boat in items)
            {
                boat.BoatId = this.GetHighestId() + 1;
                _boats.Add(boat.BoatId, boat);
            }

            return true;
        }

        public bool CreateWithId(Boat boat)
        {
            if (boat.BoatId <= 0) { return false; }

            _boats.Add(boat.BoatId, boat);

            return true;
        }

        public bool Update(params Boat[] items)
        {
            foreach (Boat boat in items) {
                if (boat.BoatId > 0 && _boats.ContainsKey(boat.BoatId)) { _boats[boat.BoatId] = boat; }
            }

            return true;
        }

        public bool Delete(params Boat[] items)
        {
            foreach (Boat boat in items) { if (boat.BoatId > 0) { _boats.Remove(boat.BoatId); } }

            return true;
        }

        public IEnumerable<Boat> GetAll() { return _boats.Values; }

        public IEnumerable<Boat> GetAll(Func<Boat, bool> predicate) { return _boats.Values.Where(predicate); }

        public Boat GetOne(long itemId)
        {
            if (_boats.ContainsKey(itemId)) { return _boats[itemId]; }

            return null;
        }

        public void LoadData(Boat item)
        {
            /* Not applicable */
        }

        public void LoadData(IEnumerable<Boat> items)
        {
            /* Not applicable */
        }

        private long GetHighestId()
        {
            if (_boats.Count == 0) { return 0; }

            return _boats.Max(boat => boat.Key);
        }
    }
}
