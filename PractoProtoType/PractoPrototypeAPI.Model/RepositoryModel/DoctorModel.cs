using System;

namespace PractoPrototypeAPI.Model
{
    public class DoctorModel
    {
        public int Id { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorSpec { get; set; }
        public int UserId { get; set; }
        public int DoctorRating { get; set; }
    }
}
