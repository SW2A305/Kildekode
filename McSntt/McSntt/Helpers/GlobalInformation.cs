using McSntt.Models;

namespace McSntt.Helpers
{
    public static class GlobalInformation
    {
        static SailClubMember.Positions UserPosition { get; set; }
        static string UserFullName { get; set; }
        static int UserId { get; set; }
    }
}