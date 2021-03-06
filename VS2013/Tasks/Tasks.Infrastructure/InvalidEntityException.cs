﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.DomainModel;

namespace Tasks.Infrastructure
{
    /// <summary>
    /// This exception is throwed if an entity is not valid
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InvalidEntityException<T> : Exception where T : IEntity
    {
        public InvalidEntityException() : base() { }
        public InvalidEntityException(string message) : base(message) { }
        public InvalidEntityException(string message, Exception innerException) : base(message, innerException) { }
    }
}
