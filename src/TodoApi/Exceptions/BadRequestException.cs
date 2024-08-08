namespace TodoApi.Exceptions;

public class BadRequestException(string message) : CustomException(message)
{
}
