using System;

namespace PractoPrototypeAPI.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
