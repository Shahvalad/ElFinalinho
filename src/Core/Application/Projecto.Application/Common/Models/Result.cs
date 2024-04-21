namespace Projecto.Application.Common.Models
{
    public class Result<T> where T : class
    {
        internal Result(bool succeeded, IEnumerable<string> errors, string successMessage = null, T data = null)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
            SuccessMessage = successMessage;
            Data = data;
        }

        public bool Succeeded { get; init; }
        public string[] Errors { get; init; }
        public string SuccessMessage { get; init; }
        public T Data { get; init; }

        public static Result<T> Success(T data = null, string message = null)
        {
            return new Result<T>(true, Array.Empty<string>(), message, data);
        }

        public static Result<T> Failure(IEnumerable<string> errors)
        {
            return new Result<T>(false, errors);
        }
    }
}
