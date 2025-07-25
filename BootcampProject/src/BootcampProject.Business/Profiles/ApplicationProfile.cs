using AutoMapper;
using BootcampProject.Entities.Concrete;
using BootcampProject.Business.DTOs.Application;

namespace BootcampProject.Business.Profiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Application, ApplicationResponseDto>()
                .ForMember(dest => dest.ApplicantFirstName, opt => opt.MapFrom(src => src.Applicant.FirstName))
                .ForMember(dest => dest.ApplicantLastName, opt => opt.MapFrom(src => src.Applicant.LastName))
                .ForMember(dest => dest.BootcampName, opt => opt.MapFrom(src => src.Bootcamp.Name));

            CreateMap<ApplicationCreateRequestDto, Application>();

            CreateMap<ApplicationUpdateRequestDto, Application>();
        }
    }
}