namespace Application.Common.Exceptions;

public sealed class AlreadyExistException : Exception
{
    public AlreadyExistException(string message) : base(message) { }
}