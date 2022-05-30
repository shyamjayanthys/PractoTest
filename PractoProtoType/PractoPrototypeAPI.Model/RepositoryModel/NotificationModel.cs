using System;

namespace PractoPrototypeAPI.Model
{
    public class NotificationModel
    {
        public Guid NotificationId { get; set; }
        public string NotificationType { get; set; }
        public string NotificationMessage { get; set; }
        public string NotificationTo { get; set; }
        public string NotificationStatus { get; set; }
    }
}
