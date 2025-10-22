using System;

namespace ClinicalManagementSystem2025.Models
{
    public class Prescription
    {
        public int PrescriptionId { get; set; }
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string Diagnosis { get; set; }
        public string Notes { get; set; }
        public DateTime PrescriptionDate { get; set; }
    }
}