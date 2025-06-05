using TaskTracker.Domain.Enums;
using TaskTracker.Domain.ValueObjects;

namespace TaskTracker.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTimeOffset DueDate { get; private set; }
        public bool IsCompleted { get; private set; }

        public Category Category { get; private set; }
        public Priority Priority { get; private set; }
        public Recurrence? Recurrence { get; private set; }

        public ICollection<Person> AssignedPeople { get; private set; } = new List<Person>();

#pragma warning disable CS8618 // Non-nullable property is uninitialized
        private TaskItem() { }
#pragma warning restore CS8618

        public TaskItem(string name, string description, DateTimeOffset dueDate, Category category, Priority priority, Recurrence? recurrence = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Task name is required.");
            if (dueDate == default) throw new ArgumentException("Due date is required.");

            Name = name;
            Description = description;
            DueDate = dueDate;
            Category = category;
            Priority = priority;
            Recurrence = recurrence;
            IsCompleted = false;
        }

        public void MarkCompleted() => IsCompleted = true;

        public void Reschedule(DateTimeOffset newDueDate)
        {
            if (newDueDate == default)
                throw new ArgumentException("New due date is required.");

            DueDate = newDueDate;
        }

        public void UpdateRecurrence(Recurrence? recurrence) => Recurrence = recurrence;

        public void Update(string name,string description,DateTimeOffset dueDate,Category category,Priority priority,Recurrence? recurrence,bool isCompleted)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Task name is required.");
            if (dueDate == default) throw new ArgumentException("Due date is required.");

            Name = name;
            Description = description;
            DueDate = dueDate;
            Category = category;
            Priority = priority;
            Recurrence = recurrence;
            IsCompleted = isCompleted;
        }
    }
}