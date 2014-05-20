using McSntt.Models;

namespace McSntt.Helpers
{
    public static class GlobalInformation
    {
        private static SailClubMember.Positions UserPosition { get; set; }
        private static string UserFullName { get; set; }
        private static int UserId { get; set; }
        public static SailClubMember CurrentUser { get; set; }
        public static StudentMember CurrentStudentMember { get; set; }
    }
}
