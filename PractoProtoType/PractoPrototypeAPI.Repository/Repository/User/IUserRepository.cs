using PractoPrototypeAPI.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PractoPrototypeAPI.Repository.Repository
{
    public interface IUserRepository
    {
        Task<UserModel> CreateUser(UserModel user, string operatorUserName);
        Task<bool> UpdateUser(UserModel user, string operatorUserName);
        Task<UserModel> GetUser(Guid userId);
        Task<UserModel> GetUser(string userName);
        Task<UserModel> GetUser(int userId);
        Task<IEnumerable<UserModel>> GetAllUser();
        Task<bool> DeleteUser(Guid userId);
        Task<bool> DeleteUser(string userName);
    }
}
