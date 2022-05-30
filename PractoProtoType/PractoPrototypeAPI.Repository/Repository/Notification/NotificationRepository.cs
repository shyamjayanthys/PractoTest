using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PractoPrototypeAPI.Model;
using System;
using System.Threading.Tasks;

namespace PractoPrototypeAPI.Repository.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly string dbConnectionString;
        public NotificationRepository(IConfiguration configuration)
        {
            dbConnectionString = configuration[Constants.PostgreConnectionString];
            if (string.IsNullOrWhiteSpace(dbConnectionString))
            {
                throw new ArgumentNullException("PostgreConnectionString is nul or empty. please provide valid PostgreConnectionString");
            }
        }
        public async Task<NotificationModel> CreateNotification(NotificationModel notification)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                connection.Open();
                try
                {
                    notification.NotificationId = Guid.NewGuid();
                    var notificationObject = new
                    {
                        notification.NotificationId,
                        notification.NotificationType,
                        notification.NotificationMessage,
                        notification.NotificationTo,
                        notification.NotificationStatus,
                    };
                    var query = "INSERT INTO notification (" +
                                                           "  notification_guid," +
                                                           "  notification_type," +
                                                           "  notification_message," +
                                                           "  notification_to," +
                                                           "  notification_status" +
                                                           " ) " +
                                                           "VALUES (" +
                                                           "  @NotificationId, " +
                                                           "  @NotificationType, " +
                                                           "  @NotificationMessage, " +
                                                           "  @NotificationTo, " +
                                                           "  @NotificationStatus" +
                                                           ") RETURNING notification_id";
                    await connection.ExecuteScalarAsync(query, notificationObject);
                    return notification;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
