using PractoPrototypeAPI.Model;
using PractoPrototypeAPI.Repository.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PractoPrototypeAPI.Logic
{
    public class PatenetLogic : IPatenetLogic
    {
        private readonly IUserRepository userRepository;
        private readonly IDoctorRepository doctorRepository;
        private readonly IAppointmentRepository appointmentRepository;
        public PatenetLogic(IUserRepository userRepository, IDoctorRepository doctorRepository,
                             IAppointmentRepository appointmentRepository)
        {
            this.userRepository = userRepository;
            this.doctorRepository = doctorRepository;
            this.appointmentRepository = appointmentRepository;
        }
        public async Task<PatentViewModel> GetDoctorsList(int patentId)
        {
            try
            {
                PatentViewModel patentViewModel = new PatentViewModel();
                if (patentId > 0)
                {
                    List<DoctorListViewModel> doctorLists = new List<DoctorListViewModel>();
                    var allDoctors = await doctorRepository.GetAllDoctor();
                    var users = await userRepository.GetAllUser();
                    foreach (var doctor in allDoctors)
                    {
                        doctorLists.Add(new DoctorListViewModel()
                        {
                            DoctorId = doctor.Id,
                            DoctorRating = doctor.DoctorRating,
                            DoctorSpec = doctor.DoctorSpec,
                            DoctorName = users.Where(x => x.Id == doctor.UserId).Select(x => x.Name).SingleOrDefault()
                        });
                    }
                    patentViewModel.doctorListViews = doctorLists;
                    patentViewModel.PatentId = patentId;
                }
                return patentViewModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SubmitPatentAppointment(PatentAppointmenRequestViewModel patentAppointment)
        {
            try
            {
                bool status = false;
                if (patentAppointment != null)
                {
                    AppointmentModel appointmentModel = new AppointmentModel()
                    {
                        DoctorId = patentAppointment.doctor.DoctorId,
                        PatentId = patentAppointment.PatentId,
                        AppointmentStatus = "Request",
                        AppointmentTime = patentAppointment.RequestAppointment
                    };
                    var CreateAppointment = await appointmentRepository.CreateAppointment(appointmentModel);
                    status = true;
                }
                return status;
            }
            catch
            {
                throw;
            }
        }
    }
}
