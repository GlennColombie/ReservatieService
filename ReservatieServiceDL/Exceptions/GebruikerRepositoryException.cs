namespace ReservatieServiceDL.Exceptions;

public class GebruikerRepositoryException : Exception
{
    public GebruikerRepositoryException(string message, Exception exception) : base(message, exception)
    {
    }
}