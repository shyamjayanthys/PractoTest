using PractoPrototypeAPI.Model;
using System.Threading.Tasks;

namespace PractoPrototypeAPI.Logic
{
    public interface IPatenetLogic
    {
        Task<PatentViewModel> GetDoctorsList(int patentId);
        Task<bool> SubmitPatentAppointment(PatentAppointmenRequestViewModel patentAppointmen);
    }
}
