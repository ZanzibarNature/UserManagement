using UserAPI.DAL;
using UserAPI.Domain;

namespace UserAPI.Service
{
    public class UserService
    {
        private readonly UserRepo<UserEntity> _userRepo;
        public UserService(UserRepo<UserEntity> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<UserEntity> UpsertUserAsync(CreateUserDTO user)
        {
            UserEntity newUser = new UserEntity
            {
                PartitionKey = user.UserType.ToString(),
                RowKey = Guid.NewGuid().ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserType = user.UserType
            };

            await _userRepo.UpsertUserAsync(newUser);
            return newUser;
        }

        public async Task<UserEntity> GetUserByKeyAsync(string partitionKey, string rowKey)
        {
            return await _userRepo.GetUserByKeyAsync(partitionKey, rowKey);
        }
    }
}
