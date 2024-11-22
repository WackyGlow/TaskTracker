namespace TaskTracker.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string> Errors { get; }

        public ValidationException(IDictionary<string, string[]> validationErrors)
        {
            Errors = new Dictionary<string, string>();
            foreach (var error in validationErrors)
            {
                Errors[error.Key] = string.Join(", ", error.Value);
            }
        }
    }
}