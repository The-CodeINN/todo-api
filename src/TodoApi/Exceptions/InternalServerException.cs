namespace TodoApi.Exceptions;

public class InternalServerException(string message) : CustomException(message)
{
}
