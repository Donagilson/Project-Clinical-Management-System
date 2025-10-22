using ClinicalManagementSystem2025.Models;
using ClinicalManagementSystem2025.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ClinicalManagementSystem2025.Controllers
{
    public class ReceptionistController : Controller
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger<ReceptionistController> _logger;

        public ReceptionistController(IPatientRepository patientRepository, ILogger<ReceptionistController> logger)
        {
            _patientRepository = patientRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddPatient(string? phone = null)
        {
            var patient = new Patient();
            if (!string.IsNullOrEmpty(phone))
            {
                patient.Phone = phone;
            }
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPatient(Patient patient)
        {
            try
            {
                // Basic validation
                if (string.IsNullOrWhiteSpace(patient.FirstName))
                {
                    ModelState.AddModelError("FirstName", "First Name is required");
                }
                if (string.IsNullOrWhiteSpace(patient.LastName))
                {
                    ModelState.AddModelError("LastName", "Last Name is required");
                }
                if (string.IsNullOrWhiteSpace(patient.Phone))
                {
                    ModelState.AddModelError("Phone", "Phone Number is required");
                }
                if (string.IsNullOrWhiteSpace(patient.Gender))
                {
                    ModelState.AddModelError("Gender", "Gender is required");
                }
                if (patient.DateOfBirth == default || patient.DateOfBirth > DateTime.Now)
                {
                    ModelState.AddModelError("DateOfBirth", "Please select a valid Date of Birth");
                }

                if (!ModelState.IsValid)
                {
                    return View(patient);
                }

                // Check if patient already exists
                var existingPatients = await _patientRepository.GetPatientsByPhoneAsync(patient.Phone);
                if (existingPatients.Any())
                {
                    ModelState.AddModelError("Phone", "A patient with this phone number already exists.");
                    return View(patient);
                }

                // Set registration date
                patient.RegistrationDate = DateTime.Now;
                patient.IsActive = true;

                // Add to database
                var patientId = await _patientRepository.AddPatientAsync(patient);

                if (patientId > 0)
                {
                    TempData["SuccessMessage"] = $"Patient {patient.FirstName} {patient.LastName} added successfully!";
                    return RedirectToAction("PatientDetails", new { id = patientId });
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to add patient. Please try again.";
                    return View(patient);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding patient");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(patient);
            }
        }

        [HttpGet]
        public async Task<IActionResult> PatientDetails(int id)
        {
            try
            {
                var patient = await _patientRepository.GetPatientByIdAsync(id);
                if (patient == null)
                {
                    TempData["ErrorMessage"] = "Patient not found";
                    return RedirectToAction("Index");
                }
                return View(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading patient details for ID: {id}");
                TempData["ErrorMessage"] = $"Error loading patient: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SearchPatient(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return Json(new { success = false, message = "Please enter a search term" });
                }

                var patients = await _patientRepository.SearchPatientsAsync(searchTerm);
                return Json(new
                {
                    success = true,
                    patients = patients.Select(p => new {
                        patientId = p.PatientId,
                        fullName = p.FullName,
                        phone = p.Phone,
                        email = p.Email ?? "N/A",
                        gender = p.Gender ?? "N/A",
                        age = p.Age
                    })
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching patients with term: {searchTerm}");
                return Json(new { success = false, message = $"Error searching patients: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPatientByPhone(string phone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                {
                    return Json(new { success = false, message = "Please enter a phone number" });
                }

                var patients = await _patientRepository.GetPatientsByPhoneAsync(phone);
                var patient = patients.FirstOrDefault();

                if (patient != null)
                {
                    return Json(new
                    {
                        success = true,
                        patient = new
                        {
                            patientId = patient.PatientId,
                            fullName = patient.FullName,
                            phone = patient.Phone,
                            email = patient.Email ?? "N/A",
                            gender = patient.Gender ?? "N/A",
                            age = patient.Age
                        }
                    });
                }

                return Json(new { success = false, message = "Patient not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting patient by phone: {phone}");
                return Json(new { success = false, message = $"Error searching patient: {ex.Message}" });
            }
        }
    }
}