using Azure;
using UserAPI.DAL;
using UserAPI.Domain;

namespace UserAPI.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepo<UserEntity> _userRepo;
        public UserService(IUserRepo<UserEntity> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<UserEntity> CreateUserAsync(CreateUserDTO user)
        {
            UserEntity newUser = new UserEntity
            {
                PartitionKey = user.UserType,
                RowKey = Guid.NewGuid().ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserType = (UserType)Enum.Parse(typeof(UserType), user.UserType)
            };

            await _userRepo.UpsertUserAsync(newUser);
            return newUser;
        }

        public async Task<Response> UpdateUserAsync(UserEntity user)
        {
            return await _userRepo.UpsertUserAsync(user);
        }

        public async Task<UserEntity> GetUserByKeyAsync(string partitionKey, string rowKey)
        {
            return await _userRepo.GetUserByKeyAsync(partitionKey, rowKey);
        }

        public async Task<IEnumerable<UserEntity>> GetPageOfUsersAsync()
        {
            return await _userRepo.GetPageOfUsersAsync();
        }

        public async Task DeleteUserAsync(string partitionKey, string rowKey)
        {
            await _userRepo.DeleteUserAsync(partitionKey, rowKey);
        }
    }
}
