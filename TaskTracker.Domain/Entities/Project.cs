namespace TaskTracker.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public bool IsCompleted { get; private set; }

        public ICollection<Person> Contributors { get; private set; } = new List<Person>();

        // TODO
        // Future addition
        // public ICollection<ProjectMilestone> Milestones { get; private set; }

        private Project() { }

        public Project(string name, string description, DateTime startDate)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Project name is required.");
            if (startDate == default) throw new ArgumentException("Start date is required.");

            Name = name;
            Description = description;
            StartDate = startDate;
            IsCompleted = false;
        }

        public void Complete(DateTime endDate)
        {
            if (endDate < StartDate)
                throw new ArgumentException("End date cannot be earlier than start date.");

            EndDate = endDate;
            IsCompleted = true;
        }
    }
}
