namespace ProducaoAPI.Exceptions
{
    public class BadRequestException(string message) : Exception(message)
    {
        public int StatusCode = 400;
    }
}
