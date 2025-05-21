using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.ValueObjects
{
    public sealed class Recurrence : IEquatable<Recurrence>
    {
        public int Interval { get; }
        public RecurrenceUnit Unit { get; }

        public Recurrence(int interval, RecurrenceUnit unit)
        {
            if (interval <= 0)
                throw new ArgumentException("Interval must be greater than zero.");

            Interval = interval;
            Unit = unit;
        }

        public override string ToString() => $"{Interval} {Unit}";

        public bool Equals(Recurrence? other) =>
            other != null && Interval == other.Interval && Unit == other.Unit;

        public override bool Equals(object? obj) => obj is Recurrence other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Interval, Unit);
    }
}