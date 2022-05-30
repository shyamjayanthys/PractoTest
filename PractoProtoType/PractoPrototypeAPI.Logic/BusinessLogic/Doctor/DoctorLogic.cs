using PractoPrototypeAPI.Model;
using PractoPrototypeAPI.Repository.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PractoPrototypeAPI.Logic
{
    public class DoctorLogic : IDoctorLogic
    {
        private readonly IUserRepository userRepository;
        private readonly IPatentRepository patentRepository;
        private readonly IDoctorRepository doctorRepository;
        private readonly IAppointmentRepository appointmentRepository;
        private readonly INotificationRepository notificationRepository;

        public DoctorLogic(IUserRepository userRepository, IDoctorRepository doctorRepository,
                           IAppointmentRepository appointmentRepository, IPatentRepository patentRepository,
                           INotificationRepository notificationRepository)
        {
            this.userRepository = userRepository;
            this.doctorRepository = doctorRepository;
            this.appointmentRepository = appointmentRepository;
            this.patentRepository = patentRepository;
            this.notificationRepository = notificationRepository;
        }

        public async Task<bool> EditMyAppointment(DoctorAppointmentManagementViewModel doctorAppointmentManagement,
            string appointmentStatus)
        {
            try
            {
                bool status = false;
                if (doctorAppointmentManagement != null)
                {
                    AppointmentModel appointmentModel = new AppointmentModel()
                    {
                        DoctorId = doctorAppointmentManagement.DoctorId,
                        PatentId = doctorAppointmentManagement.patents.PatentId,
                        AppointmentStatus = appointmentStatus,
                        AppointmentTime = doctorAppointmentManagement.patents.AppointmentTime
                    };
                    var CreateAppointment = await appointmentRepository.UpdateAppointment(appointmentModel);
                    var patent = await patentRepository.GetPatent(doctorAppointmentManagement.patents.PatentId);
                    var getUser = await userRepository.GetUser(patent.UserId);
                    NotificationModel notificationModel = new NotificationModel()
                    {
                        NotificationMessage = appointmentStatus,
                        NotificationStatus = "Sent",
                        NotificationTo = getUser.EmailId,
                        NotificationType = "mail"
                    };
                    await notificationRepository.CreateNotification(notificationModel);
                    status = true;
                }
                return status;
            }
            catch
            {
                throw;
            }
        }

        public async Task<DoctorViewModel> GetAppointmentList(int doctorId)
        {
            try
            {
                DoctorViewModel doctorViewModel = new DoctorViewModel();
                if (doctorId > 0)
                {
                    List<PatentListViewModel> patentList = new List<PatentListViewModel>();
                    var allAppointments = await appointmentRepository.GetAppointmentDoctor(doctorId);
                    var users = await userRepository.GetAllUser();
                    var getAllPatent = await patentRepository.GetAllPatent();
                    foreach (var appointment in allAppointments)
                    {
                        var userId = getAllPatent.Where(x => x.Id == appointment.PatentId).Select(x => x.UserId).SingleOrDefault();

                        patentList.Add(new PatentListViewModel()
                        {
                            PatentId = appointment.PatentId,
                            PatentName = users.Where(x => x.Id == userId).Select(x => x.Name).SingleOrDefault(),
                            AppointmentTime = appointment.AppointmentTime
                        });
                    }
                    doctorViewModel.patents = patentList;
                    doctorViewModel.DoctorId = doctorId;
                }
                return doctorViewModel;
            }
            catch
            {
                throw;
            }
        }
    }
}
