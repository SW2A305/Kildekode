using System;
using System.Collections.Generic;
using System.Linq;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Mock
{
    public class TeamMockDal : ITeamDal
    {
        private static Dictionary<long, Team> _teams;

        public TeamMockDal(bool useForTests = false)
        {
            if (useForTests || _teams == null) { _teams = new Dictionary<long, Team>(); }
        }

        #region ITeamDal Members
        public bool Create(params Team[] items)
        {
            foreach (Team team in items)
            {
                team.TeamId = this.GetHighestId() + 1;
                _teams.Add(team.TeamId, team);
            }

            return true;
        }

        public bool Update(params Team[] items)
        {
            foreach (Team team in items) {
                if (team.TeamId > 0 && _teams.ContainsKey(team.TeamId)) { _teams[team.TeamId] = team; }
            }

            return true;
        }

        public bool Delete(params Team[] items)
        {
            foreach (Team team in items) { if (team.TeamId > 0) { _teams.Remove(team.TeamId); } }

            return true;
        }

        public IEnumerable<Team> GetAll() { return _teams.Values; }

        public IEnumerable<Team> GetAll(Func<Team, bool> predicate) { return _teams.Values.Where(predicate); }

        public Team GetOne(long itemId)
        {
            if (_teams.ContainsKey(itemId)) { return _teams[itemId]; }

            return null;
        }

        public void LoadData(Team item)
        {
            var studentDal = new StudentMemberMockDal();
            var lectureDal = new LectureMockDal();

            item.TeamMembers = studentDal.GetAll(student => student.AssociatedTeamId == item.TeamId).ToList();
            item.Lectures = lectureDal.GetAll(lecture => lecture.TeamId == item.TeamId).ToList();
        }

        public void LoadData(IEnumerable<Team> items) { foreach (Team team in items) { LoadData(team); } }
        #endregion

        public bool CreateWithId(Team team)
        {
            if (team.TeamId <= 0) { return false; }

            _teams.Add(team.TeamId, team);

            return true;
        }

        private long GetHighestId()
        {
            if (_teams.Count == 0) { return 0; }

            return _teams.Max(team => team.Key);
        }
    }
}
