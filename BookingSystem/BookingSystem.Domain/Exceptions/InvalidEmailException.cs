namespace BookingSystem.Domain.Exceptions;

public class InvalidEmailException : DomainException
{
    public InvalidEmailException(string email) : base($"Email '{email}' is invalid")
    {
    }
}
