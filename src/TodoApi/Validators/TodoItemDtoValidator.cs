using FluentValidation;
using TodoApi.DTOs;

namespace TodoApi.Validators
{
    public class TodoItemCreateDtoValidator : AbstractValidator<TodoItemCreateDto>
    {
        public TodoItemCreateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        }
    }

    public class TodoItemDtoValidator : AbstractValidator<TodoItemDto>
    {
        public TodoItemDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(1).WithMessage("Id must be a positive integer.");
        }
    }
}
