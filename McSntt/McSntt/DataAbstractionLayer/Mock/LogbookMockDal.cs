using System;
using System.Collections.Generic;
using System.Linq;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Mock
{
    public class LogbookMockDal : ILogbookDal
    {
        private static Dictionary<long, Logbook> _logbooks;

        public LogbookMockDal(bool useForTests = false)
        {
            if (useForTests || _logbooks == null) { _logbooks = new Dictionary<long, Logbook>(); }
        }

        public bool Create(params Logbook[] items)
        {
            foreach (Logbook logbook in items)
            {
                logbook.LogbookId = this.GetHighestId() + 1;
                _logbooks.Add(logbook.LogbookId, logbook);
            }

            return true;
        }

        public bool Update(params Logbook[] items)
        {
            foreach (Logbook logbook in items)
            {
                if (logbook.LogbookId > 0 && _logbooks.ContainsKey(logbook.LogbookId)) {
                    _logbooks[logbook.LogbookId] = logbook;
                }
            }

            return true;
        }

        public bool Delete(params Logbook[] items)
        {
            foreach (Logbook logbook in items) { if (logbook.LogbookId > 0) { _logbooks.Remove(logbook.LogbookId); } }

            return true;
        }

        public IEnumerable<Logbook> GetAll() { return _logbooks.Values; }

        public IEnumerable<Logbook> GetAll(Func<Logbook, bool> predicate) { return _logbooks.Values.Where(predicate); }

        public Logbook GetOne(long itemId)
        {
            if (_logbooks.ContainsKey(itemId)) { return _logbooks[itemId]; }

            return null;
        }

        public void LoadData(Logbook item)
        {
            /* Not applicable */
        }

        public void LoadData(IEnumerable<Logbook> items)
        {
            /* Not applicable */
        }

        private long GetHighestId()
        {
            if (_logbooks.Count == 0) { return 0; }

            return _logbooks.Max(logbook => logbook.Key);
        }
    }
}
