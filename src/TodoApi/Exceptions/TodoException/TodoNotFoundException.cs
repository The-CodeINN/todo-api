namespace TodoApi.Exceptions.TodoException
{
    public class TodoNotFoundException(int id) : CustomException($"Todo item with id {id} was not found.")
    {
    }
}
