namespace Application.Common.Exceptions;

public sealed class AlreadyExistException : Exception
{
    public AlreadyExistException(string name, object key)
        : base($"Entity: {name} ({key}) already exists") { }
}