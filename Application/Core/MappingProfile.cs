using Application.Events.DTOs;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<User, ParticipantDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

        CreateMap<Event, EventDto>()
            .ForMember(dest => dest.ParticipantsCount, opt => opt.MapFrom(src => src.Participants.Count()));

        CreateMap<CreateEventDto, Event>();

        CreateMap<Event, EventDetailDto>()
           .ForMember(dest => dest.Participants, opt => opt.MapFrom(
             src => src.Participants.Select(ue => ue.User)
           ));
    }
}