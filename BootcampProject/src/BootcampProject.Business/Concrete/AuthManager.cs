using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using BootcampProject.Business.Abstract;
using BootcampProject.Business.DTOs.Auth;
using BootcampProject.Business.BusinessRules;
using BootcampProject.Entities.Concrete;
using BootcampProject.Repositories.Abstract;
using BootcampProject.Core.Security;

namespace BootcampProject.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly ApplicantBusinessRules _applicantBusinessRules;
        private readonly IJwtHelper _jwtHelper;

        public AuthManager(
            IApplicantRepository applicantRepository, 
            IInstructorRepository instructorRepository, 
            IEmployeeRepository employeeRepository,
            IMapper mapper,
            ApplicantBusinessRules applicantBusinessRules,
            IJwtHelper jwtHelper)
        {
            _applicantRepository = applicantRepository;
            _instructorRepository = instructorRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _applicantBusinessRules = applicantBusinessRules;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Email and password are required.",
                        UserId = null,
                        Email = string.Empty,
                        FirstName = string.Empty,
                        LastName = string.Empty,
                        UserType = string.Empty,
                        Token = null
                    };
                }

                // Check Applicants
                var applicants = await _applicantRepository.GetAllAsync();
                var applicant = applicants.FirstOrDefault(a => a.Email.ToLower() == request.Email.ToLower());
                if (applicant != null && VerifyPassword(request.Password, applicant.PasswordHash, applicant.PasswordSalt))
                {
                    return new AuthResponseDto
                    {
                        Success = true,
                        Message = "Login successful.",
                        UserId = applicant.Id,
                        Email = applicant.Email,
                        FirstName = applicant.FirstName,
                        LastName = applicant.LastName,
                        UserType = "Applicant",
                        Token = _jwtHelper.GenerateToken(applicant, "Applicant"),
                        ExpiresAt = DateTime.UtcNow.AddHours(24)
                    };
                }

                // Check Instructors
                var instructors = await _instructorRepository.GetAllAsync();
                var instructor = instructors.FirstOrDefault(i => i.Email.ToLower() == request.Email.ToLower());
                if (instructor != null && VerifyPassword(request.Password, instructor.PasswordHash, instructor.PasswordSalt))
                {
                    return new AuthResponseDto
                    {
                        Success = true,
                        Message = "Login successful.",
                        UserId = instructor.Id,
                        Email = instructor.Email,
                        FirstName = instructor.FirstName,
                        LastName = instructor.LastName,
                        UserType = "Instructor",
                        Token = _jwtHelper.GenerateToken(instructor, "Instructor"),
                        ExpiresAt = DateTime.UtcNow.AddHours(24)
                    };
                }

                // Check Employees
                var employees = await _employeeRepository.GetAllAsync();
                var employee = employees.FirstOrDefault(e => e.Email.ToLower() == request.Email.ToLower());
                if (employee != null && VerifyPassword(request.Password, employee.PasswordHash, employee.PasswordSalt))
                {
                    return new AuthResponseDto
                    {
                        Success = true,
                        Message = "Login successful.",
                        UserId = employee.Id,
                        Email = employee.Email,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        UserType = "Employee",
                        Token = _jwtHelper.GenerateToken(employee, "Employee"),
                        ExpiresAt = DateTime.UtcNow.AddHours(24)
                    };
                }

                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password.",
                    UserId = null,
                    Email = string.Empty,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    UserType = string.Empty,
                    Token = null
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = $"Login failed: {ex.Message}",
                    UserId = null,
                    Email = string.Empty,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    UserType = string.Empty,
                    Token = null
                };
            }
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password) ||
                    string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName) ||
                    string.IsNullOrWhiteSpace(request.UserType) || string.IsNullOrWhiteSpace(request.NationalityIdentity))
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "All required fields must be provided.",
                        UserId = null,
                        Email = string.Empty,
                        FirstName = string.Empty,
                        LastName = string.Empty,
                        UserType = string.Empty,
                        Token = null
                    };
                }

                // Check email uniqueness across all user types
                await CheckEmailUniquenessAsync(request.Email);

                var userType = request.UserType.ToLower();
                switch (userType)
                {
                    case "applicant":
                        return await RegisterApplicantAsync(request);

                    case "instructor":
                        return await RegisterInstructorAsync(request);

                    case "employee":
                        return await RegisterEmployeeAsync(request);

                    default:
                        return new AuthResponseDto
                        {
                            Success = false,
                            Message = "Invalid user type. Must be 'Applicant', 'Instructor', or 'Employee'.",
                            UserId = null,
                            Email = string.Empty,
                            FirstName = string.Empty,
                            LastName = string.Empty,
                            UserType = string.Empty,
                            Token = null
                        };
                }
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = $"Registration failed: {ex.Message}",
                    UserId = null,
                    Email = string.Empty,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    UserType = string.Empty,
                    Token = null
                };
            }
        }

        private async Task<AuthResponseDto> RegisterApplicantAsync(RegisterRequestDto request)
        {
            // Check business rules for applicants
            await _applicantBusinessRules.CheckIfNationalityIdentityNotDuplicateAsync(request.NationalityIdentity);

            var salt = GenerateSalt();
            var hashedPassword = HashPassword(request.Password, salt);

            var applicant = new Applicant
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                NationalityIdentity = request.NationalityIdentity,
                Email = request.Email,
                PasswordHash = hashedPassword,
                PasswordSalt = salt,
                About = request.About ?? string.Empty
            };

            await _applicantRepository.AddAsync(applicant);
            await _applicantRepository.SaveChangesAsync();

            return new AuthResponseDto
            {
                Success = true,
                Message = "Applicant registration successful.",
                UserId = applicant.Id,
                Email = applicant.Email,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                UserType = "Applicant",
                Token = _jwtHelper.GenerateToken(applicant, "Applicant"),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        private async Task<AuthResponseDto> RegisterInstructorAsync(RegisterRequestDto request)
        {
            // Check nationality identity uniqueness for instructors
            var instructors = await _instructorRepository.GetAllAsync();
            var duplicateInstructor = instructors.FirstOrDefault(i => i.NationalityIdentity == request.NationalityIdentity);
            if (duplicateInstructor != null)
            {
                throw new InvalidOperationException($"An instructor with nationality identity {request.NationalityIdentity} already exists.");
            }

            var salt = GenerateSalt();
            var hashedPassword = HashPassword(request.Password, salt);

            var instructor = new Instructor
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                NationalityIdentity = request.NationalityIdentity,
                Email = request.Email,
                PasswordHash = hashedPassword,
                PasswordSalt = salt,
                CompanyName = request.CompanyName ?? string.Empty
            };

            await _instructorRepository.AddAsync(instructor);
            await _instructorRepository.SaveChangesAsync();

            return new AuthResponseDto
            {
                Success = true,
                Message = "Instructor registration successful.",
                UserId = instructor.Id,
                Email = instructor.Email,
                FirstName = instructor.FirstName,
                LastName = instructor.LastName,
                UserType = "Instructor",
                Token = _jwtHelper.GenerateToken(instructor, "Instructor"),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        private async Task<AuthResponseDto> RegisterEmployeeAsync(RegisterRequestDto request)
        {
            // Check nationality identity uniqueness for employees
            var employees = await _employeeRepository.GetAllAsync();
            var duplicateEmployee = employees.FirstOrDefault(e => e.NationalityIdentity == request.NationalityIdentity);
            if (duplicateEmployee != null)
            {
                throw new InvalidOperationException($"An employee with nationality identity {request.NationalityIdentity} already exists.");
            }

            var salt = GenerateSalt();
            var hashedPassword = HashPassword(request.Password, salt);

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                NationalityIdentity = request.NationalityIdentity,
                Email = request.Email,
                PasswordHash = hashedPassword,
                PasswordSalt = salt,
                Position = request.Position ?? string.Empty
            };

            await _employeeRepository.AddAsync(employee);
            await _employeeRepository.SaveChangesAsync();

            return new AuthResponseDto
            {
                Success = true,
                Message = "Employee registration successful.",
                UserId = employee.Id,
                Email = employee.Email,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                UserType = "Employee",
                Token = _jwtHelper.GenerateToken(employee, "Employee"),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        private async Task CheckEmailUniquenessAsync(string email)
        {
            // Check applicants
            var applicants = await _applicantRepository.GetAllAsync();
            if (applicants.Any(a => a.Email.ToLower() == email.ToLower()))
            {
                throw new InvalidOperationException($"An account with email {email} already exists.");
            }

            // Check instructors
            var instructors = await _instructorRepository.GetAllAsync();
            if (instructors.Any(i => i.Email.ToLower() == email.ToLower()))
            {
                throw new InvalidOperationException($"An account with email {email} already exists.");
            }

            // Check employees
            var employees = await _employeeRepository.GetAllAsync();
            if (employees.Any(e => e.Email.ToLower() == email.ToLower()))
            {
                throw new InvalidOperationException($"An account with email {email} already exists.");
            }
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + salt;
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private string GenerateSalt()
        {
            var saltBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private bool VerifyPassword(string password, string hash, string salt)
        {
            var computedHash = HashPassword(password, salt);
            return computedHash == hash;
        }

    }
}