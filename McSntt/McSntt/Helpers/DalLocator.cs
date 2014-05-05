using McSntt.DataAbstractionLayer;

namespace McSntt.Helpers
{
    public static class DalLocator
    {
        public static IBoatDal BoatDal { get { return new BoatEfDal(); } }
        public static IEventDal EventDal { get { return new EventEfDal(); } }
        public static ILectureDal LectureDal { get { return new LectureEfDal(); } }
        public static ILogbookDal LogbookDal { get { return new LogbookEfDal(); } }
        public static IPersonDal PersonDal { get { return new PersonEfDal(); } }
        public static IRegularTripDal RegularTripDal { get { return new RegularTripEfDal(); } }
        public static ISailClubMemberDal SailClubMemberDal { get { return new SailClubMemberEfDal(); } }
        public static IStudentMemberDal StudentMemberDal { get { return new StudentMemberEfDal(); } }
        public static ITeamDal TeamDal { get { return new TeamEfDal(); } }
    }
}
