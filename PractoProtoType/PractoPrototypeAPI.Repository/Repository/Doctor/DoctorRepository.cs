using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PractoPrototypeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PractoPrototypeAPI.Repository.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly string dbConnectionString;
        private const string getDoctorsQuery = @"SELECT doctor_id AS Id,
                                                        doctor_guid AS DoctorId, 
                                                        doctor_spec AS DoctorSpec, 
                                                        user_id AS UserId, 
                                                        doctor_rating AS DoctorRating
                                                 FROM doctor ";
        public DoctorRepository(IConfiguration configuration)
        {
            dbConnectionString = configuration[Constants.PostgreConnectionString];
            if (string.IsNullOrWhiteSpace(dbConnectionString))
            {
                throw new ArgumentNullException("PostgreConnectionString is nul or empty. please provide valid PostgreConnectionString");
            }
        }
        public async Task<DoctorModel> CreateDoctor(DoctorModel doctor)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                connection.Open();
                try
                {
                    doctor.DoctorId = Guid.NewGuid();
                    var doctorObject = new
                    {
                        doctor.DoctorId,
                        doctor.DoctorSpec,
                        doctor.UserId,
                        doctor.DoctorRating
                    };
                    var query = "INSERT INTO doctor (" +
                                                     "  doctor_guid," +
                                                     "  doctor_spec," +
                                                     "  user_id," +
                                                     "  doctor_rating" +
                                                     " ) " +
                                                     "VALUES (" +
                                                     "  @DoctorId, " +
                                                     "  @DoctorSpec, " +
                                                     "  @UserId, " +
                                                     "  @DoctorRating " +
                                                     ") RETURNING doctor_id";
                    doctor.Id = (int)await connection.ExecuteScalarAsync(query, doctorObject);
                    return doctor;
                }
                catch(Exception ex)
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async Task<bool> DeleteDoctor(Guid doctorId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                connection.Open();
                try
                {
                    string delete = "DELETE FROM doctor WHERE doctor_guid = @doctorId";
                    int result = await connection.ExecuteAsync(delete, new { doctorId });
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

        public async Task<IEnumerable<DoctorModel>> GetAllDoctor()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    return await connection.QueryAsync<DoctorModel>(getDoctorsQuery);
                }
                catch
                {
                    throw;
                }
            }
        }

        public async Task<DoctorModel> GetDoctor(Guid doctorId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    string query = getDoctorsQuery + " WHERE doctor_guid = @doctorId";
                    return (await connection.QueryAsync<DoctorModel>(query, new { doctorId })).SingleOrDefault();
                }
                catch
                {
                    throw;
                }
            }
        }

        public async Task<bool> UpdateDoctor(DoctorModel doctor)
        {
            DateTime updatedTime = DateTime.UtcNow;

            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    var updateQuery = @"UPDATE doctor 
                                        SET doctor_spec = @DoctorSpec,                                             
                                            doctor_rating = @DoctorRating 
                                        WHERE doctor_guid = @DoctorId;";

                    await connection.ExecuteAsync(updateQuery, new {doctor.DoctorSpec,
                                                                    doctor.DoctorRating,
                                                                    doctor.DoctorId});
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
