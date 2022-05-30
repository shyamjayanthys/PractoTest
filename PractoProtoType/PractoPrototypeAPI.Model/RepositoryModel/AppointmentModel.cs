using System;

namespace PractoPrototypeAPI.Model
{
    public class AppointmentModel
    {
        public Guid AppointmentId { get; set; }
        public string AppointmentStatus { get; set; }
        public DateTime AppointmentTime { get; set; }
        public int DoctorId { get; set; }
        public int PatentId { get; set; }
    }
}
