using AutoMapper;
using TodoApi.DTOs;
using TodoApi.Entities;

namespace TodoApi.RequestHelpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TodoItemCreateDto, TodoItem>().ReverseMap();
        CreateMap<TodoItemDto, TodoItem>().ReverseMap();
    }
}
