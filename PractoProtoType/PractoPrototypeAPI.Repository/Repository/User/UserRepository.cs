using PractoPrototypeAPI.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace PractoPrototypeAPI.Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string dbConnectionString;
        private const string getUsersQuery = @"SELECT u.user_id AS Id,
                                                      u.user_guid AS UserId, 
                                                      u.user_name AS UserName, 
                                                      u.email_id AS EmailId, 
                                                      u.name AS Name
                                               FROM ""user"" u ";
        public UserRepository(IConfiguration configuration)
        {
            dbConnectionString = configuration[Constants.PostgreConnectionString];
            if (string.IsNullOrWhiteSpace(dbConnectionString))
            {
                throw new ArgumentNullException("PostgreConnectionString is nul or empty. please provide valid PostgreConnectionString");
            }            
        }
        public async Task<UserModel> CreateUser(UserModel user, string operatorUserName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                connection.Open();
                try
                {
                    user.UserId = Guid.NewGuid();
                    user.CreatedTime = DateTime.UtcNow;
                    var userObject = new
                    {
                        user.UserId,
                        user.UserName,
                        user.EmailId,
                        user.Name,
                        operatorUserName,
                        user.CreatedTime,
                    };
                    var query = "INSERT INTO \"user\" (" +
                                                       "  user_guid," +
                                                       "  user_name," +
                                                       "  email_id," +
                                                       "  name," +
                                                       "  created_by," +
                                                       "  created_time" +
                                                       " ) " +
                                                       "VALUES (" +
                                                       "  @UserId, " +
                                                       "  @UserName, " +
                                                       "  @EmailId, " +
                                                       "  @Name, " +
                                                       "  (SELECT user_id FROM \"user\" u WHERE u.user_name = @operatorUserName), " +
                                                       "  @CreatedTime" +
                                                       ") RETURNING user_id";
                    user.Id = (int) await connection.ExecuteScalarAsync(query, userObject);
                    return user;
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

        public async Task<bool> DeleteUser(Guid userId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                connection.Open();
                try
                {
                    string delete = "DELETE FROM \"user\" WHERE user_guid = @userId";
                    int result = await connection.ExecuteAsync(delete, new { userId });
                    return (result == 1);
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

        public async Task<bool> DeleteUser(string userName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                connection.Open();
                try
                {
                    string delete = "DELETE FROM \"user\" WHERE user_name = @userName";
                    int result = await connection.ExecuteAsync(delete, new { userName });
                    return (result == 1);
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

        public async Task<IEnumerable<UserModel>> GetAllUser()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {                    
                    return await connection.QueryAsync<UserModel>(getUsersQuery);
                }
                catch
                {
                    throw;
                }                
            }
        }

        public async Task<UserModel> GetUser(Guid userId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    string query = getUsersQuery + " WHERE u.user_guid = @userId";
                    return (await connection.QueryAsync<UserModel>(query, new { userId })).SingleOrDefault();
                }
                catch
                {
                    throw;
                }
            }
        }

        public async Task<UserModel> GetUser(string userName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    string query = getUsersQuery + " WHERE u.user_name = @userName";
                    return (await connection.QueryAsync<UserModel>(query, new { userName })).SingleOrDefault();
                }
                catch
                {
                    throw;
                }
            }
        }

        public async Task<bool> UpdateUser(UserModel user, string operatorUserName)
        {
            DateTime updatedTime = DateTime.UtcNow;
                     
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    var updateQuery = @"UPDATE ""user"" 
                                        SET email_id = @EmailId, 
                                            updated_by = (SELECT user_id FROM ""user"" WHERE user_name = @operatorUserName),
                                            updated_time = @updatedTime 
                                        WHERE user_guid = @UserId;";

                    await connection.ExecuteAsync(updateQuery, 
                                                         new { user.EmailId, 
                                                               operatorUserName, 
                                                               updatedTime, 
                                                               user.UserId });
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }

        public async Task<UserModel> GetUser(int userId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(dbConnectionString))
            {
                try
                {
                    string query = getUsersQuery + " WHERE u.user_id = @userId";
                    return (await connection.QueryAsync<UserModel>(query, new { userId })).SingleOrDefault();
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
