using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.DomainModel;

namespace Tasks.Infrastructure.Validators
{
    public class DomainEntityValidator<T> : IDomainEntityValidator<T> where T : IEntity
    {
        public bool Validate(T entity)
        {
            var validator = ValidationFactory.CreateValidator<T>();
            var validationResult = validator.Validate(entity);
            return validationResult.IsValid;
        }
    }
}
