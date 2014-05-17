using System;
using System.Collections.Generic;
using System.Linq;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Mock
{
    public class EventMockDal : IEventDal
    {
        private static Dictionary<long, Event> _events;

        public EventMockDal(bool useForTests = false)
        {
            if (useForTests || _events == null) { _events = new Dictionary<long, Event>(); }
        }

        public bool Create(params Event[] items)
        {
            foreach (Event @event in items)
            {
                @event.EventId = this.GetHighestId() + 1;
                _events.Add(@event.EventId, @event);
            }

            return true;
        }

        public bool CreateWithId(Event @event)
        {
            if (@event.EventId <= 0) { return false; }

            _events.Add(@event.EventId, @event);

            return true;
        }

        public bool Update(params Event[] items)
        {
            foreach (Event @event in items) {
                if (@event.EventId > 0 && _events.ContainsKey(@event.EventId)) { _events[@event.EventId] = @event; }
            }

            return true;
        }

        public bool Delete(params Event[] items)
        {
            foreach (Event @event in items) { if (@event.EventId > 0) { _events.Remove(@event.EventId); } }

            return true;
        }

        public IEnumerable<Event> GetAll() { return _events.Values; }

        public IEnumerable<Event> GetAll(Func<Event, bool> predicate) { return _events.Values.Where(predicate); }

        public Event GetOne(long itemId)
        {
            if (_events.ContainsKey(itemId)) { return _events[itemId]; }

            return null;
        }

        public void LoadData(Event item)
        {
            /* Not applicable */
        }

        public void LoadData(IEnumerable<Event> items)
        {
            /* Not applicable */
        }

        private long GetHighestId()
        {
            if (_events.Count == 0) { return 0; }

            return _events.Max(@event => @event.Key);
        }
    }
}
