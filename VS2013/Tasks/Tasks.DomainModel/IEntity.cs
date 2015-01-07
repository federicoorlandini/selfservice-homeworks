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

        // The ID of the creator user is usefull to be able to assign the creator user without retriving the entire User entity from the database
        int CreatorUserId { get; set; }

        User Creator { get; set; }
    }
}
