using McSntt.DataAbstractionLayer;
using McSntt.DataAbstractionLayer.Mock;

namespace McSntt.Helpers
{
    public static class DalLocator
    {
        public static IBoatDal BoatDal
        {
            get { return new BoatMockDal(); }
        }

        public static IEventDal EventDal
        {
            get { return new EventMockDal(); }
        }

        public static ILectureDal LectureDal
        {
            get { return new LectureMockDal(); }
        }

        public static ILogbookDal LogbookDal
        {
            get { return new LogbookMockDal(); }
        }

        public static IPersonDal PersonDal
        {
            get { return new PersonMockDal(); }
        }

        public static IRegularTripDal RegularTripDal
        {
            get { return new RegularTripMockDal(); }
        }

        public static ISailClubMemberDal SailClubMemberDal
        {
            get { return new SailClubMemberMockDal(); }
        }

        public static IStudentMemberDal StudentMemberDal
        {
            get { return new StudentMemberMockDal(); }
        }

        public static ITeamDal TeamDal
        {
            get { return new TeamMockDal(); }
        }
    }
}
