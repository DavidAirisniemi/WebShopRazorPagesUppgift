using System.Collections.Generic;

namespace ShopUI.DataAccess
{
    public interface IDataAccess<T>
    {
        public List<T> GetAll();
        void Serialize(List<T> items);
        public T GetById(int id);
    }
    
}
