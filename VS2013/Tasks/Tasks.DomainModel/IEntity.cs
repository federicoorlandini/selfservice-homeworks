using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.DomainModel
{
    public interface IEntity
    {
        int ID { get; set; }
        DateTime Created { get; set; }

        User Creator { get; set; }
    }
}
