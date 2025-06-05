using TaskTracker.Domain.ValueObjects;

namespace TaskTracker.Domain.Entities
{
    public class Person
    {
        public Guid Id { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateOfBirth DateOfBirth { get; private set; }

        // Navigation
        public ICollection<TaskItem> AssignedTasks { get; private set; } = new List<TaskItem>();
        public ICollection<Project> AssignedProjects { get; private set; } = new List<Project>();

        // EF constructor
        #pragma warning disable CS8618 // Non-nullable property is uninitialized    
        private Person() 
        { 
        }
        #pragma warning restore CS8618

        public Person(Guid id, string firstName, string lastName, DateOfBirth dateOfBirth)
        {   
            if (id == Guid.Empty) { throw new ArgumentNullException("id"); }
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required.");
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required.");
            if (dateOfBirth.Value > DateOnly.FromDateTime(DateTime.Today)) throw new ArgumentException("Date of birth cannot be in the future.");

            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            
        }

        public void Update(string firstName, string lastName, DateOfBirth dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required.");
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required.");
            if (dateOfBirth.Value > DateOnly.FromDateTime(DateTime.Today)) throw new ArgumentException("Date of birth cannot be in the future.");

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            DateOfBirth = dateOfBirth;
        }


        public string FullName => $"{FirstName} {LastName}";
    }
}
