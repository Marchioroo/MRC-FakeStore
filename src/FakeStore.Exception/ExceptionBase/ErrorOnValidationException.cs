namespace FakeStore.Exception.ExceptionBase
{
    public class ErrorOnValidationException : FakeStoreException
    {
        public List<string> Errors { get; set; } = [];

        public ErrorOnValidationException(List<string> errorMessages)
            : base("Validation error: one or more validation failures occurred.")
        {
            Errors = errorMessages;
        }
    }
}