namespace ReservatieServiceBL.Exceptions;

public class RestaurantManagerException : Exception
{
    public RestaurantManagerException(string message) : base(message)
    {
    }

    public RestaurantManagerException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}