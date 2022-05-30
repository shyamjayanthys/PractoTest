using System.Collections.Generic;

namespace PractoPrototypeAPI.Model
{
    public class DoctorViewModel
    {        
        public int DoctorId { get; set; }

        public List<PatentListViewModel> patents { get; set; }
    }
}
