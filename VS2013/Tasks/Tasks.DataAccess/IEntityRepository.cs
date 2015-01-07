using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.DomainModel;

namespace Tasks.DataAccess
{
    public interface IEntityRepository<T> where T : EntityBase
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(DomainModel.TaskStatus status);
        T Add(T entity);
        void Delete(int entityId);
        void Update(T entity);
        T FindById(int entityId);
    }
}
