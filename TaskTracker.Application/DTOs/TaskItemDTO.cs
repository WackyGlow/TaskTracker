using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Application.DTOs
{
    public class TaskItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public string Category { get; set; }
        public int Priority { get; set; }  // 1 = Low, 2 = Medium, 3 = High
        public bool IsRecurring { get; set; }
        public int? RecurrenceInterval { get; set; }
        public string RecurrenceUnit { get; set; }  // "Days", "Weeks", "Months", etc.
    }

}
