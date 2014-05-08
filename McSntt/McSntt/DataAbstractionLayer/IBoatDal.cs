using McSntt.Models;

namespace McSntt.DataAbstractionLayer
{
    public interface IBoatDal : IGenericDal<Boat>
    {
        void LoadSailTrips(Boat boat);
    }
}
