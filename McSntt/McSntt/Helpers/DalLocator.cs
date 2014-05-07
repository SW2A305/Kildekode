using McSntt.DataAbstractionLayer;
using McSntt.DataAbstractionLayer.Sqlite;

namespace McSntt.Helpers
{
    public static class DalLocator
    {
        public static IBoatDal BoatDal { get { return new BoatSqliteDal(); } }
        public static IEventDal EventDal { get { return new EventSqliteDal(); } }
        public static ILectureDal LectureDal { get { return new LectureSqliteDal(); } }
        public static ILogbookDal LogbookDal { get { return new LogbookSqliteDal(); } }
        public static IPersonDal PersonDal { get { return new PersonSqliteDal(); } }
        public static IRegularTripDal RegularTripDal { get { return new RegularTripSqliteDal(); } }
        public static ISailClubMemberDal SailClubMemberDal { get { return new SailClubMemberSqliteDal(); } }
        public static IStudentMemberDal StudentMemberDal { get { return new StudentMemberSqliteDal(); } }
        public static ITeamDal TeamDal { get { return new TeamSqliteDal(); } }
    }
}
