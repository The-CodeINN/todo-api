namespace TodoApi.DTOs;

public class Response<T>(string statusMessage, T result)
{
    public string StatusMessage { get; set; } = statusMessage;
    public T Result { get; set; } = result;
}