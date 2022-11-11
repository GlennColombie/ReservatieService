namespace ReservatieServiceBL.Exceptions;

public class LocatieManagerException : Exception
{
    public LocatieManagerException(string? message) : base(message)
    {
    }

    public LocatieManagerException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}