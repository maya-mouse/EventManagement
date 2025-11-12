using Application.Events.Commands;
using FluentValidation;

namespace Application.Events.Validators;
public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(c => c.createEventDto.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(c => c.createEventDto.Location).NotEmpty().WithMessage("Location is required");
        RuleFor(c => c.createEventDto.Capacity).GreaterThan(0).When(c => c.createEventDto.Capacity.HasValue);
        RuleFor(c => c.createEventDto.DateTime)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("You cannot create an event in the past");
        RuleFor(c => c.createEventDto.TagNames)
            .Must(tags => tags == null || tags.Count <= 5)
            .WithMessage("Maximum 5 tags are allowed per event");
    }
}