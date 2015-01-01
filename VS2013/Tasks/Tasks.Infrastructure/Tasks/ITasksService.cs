using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Infrastructure.Tasks
{
    public interface ITasksService
    {
        IEnumerable<DomainModel.Task> GetAll();
        IEnumerable<DomainModel.Task> GetAll(DomainModel.TaskStatus status);

        DomainModel.Task FindById(int ID);

        DomainModel.Task Add(DomainModel.Task task);

        DomainModel.Task Update(DomainModel.Task task);

        bool Delete(int ID);
    }
}
