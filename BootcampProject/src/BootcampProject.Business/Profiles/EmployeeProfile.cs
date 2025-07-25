using AutoMapper;
using BootcampProject.Entities.Concrete;
using BootcampProject.Business.DTOs.Employee;

namespace BootcampProject.Business.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeResponseDto>();

            CreateMap<EmployeeCreateRequestDto, Employee>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<EmployeeUpdateRequestDto, Employee>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());
        }
    }
}