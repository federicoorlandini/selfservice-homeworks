using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.DomainModel
{
    public class Task : EntityBase, IEntity
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public TaskStatus Status { get; set; }

        [Range(0, int.MaxValue)]
        public int EstimatedHours { get; set; }

        public IEnumerable<WorkSession> WorkSessions { get; set; }

        public IEnumerable<TaskComment> Comments { get; set; }

        /// <summary>
        /// Return the total logged work session for this task
        /// </summary>
        public double TotalWorkedHours {
            get 
            { 
                return (WorkSessions == null ? 0 : WorkSessions.Select(item => new TimeSpan(item.Hours, item.Minutes, item.Seconds)).Sum(item => item.TotalHours)); 
            }
        }

        public ICollection<User> WatcherUsers
        {
            get;
            set;
        }
    }
}
