using System.Data;
using System.Data.SqlClient;
using ClinicalManagementSystem2025.Models;
using Microsoft.Extensions.Configuration;

namespace ClinicalManagementSystem2025.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly string _connectionString;

        public DoctorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ClinicalConnectionString") ??
                              configuration.GetConnectionString("DefaultConnection") ??
                              configuration.GetConnectionString("MVCConnectionString") ??
                              throw new ArgumentNullException("Connection string not found");
        }

        public async Task<IEnumerable<Doctor>> GetAvailableDoctorsAsync(int? departmentId = null)
        {
            var doctors = new List<Doctor>();
            using var connection = new SqlConnection(_connectionString);

            var sql = @"
                SELECT d.DoctorId, u.FirstName + ' ' + u.LastName as DoctorName, 
                       d.Specialization, d.Qualification, dep.DepartmentName,
                       d.ConsultationFee, d.AvailableFrom, d.AvailableTo
                FROM Doctors d
                INNER JOIN Users u ON d.UserId = u.UserId
                INNER JOIN Departments dep ON d.DepartmentId = dep.DepartmentId
                WHERE d.IsActive = 1 AND u.IsActive = 1";

            if (departmentId.HasValue)
            {
                sql += " AND d.DepartmentId = @DepartmentId";
            }

            sql += " ORDER BY d.Specialization, DoctorName";

            using var command = new SqlCommand(sql, connection);
            if (departmentId.HasValue)
            {
                command.Parameters.AddWithValue("@DepartmentId", departmentId.Value);
            }

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                doctors.Add(new Doctor
                {
                    DoctorId = reader.GetInt32("DoctorId"),
                    DoctorName = reader.GetString("DoctorName"),
                    Specialization = reader.GetString("Specialization"),
                    Qualification = reader.IsDBNull(reader.GetOrdinal("Qualification")) ? null : reader.GetString("Qualification"),
                   // DepartmentName = reader.GetString("DepartmentName"),
                    ConsultationFee = reader.GetDecimal("ConsultationFee"),
                    //AvailableFrom = reader.IsDBNull(reader.GetOrdinal("AvailableFrom")) ? null : reader.GetTimeSpan("AvailableFrom"),
                    //AvailableTo = reader.IsDBNull(reader.GetOrdinal("AvailableTo")) ? null : reader.GetTimeSpan("AvailableTo")
                });
            }

            return doctors;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            var departments = new List<Department>();
            using var connection = new SqlConnection(_connectionString);

            const string sql = @"
                SELECT DepartmentId, DepartmentName, Description, IsActive
                FROM Departments 
                WHERE IsActive = 1
                ORDER BY DepartmentName";

            using var command = new SqlCommand(sql, connection);
            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                departments.Add(new Department
                {
                    DepartmentId = reader.GetInt32("DepartmentId"),
                    DepartmentName = reader.GetString("DepartmentName"),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description"),
                    IsActive = reader.GetBoolean("IsActive")
                });
            }

            return departments;
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int doctorId)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"
                SELECT d.DoctorId, u.FirstName + ' ' + u.LastName as DoctorName, 
                       d.Specialization, d.Qualification, dep.DepartmentName,
                       d.ConsultationFee, d.AvailableFrom, d.AvailableTo
                FROM Doctors d
                INNER JOIN Users u ON d.UserId = u.UserId
                INNER JOIN Departments dep ON d.DepartmentId = dep.DepartmentId
                WHERE d.DoctorId = @DoctorId AND d.IsActive = 1";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@DoctorId", doctorId);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Doctor
                {
                    DoctorId = reader.GetInt32("DoctorId"),
                    DoctorName = reader.GetString("DoctorName"),
                    Specialization = reader.GetString("Specialization"),
                    Qualification = reader.IsDBNull(reader.GetOrdinal("Qualification")) ? null : reader.GetString("Qualification"),
                   // DepartmentName = reader.GetString("DepartmentName"),
                    ConsultationFee = reader.GetDecimal("ConsultationFee"),
                   // AvailableFrom = reader.IsDBNull(reader.GetOrdinal("AvailableFrom")) ? null : reader.GetTimeSpan("AvailableFrom"),
                    //AvailableTo = reader.IsDBNull(reader.GetOrdinal("AvailableTo")) ? null : reader.GetTimeSpan("AvailableTo")
                };
            }

            return null;
        }

        public IEnumerable<Doctor> GetAvailableDoctors(int? departmentId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Department> GetAllDepartments()
        {
            throw new NotImplementedException();
        }

        public Task<List<object>?> GetDoctorsByDepartmentAsync(int departmentId)
        {
            throw new NotImplementedException();
        }
    }
}