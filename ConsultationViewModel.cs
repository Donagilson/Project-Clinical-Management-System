using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicalManagementSystem2025.Models
{
    public class ConsultationViewModel
    {
        public int ConsultationId { get; set; }
        public DateTime ConsultationDate { get; set; }
        public int AppointmentId { get; set; }
        public Patient Patient { get; set; }
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Symptoms & Observations are required")]
        [StringLength(500, ErrorMessage = "Symptoms cannot exceed 500 characters")]
        [Display(Name = "Symptoms & Observations")]
        public string Symptoms { get; set; }

        [Required(ErrorMessage = "Diagnosis is required")]
        [StringLength(300, ErrorMessage = "Diagnosis cannot exceed 300 characters")]
        [Display(Name = "Diagnosis")]
        public string Diagnosis { get; set; }

        [StringLength(200, ErrorMessage = "Notes cannot exceed 200 characters")]
        [Display(Name = "Additional Notes")]
        public string Notes { get; set; }

        [ValidateMedicineList(ErrorMessage = "Medicine details are incomplete")]
        public List<MedicineItem> Medicines { get; set; } = new List<MedicineItem>();

        [ValidateLabTestList(ErrorMessage = "Lab test details are incomplete")]
        public List<LabTestItem> LabTests { get; set; } = new List<LabTestItem>();
    }

    public class MedicineItem
    {
        [Required(ErrorMessage = "Medicine selection is required")]
        [Display(Name = "Medicine")]
        public int MedicineId { get; set; }

        public string MedicineName { get; set; }

        [Required(ErrorMessage = "Dosage is required")]
        [StringLength(50, ErrorMessage = "Dosage cannot exceed 50 characters")]
        [Display(Name = "Dosage")]
        public string Dosage { get; set; }

        [Required(ErrorMessage = "Frequency is required")]
        [StringLength(50, ErrorMessage = "Frequency cannot exceed 50 characters")]
        [Display(Name = "Frequency")]
        public string Frequency { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [StringLength(50, ErrorMessage = "Duration cannot exceed 50 characters")]
        [Display(Name = "Duration")]
        public string Duration { get; set; }

        [StringLength(100, ErrorMessage = "Instructions cannot exceed 100 characters")]
        [Display(Name = "Instructions")]
        public string Instructions { get; set; }
    }

    public class LabTestItem
    {
        [Required(ErrorMessage = "Test name is required")]
        [StringLength(100, ErrorMessage = "Test name cannot exceed 100 characters")]
        [Display(Name = "Test Name")]
        public string TestName { get; set; }

        [Required(ErrorMessage = "Instructions are required")]
        [StringLength(200, ErrorMessage = "Instructions cannot exceed 200 characters")]
        [Display(Name = "Instructions")]
        public string Instructions { get; set; }
    }

    // Custom Validation for Medicine List
    public class ValidateMedicineListAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var medicines = value as List<MedicineItem>;

            if (medicines != null && medicines.Count > 0)
            {
                foreach (var medicine in medicines)
                {
                    if (medicine.MedicineId == 0 ||
                        string.IsNullOrWhiteSpace(medicine.Dosage) ||
                        string.IsNullOrWhiteSpace(medicine.Frequency) ||
                        string.IsNullOrWhiteSpace(medicine.Duration))
                    {
                        return new ValidationResult("Please complete all medicine details (Medicine, Dosage, Frequency, Duration)");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }

    // Custom Validation for Lab Test List
    public class ValidateLabTestListAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var labTests = value as List<LabTestItem>;

            if (labTests != null && labTests.Count > 0)
            {
                foreach (var test in labTests)
                {
                    if (string.IsNullOrWhiteSpace(test.TestName) ||
                        string.IsNullOrWhiteSpace(test.Instructions))
                    {
                        return new ValidationResult("Please complete all lab test details (Test Name and Instructions)");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}