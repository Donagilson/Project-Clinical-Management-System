using System;

namespace ClinicalManagementSystem2025.Models
{
    public class MedicalHistory
    {
        public int RecordId { get; set; }
        public int PatientId { get; set; }
        public DateTime RecordDate { get; set; }
        public string RecordType { get; set; }
        public string Description { get; set; }
        public string Diagnosis { get; set; }
        public string DoctorName { get; set; }
        public string Notes { get; set; }
        public string TestName { get; set; }
        public string Result { get; set; }
        public string Status { get; set; }
    }
}