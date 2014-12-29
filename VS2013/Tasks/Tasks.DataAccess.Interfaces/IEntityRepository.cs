using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.DomainModel;

namespace Tasks.DataAccess.Interfaces
{
    public interface IEntityRepository<T> where T : IEntity
    {
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Add(IEnumerable<T> entitiesCollection);
        void Delete(T entity);
        void Update(T entity);
        T FindById(int Id);
    }
}
