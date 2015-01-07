using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.DomainModel
{
    public abstract class EntityBase : IEntity
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
        
        // The ID of the creator user is usefull to be able to assign the creator user without retriving the entire User entity from the database
        public int CreatorUserId { get; set; }

        public User Creator { get; set; }
    }
}
