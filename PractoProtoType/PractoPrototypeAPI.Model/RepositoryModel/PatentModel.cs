using System;

namespace PractoPrototypeAPI.Model
{
    public class PatentModel
    {
        public int Id { get; set; }
        public Guid PatentId { get; set; }
        public int UserId { get; set; }
    }
}
