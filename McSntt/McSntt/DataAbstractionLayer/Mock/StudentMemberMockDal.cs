using System;
using System.Collections.Generic;
using System.Linq;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Mock
{
    public class StudentMemberMockDal : IStudentMemberDal
    {
        private static Dictionary<long, StudentMember> _studentMembers;

        public StudentMemberMockDal(bool useForTests = false)
        {
            if (useForTests || _studentMembers == null)
            {
                _studentMembers = new Dictionary<long, StudentMember>();
            }
        }

        public bool Create(params StudentMember[] items)
        {
            var memberDal = new SailClubMemberMockDal();

            foreach (StudentMember studentMember in items)
            {
                memberDal.Create(studentMember);
                studentMember.StudentMemberId = this.GetHighestId() + 1;
                _studentMembers.Add(studentMember.StudentMemberId, studentMember);
            }

            return true;
        }

        public bool Update(params StudentMember[] items)
        {
            var memberDal = new SailClubMemberMockDal();

            foreach (StudentMember studentMember in items)
            {
                if (studentMember.StudentMemberId > 0 && _studentMembers.ContainsKey(studentMember.StudentMemberId))
                {
                    memberDal.Update(studentMember);
                    _studentMembers[studentMember.StudentMemberId] = studentMember;
                }
            }

            return true;
        }

        public bool Delete(params StudentMember[] items)
        {
            var memberDal = new SailClubMemberMockDal();

            foreach (StudentMember studentMember in items) {
                if (studentMember.StudentMemberId > 0 && _studentMembers.ContainsKey(studentMember.StudentMemberId))
                {
                    _studentMembers.Remove(studentMember.StudentMemberId);
                    memberDal.DeleteWithoutCheck(studentMember);
                }
            }

            return true;
        }

        public IEnumerable<StudentMember> GetAll() { return _studentMembers.Values; }

        public IEnumerable<StudentMember> GetAll(Func<StudentMember, bool> predicate)
        {
            return _studentMembers.Values.Where(predicate);
        }

        public StudentMember GetOne(long itemId)
        {
            if (_studentMembers.ContainsKey(itemId)) { return _studentMembers[itemId]; }

            return null;
        }

        public void LoadData(StudentMember item)
        {
            /* Not applicable */
        }

        public void LoadData(IEnumerable<StudentMember> items)
        {
            /* Not applicable */
        }

        private long GetHighestId()
        {
            if (_studentMembers.Count == 0) { return 0; }

            return _studentMembers.Max(studentMember => studentMember.Key);
        }
    }
}
