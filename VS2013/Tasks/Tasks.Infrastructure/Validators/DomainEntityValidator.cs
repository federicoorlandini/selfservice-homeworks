using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.DomainModel;

namespace Tasks.Infrastructure.Validators
{
    /// <summary>
    /// I use this class to validate the entity in the business logic.
    /// I'm using the Microsoft Enterprise Library Validation Block to be able to
    /// handle the validation rules using property's attributes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
