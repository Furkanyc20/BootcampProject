using AutoMapper;
using BootcampProject.Entities.Concrete;
using BootcampProject.Business.DTOs.Instructor;

namespace BootcampProject.Business.Profiles
{
    public class InstructorProfile : Profile
    {
        public InstructorProfile()
        {
            CreateMap<Instructor, InstructorResponseDto>();

            CreateMap<InstructorCreateRequestDto, Instructor>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<InstructorUpdateRequestDto, Instructor>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());
        }
    }
}