using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicalManagementSystem2025.Models;


namespace ClinicalManagementSystem2025.Repository
{
    public interface IDoctorRepository
    {
        Task<int> GetDoctorIdByUserId(int userId);
        Task<List<Appointment>> GetTodayAppointments(int doctorId);
        Task<List<Appointment>> GetWeeklyAppointments(int doctorId);
        Task<List<Patient>> GetAllPatients(int doctorId);
        Task<Patient> GetPatientDetails(int patientId);
        Task<Appointment> GetAppointmentById(int appointmentId);
        Task<bool> UpdateAppointmentStatus(int appointmentId, string status, string notes);
        Task<bool> SaveConsultation(Prescription prescription, List<PrescriptionDetail> medicines);
        Task<List<Medicine>> GetMedicines();
        Task<bool> CreateLabReport(LabReport labReport);
    }
}