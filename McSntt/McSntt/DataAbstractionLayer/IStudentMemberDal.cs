using McSntt.Models;

namespace McSntt.DataAbstractionLayer
{
    public interface IStudentMemberDal : IGenericDal<StudentMember>
    {
        bool PromoteToMember(StudentMember studentMember);
    }
}
