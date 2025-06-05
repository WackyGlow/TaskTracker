namespace TaskTracker.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTimeOffset StartDate { get; private set; }
        public DateTimeOffset? EndDate { get; private set; }
        public bool IsCompleted { get; private set; }

        public ICollection<Person> Contributors { get; private set; } = new List<Person>();

        // TODO: Future addition
        // public ICollection<ProjectMilestone> Milestones { get; private set; }

#pragma warning disable CS8618 // Non-nullable property is uninitialized
        private Project() { }
#pragma warning restore CS8618

        public Project(string name, string description, DateTimeOffset startDate)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Project name is required.");
            if (startDate == default) throw new ArgumentException("Start date is required.");

            Name = name;
            Description = description;
            StartDate = startDate;
            IsCompleted = false;
        }

        public void Update(string name, string description, DateTimeOffset startDate, DateTimeOffset? endDate, bool isCompleted)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Project name is required.");

            if (startDate == default)
                throw new ArgumentException("Start date is required.");

            if (endDate.HasValue && endDate.Value < startDate)
                throw new ArgumentException("End date cannot be earlier than start date.");

            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            IsCompleted = isCompleted;
        }

        public void Complete(DateTimeOffset endDate)
        {
            if (endDate < StartDate)
                throw new ArgumentException("End date cannot be earlier than start date.");

            EndDate = endDate;
            IsCompleted = true;
        }

        public void Uncomplete()
        {
            EndDate = null;
            IsCompleted = false;
        }
    }
}