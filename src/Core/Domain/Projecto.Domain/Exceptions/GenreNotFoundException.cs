namespace Projecto.Domain.Exceptions
{
    public class GenreNotFoundException : Exception
    {
        public GenreNotFoundException(string message) : base(message)
        {
        }
    }
}
