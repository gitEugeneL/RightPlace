namespace Application.Common.Exceptions;

public sealed class AccessDeniedException : Exception
{
    public AccessDeniedException(string name, object key)
        : base($"Entity: {name} ({key}) doesn't exist or your password is incorrect") { }
}
