using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<TodoItemDto>>>> GetTodos()
        {
            var response = await _todoService.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<TodoItemDto>>> GetById(int id)
        {
            var response = await _todoService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoItem(TodoItemCreateDto itemDto)
        {
            var response = await _todoService.AddAsync(itemDto);
            return CreatedAtAction(nameof(GetById), new { id = response.Result.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<TodoItemDto>>> Update(int id, TodoItemDto itemDto)
        {
            if (id != itemDto.Id)
            {
                return BadRequest(new Response<string>("Id mismatch", null));
            }

            var response = await _todoService.UpdateAsync(itemDto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> Delete(int id)
        {
            var response = await _todoService.DeleteAsync(id);
            return Ok(response);
        }
    }
}
