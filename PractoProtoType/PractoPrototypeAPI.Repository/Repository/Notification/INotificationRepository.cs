using PractoPrototypeAPI.Model;
using System.Threading.Tasks;

namespace PractoPrototypeAPI.Repository.Repository
{
    public interface INotificationRepository
    {
        Task<NotificationModel> CreateNotification(NotificationModel notification);
    }
}
