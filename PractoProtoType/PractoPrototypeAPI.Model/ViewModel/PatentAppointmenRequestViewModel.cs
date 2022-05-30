using System;

namespace PractoPrototypeAPI.Model
{
    public class PatentAppointmenRequestViewModel
    {
        public int PatentId { get; set; }
        public DoctorListViewModel doctor { get; set; }
        public DateTime RequestAppointment { get; set; }
    }
}
