using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PractoPrototypeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PractoPrototypeAPI.Repository.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly string dbConnectionString;
        private const string getAppointmentQuery = @"SELECT appointment_guid AS AppointmentId, 
                                                      appointment_status AS AppointmentStatus, 
                                                      appointment_time AS AppointmentTime, 
                                                      patent_id AS PatentId, 
                                                      doctor_id AS DoctorId
                                               FROM appointment ";
        public AppointmentRepository(IConfiguration configuration)
        {
            dbConnectionString = configuration[Constants.PostgreConnectionString];
            if (string.IsNullOrWhiteSpace(dbConnectionString))
            {
                throw new ArgumentNullException("PostgreConnectionString is nul or empty. please provide valid PostgreConnectionString");
            }
        }
        public async Task<AppointmentModel> CreateAppointment(AppointmentModel appointment)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                connection.Open();
                try
                {
                    appointment.AppointmentId = Guid.NewGuid();

                    var appointmentObject = new
                    {
                        appointment.AppointmentId,
                        appointment.AppointmentStatus,
                        appointment.AppointmentTime,
                        appointment.DoctorId,
                        appointment.PatentId
                    };
                    var query = "INSERT INTO appointment (" +
                                                          "  appointment_guid," +
                                                          "  appointment_status," +
                                                          "  appointment_time," +
                                                          "  doctor_id," +
                                                          "  patent_id" +
                                                          " ) " +
                                                          "VALUES (" +
                                                          "  @AppointmentId, " +
                                                          "  @AppointmentStatus, " +
                                                          "  @AppointmentTime, " +
                                                          "  @DoctorId, " +
                                                          "  @PatentId" +
                                                          ") RETURNING appointment_id";
                    await connection.ExecuteScalarAsync(query, appointmentObject);
                    return appointment;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async Task<bool> DeleteAppointment(Guid appointmentId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                connection.Open();
                try
                {
                    string delete = "DELETE FROM appointment WHERE appointment_guid = @appointmentId";
                    int result = await connection.ExecuteAsync(delete, new { appointmentId });
                    return (result == 1);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async Task<IEnumerable<AppointmentModel>> GetAppointmentDoctor(int doctorId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    string query = getAppointmentQuery + " WHERE doctor_id = @doctorId";
                    return await connection.QueryAsync<AppointmentModel>(query, new { doctorId });
                }
                catch
                {
                    throw;
                }
            }
        }

        public async Task<AppointmentModel> GetAppointmentPatent(int patentId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    string query = getAppointmentQuery + " WHERE patent_id = @patentId";
                    return (await connection.QueryAsync<AppointmentModel>(query, new { patentId })).SingleOrDefault();
                }
                catch
                {
                    throw;
                }
            }
        }

        public async Task<bool> UpdateAppointment(AppointmentModel appointment)
        {
            DateTime updatedTime = DateTime.UtcNow;

            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    var updateQuery = @"UPDATE appointment 
                                        SET appointment_status = @AppointmentStatus, 
                                            appointment_time = @AppointmentTime 
                                        WHERE appointment_guid = @AppointmentId;";

                    await connection.ExecuteAsync(updateQuery,new {appointment.AppointmentStatus,
                                                                   appointment.AppointmentTime,
                                                                   appointment.AppointmentId});
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
