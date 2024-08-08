namespace TodoApi.Exceptions;

public class NotFoundException(string message) : CustomException(message)
{
}
