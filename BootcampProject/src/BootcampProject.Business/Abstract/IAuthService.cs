using System.Threading.Tasks;
using BootcampProject.Business.DTOs.Auth;

namespace BootcampProject.Business.Abstract
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
    }
}