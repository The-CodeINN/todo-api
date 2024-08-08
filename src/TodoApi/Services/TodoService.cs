using AutoMapper;
using TodoApi.DTOs;
using TodoApi.Entities;
using TodoApi.Exceptions;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly IMapper _mapper;
        private readonly ITodoRepository _repository;

        public TodoService(IMapper mapper, ITodoRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Response<IEnumerable<TodoItemDto>>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return new Response<IEnumerable<TodoItemDto>>("Fetched successfully", _mapper.Map<IEnumerable<TodoItemDto>>(items));
        }

        public async Task<Response<TodoItemDto>> GetByIdAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null)
            {
                throw new CustomException($"Item with id {id} not found");
            }
            return new Response<TodoItemDto>("Fetched successfully", _mapper.Map<TodoItemDto>(item));
        }

        public async Task<Response<TodoItemDto>> AddAsync(TodoItemCreateDto itemDto)
        {
            var item = _mapper.Map<TodoItem>(itemDto);
            var lastItem = (await _repository.GetAllAsync()).OrderByDescending(x => x.Id).FirstOrDefault();
            item.Id = (lastItem != null) ? lastItem.Id + 1 : 1;
            item = await _repository.AddAsync(item);
            return new Response<TodoItemDto>("Todo item created successfully", _mapper.Map<TodoItemDto>(item));
        }

        public async Task<Response<TodoItemDto>> UpdateAsync(TodoItemDto itemDto)
        {
            var item = _mapper.Map<TodoItem>(itemDto);
            item = await _repository.UpdateAsync(item);
            return new Response<TodoItemDto>("Todo item updated successfully", _mapper.Map<TodoItemDto>(item));
        }

        public async Task<Response<string>> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return new Response<string>("Todo item deleted successfully", null);
        }
    }
}
