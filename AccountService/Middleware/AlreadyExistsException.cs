namespace AccountService.Middleware
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string message) : base(message)
        {
        }
    }
}
