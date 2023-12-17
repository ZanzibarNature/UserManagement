﻿using UserAPI.Domain;
using Azure;

namespace UserAPI.Service
{
    public interface IUserService
    {
        Task<UserEntity> CreateUserAsync(CreateUserDTO user);
        Task<Response> UpdateUserAsync(UserEntity user);
        Task<UserEntity> GetUserByKeyAsync(string partitionKey, string rowKey);
        Task<IEnumerable<UserEntity>> GetPageOfUsersAsync();
        Task DeleteUserAsync(string partitionKey, string rowKey);
    }
}
