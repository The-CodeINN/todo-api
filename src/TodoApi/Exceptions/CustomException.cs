namespace TodoApi.Exceptions;

public class CustomException(string message) : Exception(message)
{
}
