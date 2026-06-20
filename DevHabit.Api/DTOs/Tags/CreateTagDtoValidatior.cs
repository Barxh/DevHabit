using FluentValidation;

namespace DevHabit.Api.DTOs.Tags;

public sealed class CreateTagDtoValidatior : AbstractValidator<CreateTagDto>
{
    public CreateTagDtoValidatior()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Description).MaximumLength(50);
    }
}