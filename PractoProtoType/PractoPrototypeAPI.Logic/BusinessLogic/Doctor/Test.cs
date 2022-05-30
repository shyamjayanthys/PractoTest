using PractoPrototypeAPI.Model;
using PractoPrototypeAPI.Repository.Repository;
using System;
using System.Threading.Tasks;

namespace PractoPrototypeAPI.Logic.BusinessLogic.Doctor
{
    public class Test : ITest
    {
        private readonly IUserRepository userRepository;
        private readonly IDoctorRepository doctorRepository;
        private readonly IPatentRepository patentRepository;
        private readonly IPatentReportRepository patentReportRepository;
        private readonly IAppointmentRepository appointmentRepository;
        private readonly INotificationRepository notificationRepository;
        public Test(IUserRepository userRepository, IDoctorRepository doctorRepository, 
            IPatentRepository patentRepository,IPatentReportRepository patentReportRepository,IAppointmentRepository appointmentRepository
            , INotificationRepository notificationRepository)
        {
            this.userRepository = userRepository;
            this.doctorRepository = doctorRepository;
            this.patentRepository = patentRepository;
            this.patentReportRepository = patentReportRepository;
            this.appointmentRepository = appointmentRepository;
            this.notificationRepository = notificationRepository;
        }
        public async Task<bool> meth()
        {
            bool status = false;
           
            try
            {
                await UserCreateTest();
               
                
                
                status = true;
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                throw;
            }
            return status;
        }

        private async Task UserCreateTest()
        {
            UserModel userModel1 = new UserModel()
            {
                UserName = "Doctor1",
                Name = "Doctor1",
                EmailId = "Doctor1@gmail.com"
            };
            UserModel userModel2 = new UserModel()
            {
                UserName = "Doctor2",
                Name = "Doctor2",
                EmailId = "Doctor2@gmail.com"
            };
            UserModel userModel3 = new UserModel()
            {
                UserName = "Doctor3",
                Name = "Doctor3",
                EmailId = "Doctor3@gmail.com"
            };
            var userDoctor1 = await userRepository.CreateUser(userModel1, "admin");
            var userDoctor2 = await userRepository.CreateUser(userModel2, "admin");
            var userDoctor3 = await userRepository.CreateUser(userModel3, "admin");

            DoctorModel doctorModel = new DoctorModel()
            {
                UserId = userDoctor1.Id,
                DoctorRating = 3,
                DoctorSpec = "Mbbs"
            };
            var doctCreate1 = await doctorRepository.CreateDoctor(doctorModel);
            DoctorModel doctorModel2 = new DoctorModel()
            {
                UserId = userDoctor2.Id,
                DoctorRating = 4,
                DoctorSpec = "Mbbs frcs"
            };
            var doctCreate2 = await doctorRepository.CreateDoctor(doctorModel2);
            DoctorModel doctorModel3 = new DoctorModel()
            {
                UserId = userDoctor3.Id,
                DoctorRating = 5,
                DoctorSpec = "Mbbs"
            };
            var doctCreate3 = await doctorRepository.CreateDoctor(doctorModel3);

            UserModel usereModel4 = new UserModel()
            {
                UserName = "PatentA",
                Name = "Patenet A",
                EmailId = "Patenet.A@gmail.com"
            };
            UserModel usereModel5 = new UserModel()
            {
                UserName = "PatentB",
                Name = "Patenet B",
                EmailId = "Patenet.B@gmail.com"
            };
            var userPatenet1 = await userRepository.CreateUser(usereModel4, "admin");
            var userPatenet2 = await userRepository.CreateUser(usereModel5, "admin");
            

            //-----patent
            PatentModel patent1 = new PatentModel()
            {
                 UserId = userPatenet1.Id
            };
            PatentModel patent2 = new PatentModel()
            {
                UserId = userPatenet2.Id
            };
            var patentCreate1 = await patentRepository.CreatePatent(patent1);
            var patentCreate2 = await patentRepository.CreatePatent(patent2);

            //-------------

            //------patent report
            //PatentReportModel patentReportModel = new PatentReportModel()
            //{
            //    PatentId = patentCreate.Id,
            //    PatentRecord = "tes record"
            //};
            //var CreatePatentReport = await patentReportRepository.CreatePatentReport(patentReportModel);
            //var GetAllPatentReport = await patentReportRepository.GetAllPatentReport();
            //var GetPatentReport = await patentReportRepository.GetPatentReport(CreatePatentReport.PatentReportId);
            //patentReportModel.PatentRecord = "tes record updated";
            //var UpdatePatentReport = await patentReportRepository.UpdatePatentReport(patentReportModel);
            ////-----------

            ////------appointment
            //AppointmentModel appointmentModel = new AppointmentModel()
            //{
            //     DoctorId = doctorModelup.Id,
            //      PatentId = CreatePatentReport.PatentId,
            //       AppointmentStatus = "ongoing",
            //        AppointmentTime  = DateTime.Now
            //};
            //var CreateAppointment = await appointmentRepository.CreateAppointment(appointmentModel); 
            //var GetAppointmentDoctor = await appointmentRepository.GetAppointmentDoctor(doctorModelup.UserId);
            //var GetAppointmentPatent = await appointmentRepository.GetAppointmentPatent(CreatePatentReport.PatentId);
            //appointmentModel.AppointmentStatus = "Cancel";
            //var UpdateAppointment = await appointmentRepository.UpdateAppointment(appointmentModel);

            //NotificationModel notificationModel = new NotificationModel()
            //{
            //    NotificationMessage = "Canceled",
            //    NotificationStatus = "Sent",
            //    NotificationTo = "Shyam",
            //    NotificationType = "mail"
            //};
            //await notificationRepository.CreateNotification(notificationModel);
            //var DeleteAppointment = await appointmentRepository.DeleteAppointment(appointmentModel.AppointmentId);
            //var DeletePatentReport = await patentReportRepository.DeletePatentReport(patentReportModel.PatentReportId);

            //-----------

        }
    }
}

