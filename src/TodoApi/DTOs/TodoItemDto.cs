namespace TodoApi.DTOs;

public class TodoItemCreateDto
{
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}

public class TodoItemDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}
