using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Application.Exceptions
{
    internal class ValidationException : Exception
    {
        public IDictionary<string, string> Errors { get; }
        public ValidationException(string message, IDictionary<string, string[]> errors = null) : base(message)
        {
            Errors = errors ?? new Dictionary<string, string[]>();
        }
    }
}
