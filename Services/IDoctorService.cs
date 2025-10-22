// Services/IDoctorService.cs
using ClinicalManagementSystem2025.Models;

namespace ClinicalManagementSystem2025.Services
{
    public interface IDoctorService
    {
        IEnumerable<Doctor> GetAvailableDoctors(int? departmentId = null);
        IEnumerable<Department> GetAllDepartments();
    }
}