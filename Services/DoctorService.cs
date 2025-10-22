// Services/DoctorService.cs
using ClinicalManagementSystem2025.Models;
using ClinicalManagementSystem2025.Repository;

namespace ClinicalManagementSystem2025.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public IEnumerable<Doctor> GetAvailableDoctors(int? departmentId = null)
        {
            return _doctorRepository.GetAvailableDoctors(departmentId);
        }

        public IEnumerable<Department> GetAllDepartments()
        {
            return _doctorRepository.GetAllDepartments();
        }
    }
}