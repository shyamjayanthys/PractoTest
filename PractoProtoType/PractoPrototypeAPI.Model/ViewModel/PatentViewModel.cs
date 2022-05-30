using System.Collections.Generic;

namespace PractoPrototypeAPI.Model
{
    public class PatentViewModel
    {
        public int PatentId { get; set; }
        public List<DoctorListViewModel> doctorListViews { get; set; }
        public string AvailableAppointment { get; set; }
        public string RequestAppointment { get; set; }
    }
}
