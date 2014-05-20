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
            if (useForTests || _studentMembers == null) { _studentMembers = new Dictionary<long, StudentMember>(); }
        }

        #region IStudentMemberDal Members
        public bool Create(params StudentMember[] items)
        {
            var memberDal = new SailClubMemberMockDal();

            foreach (StudentMember studentMember in items)
            {
                if (studentMember.SailClubMemberId <= 0) {
                    memberDal.Create(studentMember);
                }
                else
                {
                    memberDal.Update(studentMember);
                }

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

            foreach (StudentMember studentMember in items)
            {
                if (studentMember.StudentMemberId > 0 && _studentMembers.ContainsKey(studentMember.StudentMemberId))
                {
                    _studentMembers.Remove(studentMember.StudentMemberId);
                    memberDal.DeleteWithoutCheck(studentMember);
                }
            }

            return true;
        }

        public IEnumerable<StudentMember> GetAll()
        {
            return _studentMembers.Values
                                  .Where(student => student.Position == SailClubMember.Positions.Student);
        }

        public IEnumerable<StudentMember> GetAll(Func<StudentMember, bool> predicate)
        {
            return
                _studentMembers.Values
                               .Where(student => student.Position == SailClubMember.Positions.Student)
                               .Where(predicate);
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

        public bool PromoteToMember(StudentMember studentMember)
        {
            studentMember.Position = SailClubMember.Positions.Member;
            studentMember.BoatDriver = true;

            this.Update(studentMember);

            return true;
        }
        #endregion

        public bool DeleteWithoutCheck(StudentMember studentMember)
        {
            if (studentMember.StudentMemberId > 0 && _studentMembers.ContainsKey(studentMember.StudentMemberId)) {
                _studentMembers.Remove(studentMember.StudentMemberId);
            }

            return true;
        }

        public bool CreateWithId(StudentMember studentMember)
        {
            var memberDal = new SailClubMemberMockDal();

            if (studentMember.StudentMemberId <= 0) { return false; }
            if (studentMember.SailClubMemberId > 0)
            {
                SailClubMember member = memberDal.GetOne(studentMember.SailClubMemberId);

                studentMember.Address = member.Address;
                studentMember.BoatDriver = member.BoatDriver;
                studentMember.Cityname = member.Cityname;
                studentMember.DateOfBirth = member.DateOfBirth;
                studentMember.Email = member.Email;
                studentMember.FirstName = member.FirstName;
                studentMember.Gender = member.Gender;
                studentMember.LastName = member.LastName;
                studentMember.PasswordHash = member.PasswordHash;
                studentMember.PhoneNumber = member.PhoneNumber;
                studentMember.PersonId = member.PersonId;
                studentMember.Position = member.Position;
                studentMember.Postcode = member.Postcode;
                studentMember.Username = member.Username;

                memberDal.Update(studentMember);
            }

            _studentMembers.Add(studentMember.StudentMemberId, studentMember);

            return true;
        }

        private long GetHighestId()
        {
            if (_studentMembers.Count == 0) { return 0; }

            return _studentMembers.Max(studentMember => studentMember.Key);
        }
    }
}
