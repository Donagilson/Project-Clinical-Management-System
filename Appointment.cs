using System;

namespace ClinicalManagementSystem2025.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string BloodGroup { get; set; }
        public string EmergencyContact { get; set; }
        public string Specialization { get; set; }
        public string DepartmentName { get; set; }
        public int Age => DateTime.Now.Year - DateOfBirth.Year;
    }
}