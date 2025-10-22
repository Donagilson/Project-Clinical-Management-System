using System;

namespace ClinicalManagementSystem2025.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatientName => $"{FirstName} {LastName}";
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        // CORRECTED Age Calculation
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;

                // Subtract a year if the birthday hasn't occurred yet this year
                if (DateOfBirth.Date > today.AddYears(-age))
                    age--;

                return age;
            }
        }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string BloodGroup { get; set; }
        public string EmergencyContact { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}