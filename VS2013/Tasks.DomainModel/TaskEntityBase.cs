using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.DomainModel
{
    public abstract class TaskEntityBase
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }

        public int CreatorUserID { get; set; }
        public User Creator { get; set; }
    }
}
