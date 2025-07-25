using AutoMapper;
using BootcampProject.Entities.Concrete;
using BootcampProject.Business.DTOs.Blacklist;

namespace BootcampProject.Business.Profiles
{
    public class BlacklistProfile : Profile
    {
        public BlacklistProfile()
        {
            CreateMap<Blacklist, BlacklistResponseDto>()
                .ForMember(dest => dest.ApplicantFirstName, opt => opt.MapFrom(src => src.Applicant.FirstName))
                .ForMember(dest => dest.ApplicantLastName, opt => opt.MapFrom(src => src.Applicant.LastName))
                .ForMember(dest => dest.ApplicantEmail, opt => opt.MapFrom(src => src.Applicant.Email));

            CreateMap<BlacklistCreateRequestDto, Blacklist>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<BlacklistUpdateRequestDto, Blacklist>();
        }
    }
}