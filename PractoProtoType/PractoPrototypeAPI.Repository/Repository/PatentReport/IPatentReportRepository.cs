using PractoPrototypeAPI.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PractoPrototypeAPI.Repository.Repository
{
    public interface IPatentReportRepository
    {
        Task<PatentReportModel> CreatePatentReport(PatentReportModel patentReport);
        Task<bool> UpdatePatentReport(PatentReportModel patentReport);
        Task<PatentReportModel> GetPatentReport(int patentReportId);
        Task<IEnumerable<PatentReportModel>> GetAllPatentReport();
        Task<bool> DeletePatentReport(int patentReportId);
    }
}
