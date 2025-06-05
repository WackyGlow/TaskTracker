using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Utils
{
    public static class EnumParser
    {
        public static bool TryParseRecurrenceUnit(string? input, out RecurrenceUnit unit)
        {
            return Enum.TryParse(input, ignoreCase: true, out unit)
                   && Enum.IsDefined(typeof(RecurrenceUnit), unit);
        }

        public static bool TryParsePriority(string? input, out Priority priority)
        {
            return Enum.TryParse(input, ignoreCase: true, out priority)
                   && Enum.IsDefined(typeof(Priority), priority);
        }

        public static RecurrenceUnit ParseRecurrenceUnitOrThrow(string? input)
        {
            if (!TryParseRecurrenceUnit(input, out var unit))
                throw new ArgumentException($"Invalid recurrence unit: {input}");
            return unit;
        }

        public static Priority ParsePriorityOrThrow(string? input)
        {
            if (!TryParsePriority(input, out var priority))
                throw new ArgumentException($"Invalid priority: {input}");
            return priority;
        }
    }
}
