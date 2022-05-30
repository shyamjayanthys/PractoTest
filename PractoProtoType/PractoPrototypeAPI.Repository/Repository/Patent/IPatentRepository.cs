using PractoPrototypeAPI.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PractoPrototypeAPI.Repository.Repository
{
    public interface IPatentRepository
    {
        Task<PatentModel> CreatePatent(PatentModel patent);
        Task<PatentModel> GetPatent(Guid patentId);
        Task<IEnumerable<PatentModel>> GetAllPatent();
        Task<PatentModel> GetPatent(int patentId);
    }
}
