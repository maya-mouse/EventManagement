using Application.Events.DTOs;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           
            CreateMap<User, ParticipantDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<Event, EventDto>();
            CreateMap<CreateEventDto, Event>();
            
            CreateMap<Event, EventDetailDto>()
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants.Select(ue => ue.User)));
               
        }
    }