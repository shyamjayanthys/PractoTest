using PractoPrototypeAPI.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PractoPrototypeAPI.Repository.Repository
{
    public interface IAppointmentRepository
    {
        Task<AppointmentModel> CreateAppointment(AppointmentModel  appointment);
        Task<bool> UpdateAppointment(AppointmentModel appointment);
        Task<IEnumerable<AppointmentModel>> GetAppointmentDoctor(int doctorId);
        Task<AppointmentModel> GetAppointmentPatent(int patentId);
        Task<bool> DeleteAppointment(Guid appointmentId);
    }
}
