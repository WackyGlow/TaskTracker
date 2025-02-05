namespace TaskTracker.Domain.Entities
{
    public class TaskAssignment
    {
        public int TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
