using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.DataAccess.Tasks
{
    public interface ITaskRepository : IEntityRepository<DomainModel.Task>
    {
        IEnumerable<DomainModel.User> GetWatcherUsersForTask(int taskId);
    }
}
