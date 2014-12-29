using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Infrastructure.Interfaces
{
    public interface ITasksService
    {
        IEnumerable<DomainModel.Task> GetAll();

        DomainModel.Task FindById(int ID);
    }
}
