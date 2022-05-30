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
    public class PatentRepository : IPatentRepository
    {
        private readonly string dbConnectionString;
        private const string getPatent = @"SELECT patent_id AS Id,
                                                  patent_guid AS PatentId, 
                                                  user_id AS UserId
                                               FROM patent ";
        public PatentRepository(IConfiguration configuration)
        {
            dbConnectionString = configuration[Constants.PostgreConnectionString];
            if (string.IsNullOrWhiteSpace(dbConnectionString))
            {
                throw new ArgumentNullException("PostgreConnectionString is nul or empty. please provide valid PostgreConnectionString");
            }
        }

        public async Task<PatentModel> CreatePatent(PatentModel patent)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                connection.Open();
                try
                {
                    patent.PatentId = Guid.NewGuid();
                    var patentObject = new
                    {
                        patent.PatentId,
                        patent.UserId
                    };
                    var query = "INSERT INTO patent (" +
                                                       "  patent_guid," +
                                                       "  user_id" +
                                                       " ) " +
                                                       "VALUES (" +
                                                       "  @PatentId, " +
                                                       "  @UserId " +
                                                       ") RETURNING patent_id";
                    patent.Id = (int) await connection.ExecuteScalarAsync(query, patentObject);
                    return patent;
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

        public async Task<bool> DeletePatent(Guid patentId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                connection.Open();
                try
                {
                    string delete = "DELETE FROM patent WHERE patent_guid = @patentId";
                    int result = await connection.ExecuteAsync(delete, new { patentId });
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

        public async Task<IEnumerable<PatentModel>> GetAllPatent()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    return await connection.QueryAsync<PatentModel>(getPatent);
                }
                catch
                {
                    throw;
                }
            }
        }

        public async Task<PatentModel> GetPatent(Guid patentId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    string query = getPatent + " WHERE patent_guid = @patentId";
                    return (await connection.QueryAsync<PatentModel>(query, new { patentId })).SingleOrDefault();
                }
                catch
                {
                    throw;
                }
            }
        }

        public async Task<PatentModel> GetPatent(int patentId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    string query = getPatent + " WHERE patent_id = @patentId";
                    return (await connection.QueryAsync<PatentModel>(query, new { patentId })).SingleOrDefault();
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
