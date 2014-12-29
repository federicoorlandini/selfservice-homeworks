using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.DomainModel
{
    public class TaskComment : IEntity
    {
        public string CommentText { get; set; }
    }
}
