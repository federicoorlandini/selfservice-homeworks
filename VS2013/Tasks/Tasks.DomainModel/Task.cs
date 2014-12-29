using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.DomainModel
{
    public class Task : IEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public TaskStatus Status { get; set; }

        public int EstimatedHours { get; set; }

        public IEnumerable<WorkingSession> WorkingSessions { get; set; }
        public IEnumerable<TaskComment> Comments { get; set; }

        public int WorkedHours { get; set; }
    }
}
