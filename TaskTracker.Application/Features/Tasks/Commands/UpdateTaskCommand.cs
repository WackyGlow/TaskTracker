using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Application.Features.Tasks.Commands
{
    public class UpdateTaskCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public string Category { get; set; }
        public int Priority { get; set; }
        public bool IsRecurring { get; set; }
        public int? RecurrenceInterval { get; set; }
        public string RecurrenceUnit { get; set; }
    }

}
