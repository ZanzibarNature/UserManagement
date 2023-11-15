using Microsoft.AspNetCore.Mvc;
using UserAPI.Domain;
using UserAPI.Domain.DTO;

namespace UserAPI.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController()
        {
            // inject services
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            return Ok();
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] CreateUserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest("Invalid data in the request body");
            }

            RetrieveUserDTO newUser = new RetrieveUserDTO(); // add service and repo layers

            return CreatedAtAction("GetById", new { id = newUser.ID }, newUser);
        }
    }
}
