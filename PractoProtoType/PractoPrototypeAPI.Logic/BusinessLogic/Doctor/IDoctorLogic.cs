using PractoPrototypeAPI.Model;
using System.Threading.Tasks;

namespace PractoPrototypeAPI.Logic
{
    public interface IDoctorLogic
    {
        Task<DoctorViewModel> GetAppointmentList(int doctorId);
        Task<bool> EditMyAppointment(DoctorAppointmentManagementViewModel doctorAppointmentManagement, string appointmentStatus);
    }
}
