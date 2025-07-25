using AutoMapper;
using BootcampProject.Entities.Concrete;
using BootcampProject.Business.DTOs.Bootcamp;

namespace BootcampProject.Business.Profiles
{
    public class BootcampProfile : Profile
    {
        public BootcampProfile()
        {
            CreateMap<Bootcamp, BootcampResponseDto>()
                .ForMember(dest => dest.InstructorFirstName, opt => opt.MapFrom(src => src.Instructor.FirstName))
                .ForMember(dest => dest.InstructorLastName, opt => opt.MapFrom(src => src.Instructor.LastName))
                .ForMember(dest => dest.InstructorCompanyName, opt => opt.MapFrom(src => src.Instructor.CompanyName));

            CreateMap<BootcampCreateRequestDto, Bootcamp>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<BootcampUpdateRequestDto, Bootcamp>();
        }
    }
}