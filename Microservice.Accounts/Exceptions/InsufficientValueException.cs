using System;

namespace Microservice.Accounts.Exceptions
{
    public class InsufficientValueException : Exception
    {
        public InsufficientValueException() { }
        public InsufficientValueException(string message) : base(message) { }
        public InsufficientValueException(string message, Exception inner) : base(message, inner) { }
    }
}
