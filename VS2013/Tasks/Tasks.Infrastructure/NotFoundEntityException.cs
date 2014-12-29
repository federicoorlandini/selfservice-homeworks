using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.DomainModel;

namespace Tasks.Infrastructure
{
    public class NotFoundEntityException<T> : Exception where T : IEntity
    {
        public NotFoundEntityException() : base() { }
        public NotFoundEntityException(string message) : base(message) { }
        public NotFoundEntityException(string message, Exception innerException) : base(message, innerException) { }
    }
}
