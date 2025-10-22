namespace ClinicalManagementSystem2025.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string Specialization { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public int Experience { get; set; }
        public int DepartmentId { get; set; }
        public decimal ConsultationFee { get; set; }
        public TimeSpan AvailableFrom { get; set; }
        public TimeSpan AvailableTo { get; set; }
        public bool IsActive { get; set; } = true;

        // Display property
        public string DoctorName { get; set; } = string.Empty;
    }
}