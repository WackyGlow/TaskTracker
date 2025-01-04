namespace TaskTracker.Application.DTOs
{
    public class TaskItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; } // This should remain in the DTO
        public string Category { get; set; }
        public int Priority { get; set; } // Assuming 1 = Low, 2 = Medium, 3 = High
        public bool IsRecurring { get; set; }
        public int? RecurrenceInterval { get; set; } // Interval in days, weeks, etc.
        public string RecurrenceUnit { get; set; } // "Days", "Weeks", "Months", etc.
        public ICollection<PersonDto> AssignedPersons { get; set; } // List of Persons for the assignments
    }
}
