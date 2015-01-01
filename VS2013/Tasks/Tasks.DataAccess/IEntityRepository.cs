using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.DomainModel;

namespace Tasks.DataAccess
{
    public interface IEntityRepository<T> where T : IEntity
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(DomainModel.TaskStatus status);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T FindById(int Id);
    }
}
