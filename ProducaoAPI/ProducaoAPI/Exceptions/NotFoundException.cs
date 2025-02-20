namespace ProducaoAPI.Exceptions
{
    public class NotFoundException(string message) : Exception(message)
    {
        public int StatusCode = 404;
    }
}
