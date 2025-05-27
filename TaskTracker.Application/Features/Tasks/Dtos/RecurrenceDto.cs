namespace TaskTracker.Application.Features.Tasks.Dtos
{
    public class RecurrenceDto
    {
        public int Interval { get; init; }
        public string Unit { get; init; } = string.Empty;
    }
}