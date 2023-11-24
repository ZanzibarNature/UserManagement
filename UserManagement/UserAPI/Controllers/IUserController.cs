using Microsoft.AspNetCore.Mvc;
using UserAPI.Domain;

namespace UserAPI.Controllers
{
    public interface IUserController
    {
        Task<IActionResult> GetByKey(string partitionKey, string rowKey);
        Task<IActionResult> GetPageOfUsers();
        Task<IActionResult> CreateUser([FromBody] CreateUserDTO userDTO);
        Task<IActionResult> UpdateUser([FromBody] UserEntity user);
        Task<IActionResult> DeleteUser(string partitionKey, string rowKey);
    }
}
