using ClinicalManagementSystem2025.Models;

namespace ClinicalManagementSystem2025.Repository
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAvailableDoctorsAsync(int? departmentId = null);
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<Doctor?> GetDoctorByIdAsync(int doctorId);
        IEnumerable<Doctor> GetAvailableDoctors(int? departmentId);
        IEnumerable<Department> GetAllDepartments();
        Task<List<object>?> GetDoctorsByDepartmentAsync(int departmentId);
    }
}