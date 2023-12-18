using Azure;
using Moq;
using UserAPI.DAL;
using UserAPI.Domain;
using UserAPI.Service;

namespace xUnitTests.Service
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepo<UserEntity>> _userRepoMock;
        private readonly IUserService _userService;
        private readonly Guid _mockRowKey;
        private readonly string _mockPartKey;

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepo<UserEntity>>();
            _mockRowKey = Guid.NewGuid();
            _mockPartKey = "User";
            _userService = new UserService(_userRepoMock.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateUserAndReturnUserEntity()
        {
            // Arrange
            var createUserDto = new CreateUserDTO
            {
                UserType = "User",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            // Act
            var result = await _userService.CreateUserAsync(createUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createUserDto.UserType, result.PartitionKey);
            Assert.NotNull(result.RowKey);
            Assert.Equal(createUserDto.FirstName, result.FirstName);
            Assert.Equal(createUserDto.LastName, result.LastName);
            Assert.Equal(createUserDto.Email, result.Email);
            Assert.Equal(UserType.User, result.UserType);
            _userRepoMock.Verify(repo => repo.UpsertUserAsync(It.IsAny<UserEntity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUserAndReturnUserEntity()
        {
            // Arrange
            UserEntity userEntity = new UserEntity
            {
                PartitionKey = _mockPartKey,
                RowKey = _mockRowKey.ToString(),
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                UserType = UserType.User
            };

            // Act
            Response result = await _userService.UpdateUserAsync(userEntity);

            // Assert
            //Assert.NotNull(result);
            //Assert.False(result.IsError);
            _userRepoMock.Verify(repo => repo.UpsertUserAsync(userEntity), Times.Once);
        }

        [Fact]
        public async Task GetUserByKeyAsync_ShouldReturnUserEntity()
        {
            // Arrange
            var partitionKey = _mockPartKey;
            var rowKey = _mockRowKey.ToString();
            var expectedUserEntity = new UserEntity
            {
                PartitionKey = partitionKey,
                RowKey = rowKey,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserType = UserType.User
            };

            _userRepoMock.Setup(repo => repo.GetUserByKeyAsync(partitionKey, rowKey))
                .ReturnsAsync(expectedUserEntity);

            // Act
            var result = await _userService.GetUserByKeyAsync(partitionKey, rowKey);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUserEntity, result);
        }

        [Fact]
        public async Task GetPageOfUsersAsync_ShouldReturnListOfUserEntities()
        {
            // Arrange
            var expectedUsers = new List<UserEntity>
            {
                new UserEntity { /* UserEntity properties */ },
                new UserEntity { /* UserEntity properties */ },
                // Add more user entities as needed
            };

            _userRepoMock.Setup(repo => repo.GetPageOfUsersAsync())
                .ReturnsAsync(expectedUsers);

            // Act
            var result = await _userService.GetPageOfUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUsers, result);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldDeleteUser()
        {
            // Arrange
            var partitionKey = _mockPartKey;
            var rowKey = _mockRowKey.ToString();

            // Act
            await _userService.DeleteUserAsync(partitionKey, rowKey);

            // Assert
            _userRepoMock.Verify(repo => repo.DeleteUserAsync(partitionKey, rowKey), Times.Once);
        }
    }

}