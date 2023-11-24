using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Domain;
using UserAPI.Service;

namespace UserAPI.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetByKey/{partitionKey}/{rowKey}")]
        public async Task<IActionResult> GetByKey(string partitionKey, string rowKey)
        {
            var user = await _userService.GetUserByKeyAsync(partitionKey, rowKey);

            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet("GetPage")]
        public async Task<IActionResult> GetPageOfUsers()
        {
            IEnumerable<UserEntity> result = await _userService.GetPageOfUsersAsync();
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost("Create")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest("Invalid data in the request body");
            }

            UserEntity newUser = await _userService.CreateUserAsync(userDTO);

            return CreatedAtAction(nameof(GetByKey), new { partitionKey = newUser.PartitionKey, rowKey = newUser.RowKey }, newUser);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserEntity user)
        {
            if (user == null)
            {
                return BadRequest("Invalid data in the request body");
            }

            UserEntity updatedUser = await _userService.UpdateUserAsync(user);

            return Ok(updatedUser);
        }

        [HttpDelete("Delete/{partitionKey}/{rowKey}")]
        public async Task<IActionResult> DeleteUser(string partitionKey, string rowKey)
        {
            await _userService.DeleteUserAsync(partitionKey, rowKey);

            return NoContent();
        }
    }
}
