using System;
using System.Security.Claims;
using BootcampProject.Entities.Concrete;

namespace BootcampProject.Core.Security
{
    public interface IJwtHelper
    {
        string GenerateToken(User user, string userType);
        ClaimsPrincipal ValidateToken(string token);
        bool IsTokenExpired(string token);
        Guid? GetUserIdFromToken(string token);
        string GetUserTypeFromToken(string token);
        string GetEmailFromToken(string token);
    }
}