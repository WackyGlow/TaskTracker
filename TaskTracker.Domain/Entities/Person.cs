namespace TaskTracker.Domain.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        // Relationships
        public ICollection<TaskAssignment> Assignments { get; set; }
    }
}