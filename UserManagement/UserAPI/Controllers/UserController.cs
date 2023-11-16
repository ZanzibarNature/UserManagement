using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
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

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest("Invalid data in the request body");
            }

            UserEntity newUser = await _userService.UpsertUserAsync(userDTO);

            return CreatedAtAction(nameof(GetByKey), new { partitionKey = newUser.PartitionKey, rowKey = newUser.RowKey }, newUser);
        }
    }
}
