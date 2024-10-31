using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public string Category { get; set; }
        public int Priority { get; set; } // Assuming 1 = Low, 2 = Medium, 3 = High

        // New Fields for Recurrence
        public bool IsRecurring { get; set; } // To indicate if the task is recurring
        public int? RecurrenceInterval { get; set; } // Interval in days, weeks, etc.
        public string RecurrenceUnit { get; set; } // "Days", "Weeks", "Months", etc.

        // Relationships
        public ICollection<TaskAssignment> Assignments { get; set; }
    }
}
