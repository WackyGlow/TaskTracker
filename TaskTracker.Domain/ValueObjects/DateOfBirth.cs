using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Domain.ValueObjects
{
    public class DateOfBirth
    {
        public DateOnly Value { get; }

        // constructor for EF
        private DateOfBirth() { }

        public DateOfBirth(DateOnly value)
        {
            if (value < DateOnly.FromDateTime(DateTime.Today)) throw new ArgumentException("Date of birth cannot be in the future.");

            Value = value;
        }

        public int Age => CalculateAge();

        private int CalculateAge()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - Value.Year;
            return age;
        }

        public override string ToString() => Value.ToString("yyyy-MM-dd");
        public override bool Equals(object? obj)
        {
            return obj is DateOfBirth other && Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
