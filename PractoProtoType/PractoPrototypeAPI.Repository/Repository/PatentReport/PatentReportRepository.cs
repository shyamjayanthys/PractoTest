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
    public class PatentReportRepository : IPatentReportRepository
    {
        private readonly string dbConnectionString;
        private const string getPatentReport = @"SELECT patent_report_id AS PatentReportId, 
                                                        patent_record AS PatentRecord, 
                                                        patent_id AS PatentId
                                                 FROM patent_report ";
        public PatentReportRepository(IConfiguration configuration)
        {
            dbConnectionString = configuration[Constants.PostgreConnectionString];
            if (string.IsNullOrWhiteSpace(dbConnectionString))
            {
                throw new ArgumentNullException("PostgreConnectionString is nul or empty. please provide valid PostgreConnectionString");
            }
        }
        public async Task<PatentReportModel> CreatePatentReport(PatentReportModel patentReport)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                connection.Open();
                try
                {
                    var patentReportObject = new
                    {
                        patentReport.PatentId,
                        patentReport.PatentRecord
                    };
                    var query = "INSERT INTO patent_report (" +
                                                       "  patent_record," +
                                                       "  patent_id" +
                                                       " ) " +
                                                       "VALUES (" +
                                                       "  @PatentRecord, " +
                                                       "  @PatentId" +
                                                       ") RETURNING patent_report_id";
                    patentReport.PatentReportId = (int)await connection.ExecuteScalarAsync(query, patentReportObject);
                    return patentReport;
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

        public async Task<bool> DeletePatentReport(int patentReportId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                connection.Open();
                try
                {
                    string delete = "DELETE FROM patent_report WHERE patent_report_id = @patentReportId";
                    int result = await connection.ExecuteAsync(delete, new { patentReportId });
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

        public async Task<IEnumerable<PatentReportModel>> GetAllPatentReport()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    return await connection.QueryAsync<PatentReportModel>(getPatentReport);
                }
                catch
                {
                    throw;
                }
            }
        }

        public async Task<PatentReportModel> GetPatentReport(int patentReportId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    string query = getPatentReport + " WHERE patent_report_id = @patentReportId";
                    return (await connection.QueryAsync<PatentReportModel>(query, new { patentReportId })).SingleOrDefault();
                }
                catch
                {
                    throw;
                }
            }
        }

        public async Task<bool> UpdatePatentReport(PatentReportModel patentReport)
        {
            DateTime updatedTime = DateTime.UtcNow;

            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    var updateQuery = @"UPDATE patent_report 
                                        SET patent_record = @PatentRecord
                                        WHERE patent_report_id = @PatentReportId;";

                    await connection.ExecuteAsync(updateQuery, new { patentReport.PatentRecord, patentReport.PatentReportId });
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
