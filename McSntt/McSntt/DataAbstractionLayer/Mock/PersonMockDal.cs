using System;
using System.Collections.Generic;
using System.Linq;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Mock
{
    public class PersonMockDal : IPersonDal
    {
        private static Dictionary<long, Person> _persons;

        public PersonMockDal(bool useForTests = false)
        {
            if (useForTests || _persons == null) { _persons = new Dictionary<long, Person>(); }
        }

        public bool Create(params Person[] items)
        {
            foreach (Person person in items)
            {
                person.PersonId = this.GetHighestId() + 1;
                _persons.Add(person.PersonId, person);
            }

            return true;
        }

        public bool CreateWithId(Person person)
        {
            if (person.PersonId <= 0) { return false; }

            _persons.Add(person.PersonId, person);

            return true;
        }

        public bool Update(params Person[] items)
        {
            foreach (Person person in items) {
                if (person.PersonId > 0 && _persons.ContainsKey(person.PersonId)) { _persons[person.PersonId] = person; }
            }

            return true;
        }

        public bool Delete(params Person[] items)
        {
            var memberDal = new SailClubMemberMockDal();

            foreach (Person person in items)
            {
                var member = person as StudentMember;
                if (member != null) { memberDal.Delete(member); continue; }

                if (person.PersonId > 0) { _persons.Remove(person.PersonId); }
            }

            return true;
        }

        public bool DeleteWithoutCheck(Person person)
        {
            if (person.PersonId > 0) { _persons.Remove(person.PersonId); }

            return true;
        }

        public IEnumerable<Person> GetAll() { return _persons.Values; }

        public IEnumerable<Person> GetAll(Func<Person, bool> predicate) { return _persons.Values.Where(predicate); }

        public Person GetOne(long itemId)
        {
            if (_persons.ContainsKey(itemId)) { return _persons[itemId]; }

            return null;
        }

        public void LoadData(Person item)
        {
            /* Not applicable */
        }

        public void LoadData(IEnumerable<Person> items)
        {
            /* Not applicable */
        }

        private long GetHighestId()
        {
            if (_persons.Count == 0) { return 0; }

            return _persons.Max(person => person.Key);
        }
    }
}
