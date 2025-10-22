using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ClinicalManagementSystem2025.Models;

namespace ClinicalManagementSystem2025.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly string _connectionString;

        public DoctorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MVCConnectionString");
        }

        public async Task<int> GetDoctorIdByUserId(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetDoctorIdByUserId", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@UserId", userId);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public async Task<List<Appointment>> GetTodayAppointments(int doctorId)
        {
            var appointments = new List<Appointment>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetTodayAppointmentsForDoctor", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@DoctorId", doctorId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                appointments.Add(new Appointment
                {
                    AppointmentId = reader.GetInt32("AppointmentId"),
                    AppointmentDate = reader.GetDateTime("AppointmentDate"),
                    AppointmentTime = reader.GetDateTime("AppointmentTime"),
                    Status = reader.GetString("Status"),
                    Reason = reader.IsDBNull("Reason") ? "" : reader.GetString("Reason"),
                    Notes = reader.IsDBNull("Notes") ? "" : reader.GetString("Notes"),
                    PatientId = reader.GetInt32("PatientId"),
                    PatientName = reader.GetString("PatientName"),
                    Gender = reader.GetString("Gender"),
                    DateOfBirth = reader.GetDateTime("DateOfBirth"),
                    Phone = reader.GetString("Phone"),
                    Email = reader.IsDBNull("Email") ? "" : reader.GetString("Email"),
                    Address = reader.IsDBNull("Address") ? "" : reader.GetString("Address"),
                    BloodGroup = reader.IsDBNull("BloodGroup") ? "" : reader.GetString("BloodGroup"),
                    EmergencyContact = reader.IsDBNull("EmergencyContact") ? "" : reader.GetString("EmergencyContact"),
                    Specialization = reader.GetString("Specialization"),
                    DepartmentName = reader.GetString("DepartmentName")
                });
            }

            return appointments;
        }

        public async Task<List<Appointment>> GetWeeklyAppointments(int doctorId)
        {
            var appointments = new List<Appointment>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetWeeklyAppointmentsForDoctor", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@DoctorId", doctorId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                appointments.Add(new Appointment
                {
                    AppointmentId = reader.GetInt32("AppointmentId"),
                    AppointmentDate = reader.GetDateTime("AppointmentDate"),
                    AppointmentTime = reader.GetDateTime("AppointmentTime"),
                    Status = reader.GetString("Status"),
                    Reason = reader.IsDBNull("Reason") ? "" : reader.GetString("Reason"),
                    Notes = reader.IsDBNull("Notes") ? "" : reader.GetString("Notes"),
                    PatientId = reader.GetInt32("PatientId"),
                    PatientName = reader.GetString("PatientName"),
                    Gender = reader.GetString("Gender"),
                    DateOfBirth = reader.GetDateTime("DateOfBirth"),
                    Phone = reader.GetString("Phone"),
                    Email = reader.IsDBNull("Email") ? "" : reader.GetString("Email"),
                    Specialization = reader.GetString("Specialization"),
                    DepartmentName = reader.GetString("DepartmentName")
                });
            }

            return appointments;
        }

        public async Task<List<Patient>> GetAllPatients(int doctorId)
        {
            var patients = new List<Patient>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetAllPatientsForDoctor", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@DoctorId", doctorId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                patients.Add(new Patient
                {
                    PatientId = reader.GetInt32("PatientId"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    Gender = reader.GetString("Gender"),
                    DateOfBirth = reader.GetDateTime("DateOfBirth"),
                    Phone = reader.GetString("Phone"),
                    Email = reader.IsDBNull("Email") ? "" : reader.GetString("Email"),
                    Address = reader.IsDBNull("Address") ? "" : reader.GetString("Address"),
                    BloodGroup = reader.IsDBNull("BloodGroup") ? "" : reader.GetString("BloodGroup"),
                    EmergencyContact = reader.IsDBNull("EmergencyContact") ? "" : reader.GetString("EmergencyContact"),
                    RegistrationDate = reader.GetDateTime("RegistrationDate")
                });
            }

            return patients;
        }

        public async Task<Patient> GetPatientDetails(int patientId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetPatientDetails", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@PatientId", patientId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Patient
                {
                    PatientId = reader.GetInt32("PatientId"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    Gender = reader.GetString("Gender"),
                    DateOfBirth = reader.GetDateTime("DateOfBirth"),
                    Phone = reader.GetString("Phone"),
                    Email = reader.IsDBNull("Email") ? "" : reader.GetString("Email"),
                    Address = reader.IsDBNull("Address") ? "" : reader.GetString("Address"),
                    BloodGroup = reader.IsDBNull("BloodGroup") ? "" : reader.GetString("BloodGroup"),
                    EmergencyContact = reader.IsDBNull("EmergencyContact") ? "" : reader.GetString("EmergencyContact"),
                    RegistrationDate = reader.GetDateTime("RegistrationDate")
                };
            }

            return null;
        }

        public async Task<Appointment> GetAppointmentById(int appointmentId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_GetAppointmentById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@AppointmentId", appointmentId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Appointment
                {
                    AppointmentId = reader.GetInt32("AppointmentId"),
                    AppointmentDate = reader.GetDateTime("AppointmentDate"),
                    AppointmentTime = reader.GetDateTime("AppointmentTime"),
                    Status = reader.GetString("Status"),
                    Reason = reader.IsDBNull("Reason") ? "" : reader.GetString("Reason"),
                    Notes = reader.IsDBNull("Notes") ? "" : reader.GetString("Notes"),
                    PatientId = reader.GetInt32("PatientId"),
                    PatientName = reader.GetString("PatientName"),
                    Gender = reader.GetString("Gender"),
                    DateOfBirth = reader.GetDateTime("DateOfBirth"),
                    Phone = reader.GetString("Phone"),
                    Email = reader.IsDBNull("Email") ? "" : reader.GetString("Email"),
                    Address = reader.IsDBNull("Address") ? "" : reader.GetString("Address"),
                    BloodGroup = reader.IsDBNull("BloodGroup") ? "" : reader.GetString("BloodGroup"),
                    EmergencyContact = reader.IsDBNull("EmergencyContact") ? "" : reader.GetString("EmergencyContact"),
                    Specialization = reader.GetString("Specialization"),
                    DepartmentName = reader.GetString("DepartmentName")
                };
            }

            return null;
        }

        public async Task<bool> UpdateAppointmentStatus(int appointmentId, string status, string notes)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("sp_UpdateAppointmentStatus", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@AppointmentId", appointmentId);
            command.Parameters.AddWithValue("@Status", status);
            command.Parameters.AddWithValue("@Notes", notes ?? "");

            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            return result > 0;
        }

        public async Task<bool> SaveConsultation(Prescription prescription, List<PrescriptionDetail> medicines)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Insert prescription
                var prescriptionCommand = new SqlCommand(
                    @"INSERT INTO TblPrescriptions (AppointmentId, PatientId, DoctorId, Diagnosis, Notes, PrescriptionDate)
                      VALUES (@AppointmentId, @PatientId, @DoctorId, @Diagnosis, @Notes, GETDATE());
                      SELECT SCOPE_IDENTITY();",
                    connection,
                    transaction
                );

                prescriptionCommand.Parameters.AddWithValue("@AppointmentId", prescription.AppointmentId);
                prescriptionCommand.Parameters.AddWithValue("@PatientId", prescription.PatientId);
                prescriptionCommand.Parameters.AddWithValue("@DoctorId", prescription.DoctorId);
                prescriptionCommand.Parameters.AddWithValue("@Diagnosis", prescription.Diagnosis);
                prescriptionCommand.Parameters.AddWithValue("@Notes", prescription.Notes ?? "");

                var prescriptionId = Convert.ToInt32(await prescriptionCommand.ExecuteScalarAsync());

                // Insert prescription details
                foreach (var medicine in medicines)
                {
                    var detailCommand = new SqlCommand(
                        @"INSERT INTO TblPrescriptionDetails (PrescriptionId, MedicineId, Dosage, Frequency, Duration)
                          VALUES (@PrescriptionId, @MedicineId, @Dosage, @Frequency, @Duration)",
                        connection,
                        transaction
                    );

                    detailCommand.Parameters.AddWithValue("@PrescriptionId", prescriptionId);
                    detailCommand.Parameters.AddWithValue("@MedicineId", medicine.MedicineId);
                    detailCommand.Parameters.AddWithValue("@Dosage", medicine.Dosage);
                    detailCommand.Parameters.AddWithValue("@Frequency", medicine.Frequency);
                    detailCommand.Parameters.AddWithValue("@Duration", medicine.Duration);

                    await detailCommand.ExecuteNonQueryAsync();
                }

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

        public async Task<List<Medicine>> GetMedicines()
        {
            var medicines = new List<Medicine>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "SELECT MedicineId, MedicineName, Manufacturer, UnitPrice, StockQuantity FROM TblMedicines WHERE IsActive = 1 AND StockQuantity > 0",
                connection
            );

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                medicines.Add(new Medicine
                {
                    MedicineId = reader.GetInt32("MedicineId"),
                    MedicineName = reader.GetString("MedicineName"),
                    Manufacturer = reader.IsDBNull("Manufacturer") ? "" : reader.GetString("Manufacturer"),
                    UnitPrice = reader.GetDecimal("UnitPrice"),
                    StockQuantity = reader.GetInt32("StockQuantity")
                });
            }

            return medicines;
        }

        public async Task<bool> CreateLabReport(LabReport labReport)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                @"INSERT INTO TblLabReports (PatientId, DoctorId, TestName, TestDate, Result, Status)
                  VALUES (@PatientId, @DoctorId, @TestName, GETDATE(), @Result, @Status)",
                connection
            );

            command.Parameters.AddWithValue("@PatientId", labReport.PatientId);
            command.Parameters.AddWithValue("@DoctorId", labReport.DoctorId);
            command.Parameters.AddWithValue("@TestName", labReport.TestName);
            command.Parameters.AddWithValue("@Result", labReport.Result ?? "");
            command.Parameters.AddWithValue("@Status", labReport.Status);

            await connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            return result > 0;
        }
    }
}