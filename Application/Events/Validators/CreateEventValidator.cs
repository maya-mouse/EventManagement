using Application.Events.Commands;
using FluentValidation;

namespace Application.Events.Validators;
public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(c => c.createEventDto.Title).NotEmpty().WithMessage("Назва події є обов'язковою.");
        RuleFor(c => c.createEventDto.Location).NotEmpty().WithMessage("Місце проведення є обов'язковим.");
        RuleFor(c => c.createEventDto.DateTime)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Не можна створити подію в минулому."); 
    }
}