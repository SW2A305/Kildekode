using McSntt.Models;

namespace McSntt.DataAbstractionLayer
{
    public interface IEventDal : IGenericDal<Event>
    {
        void LoadParticipants(Event eventItem);
    }
}