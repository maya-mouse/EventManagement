using Application.Events.DTOs;
using Application.Tags.DTOs;
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
            .ForMember(dest => dest.ParticipantsCount, opt => opt.MapFrom(src => src.Participants.Count()))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(
            src => src.EventTags.Select(et => et.Tag)
        ));

        CreateMap<CreateEventDto, Event>();

        CreateMap<Event, EventDetailDto>()
           .ForMember(dest => dest.Participants, opt => opt.MapFrom(
             src => src.Participants.Select(ue => ue.User)))
           .ForMember(dest => dest.Tags, opt => opt.MapFrom(
            src => src.EventTags.Select(et => et.Tag)
        ));

        CreateMap<Event, CalendarEventDto>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(
                src => src.EventTags.Select(et => et.Tag)));
        
        CreateMap<Tag, TagDto>();
    }
}