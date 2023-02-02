using AutoMapper;
using Meet.API.Entities;
using Meet.API.Models;

namespace Meet.API;

public class MeetupProfile : Profile
{
	/// <summary>
	/// Maps our data into our DTO class using Automapper
	/// </summary>
	public MeetupProfile()
	{
		CreateMap<Meetup, MeetupDetailsDTO>()
			.ForMember(m => m.City, map => map.MapFrom(meetup => meetup.Location.City))
			.ForMember(m => m.PostCode, map => map.MapFrom(meetup => meetup.Location.PostCode))
			.ForMember(m => m.Street, map => map.MapFrom(meetup => meetup.Location.Street));

		// the types overlap so we don't need custom mappings
		CreateMap<MeetupDTO, Meetup>();

		CreateMap<LectureDTO, Lecture>().ReverseMap();	
	}
}
