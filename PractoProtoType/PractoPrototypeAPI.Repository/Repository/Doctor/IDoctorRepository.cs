using PractoPrototypeAPI.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PractoPrototypeAPI.Repository.Repository
{
    public interface IDoctorRepository
    {
        Task<DoctorModel> CreateDoctor(DoctorModel doctor);
        Task<bool> UpdateDoctor(DoctorModel doctor);
        Task<DoctorModel> GetDoctor(Guid doctorId);
        Task<IEnumerable<DoctorModel>> GetAllDoctor();
        Task<bool> DeleteDoctor(Guid doctorId);
    }
}
