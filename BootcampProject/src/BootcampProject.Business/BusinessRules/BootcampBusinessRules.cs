using System.Linq;
using BootcampProject.Repositories.Abstract;
using BootcampProject.Entities.Concrete;
using BootcampProject.Entities.Enums;

namespace BootcampProject.Business.BusinessRules
{
    public class BootcampBusinessRules
    {
        private readonly IBootcampRepository _bootcampRepository;
        private readonly IInstructorRepository _instructorRepository;

        public BootcampBusinessRules(IBootcampRepository bootcampRepository, IInstructorRepository instructorRepository)
        {
            _bootcampRepository = bootcampRepository;
            _instructorRepository = instructorRepository;
        }

        public async Task CheckIfBootcampExistsAsync(Guid bootcampId)
        {
            var bootcamp = await _bootcampRepository.GetByIdAsync(bootcampId);
            if (bootcamp == null)
            {
                throw new InvalidOperationException($"Bootcamp with ID {bootcampId} does not exist.");
            }
        }

        public async Task CheckIfBootcampNameNotDuplicateAsync(string name)
        {
            var bootcamps = await _bootcampRepository.GetAllAsync();
            var existingBootcamp = bootcamps.FirstOrDefault(b => b.Name == name);
            
            if (existingBootcamp != null)
            {
                throw new InvalidOperationException($"A bootcamp with name '{name}' already exists.");
            }
        }

        public async Task CheckIfStartDateBeforeEndDateAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
            {
                throw new InvalidOperationException("Start date must be before end date.");
            }
            await Task.CompletedTask;
        }

        public async Task CheckIfInstructorExistsAsync(Guid instructorId)
        {
            var instructor = await _instructorRepository.GetByIdAsync(instructorId);
            if (instructor == null)
            {
                throw new InvalidOperationException($"Instructor with ID {instructorId} does not exist.");
            }
        }

        public async Task CheckIfBootcampOpenForApplicationAsync(Guid bootcampId)
        {
            var bootcamp = await _bootcampRepository.GetByIdAsync(bootcampId);
            if (bootcamp == null)
            {
                throw new InvalidOperationException($"Bootcamp with ID {bootcampId} does not exist.");
            }

            if (bootcamp.BootcampState != BootcampState.OPEN_FOR_APPLICATION)
            {
                throw new InvalidOperationException($"Bootcamp '{bootcamp.Name}' is not open for applications. Current state: {bootcamp.BootcampState}");
            }
        }
    }
}