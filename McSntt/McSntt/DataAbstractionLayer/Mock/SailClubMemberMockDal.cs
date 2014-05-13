using System;
using System.Collections.Generic;
using System.Linq;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Mock
{
    public class SailClubMemberMockDal : ISailClubMemberDal
    {
        private static Dictionary<long, SailClubMember> _sailClubMembers;

        public SailClubMemberMockDal(bool useForTests = false)
        {
            if (useForTests || _sailClubMembers == null) {
                _sailClubMembers = new Dictionary<long, SailClubMember>();
            }
        }

        public bool Create(params SailClubMember[] items)
        {
            var personDal = new PersonMockDal();

            foreach (SailClubMember sailClubMember in items)
            {
                personDal.Create(sailClubMember);
                sailClubMember.SailClubMemberId = this.GetHighestId() + 1;
                _sailClubMembers.Add(sailClubMember.SailClubMemberId, sailClubMember);
            }

            return true;
        }

        public bool Update(params SailClubMember[] items)
        {
            var personDal = new PersonMockDal();

            foreach (SailClubMember sailClubMember in items)
            {
                if (sailClubMember.SailClubMemberId > 0 && _sailClubMembers.ContainsKey(sailClubMember.SailClubMemberId))
                {
                    personDal.Update(sailClubMember);
                    _sailClubMembers[sailClubMember.SailClubMemberId] = sailClubMember;
                }
            }

            return true;
        }

        public bool Delete(params SailClubMember[] items)
        {
            var personDal = new PersonMockDal();
            var studentDal = new StudentMemberMockDal();

            foreach (SailClubMember sailClubMember in items)
            {
                var student = sailClubMember as StudentMember;
                if (student != null) { studentDal.Delete(student); continue; }

                if (sailClubMember.SailClubMemberId > 0 && _sailClubMembers.ContainsKey(sailClubMember.SailClubMemberId))
                {
                    _sailClubMembers.Remove(sailClubMember.SailClubMemberId);
                    personDal.DeleteWithoutCheck(sailClubMember);
                }
            }

            return true;
        }

        public bool DeleteWithoutCheck(SailClubMember sailClubMember)
        {
            var personDal = new PersonMockDal();

            if (sailClubMember.SailClubMemberId > 0 && _sailClubMembers.ContainsKey(sailClubMember.SailClubMemberId))
            {
                _sailClubMembers.Remove(sailClubMember.SailClubMemberId);
                personDal.Delete(sailClubMember);
            }

            return true;
        }

        public IEnumerable<SailClubMember> GetAll() { return _sailClubMembers.Values; }

        public IEnumerable<SailClubMember> GetAll(Func<SailClubMember, bool> predicate)
        {
            return _sailClubMembers.Values.Where(predicate);
        }

        public SailClubMember GetOne(long itemId)
        {
            if (_sailClubMembers.ContainsKey(itemId)) { return _sailClubMembers[itemId]; }

            return null;
        }

        public void LoadData(SailClubMember item)
        {
            /* Not applicable */
        }

        public void LoadData(IEnumerable<SailClubMember> items)
        {
            /* Not applicable */
        }

        private long GetHighestId()
        {
            if (_sailClubMembers.Count == 0) { return 0; }

            return _sailClubMembers.Max(sailClubMember => sailClubMember.Key);
        }
    }
}
