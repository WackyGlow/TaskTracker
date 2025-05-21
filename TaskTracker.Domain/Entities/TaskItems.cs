using TaskTracker.Domain.Enums;
using TaskTracker.Domain.ValueObjects;

namespace TaskTracker.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime DueDate { get; private set; }
        public bool IsCompleted { get; private set; }

        public Category Category { get; private set; }
        public Priority Priority { get; private set; }
        public Recurrence? Recurrence { get; private set; }

        public ICollection<Person> AssignedPeople { get; private set; } = new List<Person>();

        private TaskItem() { }

        public TaskItem(string name, string description, DateTime dueDate, Category category, Priority priority, Recurrence? recurrence = null)
        {
            Name = name;
            Description = description;
            DueDate = dueDate;
            Category = category;
            Priority = priority;
            Recurrence = recurrence;
            IsCompleted = false;
        }

        public void MarkCompleted() => IsCompleted = true;
        public void Reschedule(DateTime newDueDate) => DueDate = newDueDate;
        public void UpdateRecurrence(Recurrence? recurrence) => Recurrence = recurrence;
    }
}