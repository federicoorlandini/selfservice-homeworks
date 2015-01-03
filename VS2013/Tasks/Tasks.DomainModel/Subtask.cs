using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tasks.DomainModel
{
    public class Subtask : Task
    {
        public Task ParentTask
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
