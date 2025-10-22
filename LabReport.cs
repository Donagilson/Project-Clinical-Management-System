using System;

namespace ClinicalManagementSystem2025.Models
{
    public class LabReport
    {
        public int LabReportId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string TestName { get; set; }
        public DateTime TestDate { get; set; }
        public string Result { get; set; }
        public string Status { get; set; }
    }
}