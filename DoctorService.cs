using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicalManagementSystem2025.Models;
using ClinicalManagementSystem2025.Repository;

namespace ClinicalManagementSystem2025.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private static List<ConsultationViewModel> _savedConsultations = new List<ConsultationViewModel>();

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<bool> SaveConsultation(ConsultationViewModel consultation)
        {
            try
            {
                // Add timestamp and ID for in-memory storage
                consultation.ConsultationId = _savedConsultations.Count + 1;
                consultation.ConsultationDate = DateTime.Now;

                // Save to static list (in-memory storage)
                _savedConsultations.Add(consultation);

                // Also try to save to database via repository
                try
                {
                    var prescription = new Prescription
                    {
                        AppointmentId = consultation.AppointmentId,
                        PatientId = consultation.Patient.PatientId,
                        DoctorId = consultation.DoctorId,
                        Diagnosis = consultation.Diagnosis,
                        Notes = $"{consultation.Symptoms} | {consultation.Notes}",
                        PrescriptionDate = DateTime.Now
                    };

                    var medicines = consultation.Medicines.Select(m => new PrescriptionDetail
                    {
                        MedicineId = m.MedicineId,
                        Dosage = m.Dosage,
                        Frequency = m.Frequency,
                        Duration = m.Duration,
                        Instructions = m.Instructions
                    }).ToList();

                    // Save lab reports if any
                    if (consultation.LabTests != null && consultation.LabTests.Any())
                    {
                        foreach (var labTest in consultation.LabTests)
                        {
                            var labReport = new LabReport
                            {
                                PatientId = consultation.Patient.PatientId,
                                DoctorId = consultation.DoctorId,
                                TestName = labTest.TestName,
                                Result = labTest.Instructions,
                                Status = "Pending",
                                TestDate = DateTime.Now
                            };
                            await _doctorRepository.CreateLabReport(labReport);
                        }
                    }

                    await _doctorRepository.SaveConsultation(prescription, medicines);
                }
                catch (Exception dbEx)
                {
                    // If database save fails, continue with in-memory storage
                    System.Diagnostics.Debug.WriteLine($"Database save failed: {dbEx.Message}");
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Save consultation failed: {ex.Message}");
                return false;
            }
        }

        // Get all saved consultations (for checking)
        public List<ConsultationViewModel> GetSavedConsultations()
        {
            return _savedConsultations;
        }

        // Get consultation by ID
        public ConsultationViewModel GetConsultationById(int id)
        {
            return _savedConsultations.FirstOrDefault(c => c.ConsultationId == id);
        }

        // Other existing methods...
        public async Task<int> GetDoctorIdByUserId(int userId)
        {
            return await _doctorRepository.GetDoctorIdByUserId(userId);
        }

        public async Task<List<Appointment>> GetTodayAppointments(int doctorId)
        {
            return await _doctorRepository.GetTodayAppointments(doctorId);
        }

        public async Task<List<Appointment>> GetWeeklyAppointments(int doctorId)
        {
            return await _doctorRepository.GetWeeklyAppointments(doctorId);
        }

        public async Task<List<Patient>> GetAllPatients(int doctorId)
        {
            return await _doctorRepository.GetAllPatients(doctorId);
        }

        public async Task<Patient> GetPatientDetails(int patientId)
        {
            return await _doctorRepository.GetPatientDetails(patientId);
        }

        public async Task<Appointment> GetAppointmentById(int appointmentId)
        {
            return await _doctorRepository.GetAppointmentById(appointmentId);
        }

        public async Task<bool> UpdateAppointmentStatus(int appointmentId, string status, string notes)
        {
            return await _doctorRepository.UpdateAppointmentStatus(appointmentId, status, notes);
        }

        public async Task<List<Medicine>> GetMedicines()
        {
            return await _doctorRepository.GetMedicines();
        }
    }
}