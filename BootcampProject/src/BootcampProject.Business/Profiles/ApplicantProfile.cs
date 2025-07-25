using AutoMapper;
using BootcampProject.Entities.Concrete;
using BootcampProject.Business.DTOs.Applicant;

namespace BootcampProject.Business.Profiles
{
    public class ApplicantProfile : Profile
    {
        public ApplicantProfile()
        {
            CreateMap<Applicant, ApplicantResponseDto>();

            CreateMap<ApplicantCreateRequestDto, Applicant>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<ApplicantUpdateRequestDto, Applicant>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());
        }
    }
}