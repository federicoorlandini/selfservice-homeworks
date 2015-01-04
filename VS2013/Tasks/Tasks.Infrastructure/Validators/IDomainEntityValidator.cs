using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.DomainModel;

namespace Tasks.Infrastructure.Validators
{
    public interface IDomainEntityValidator<T> where T : IEntity
    {
        bool Validate(T entity);
    }
}
