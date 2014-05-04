using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McSntt.DataAbstractionLayer
{
    public interface IGenericDal<T> where T : class
    {
        bool Create(params T[] items);
        bool Update(params T[] items);
        bool Delete(params T[] items);
        IEnumerable<T> GetAll();
        T GetOne(int itemId);
    }
}
