using BankApp.Data;
using BankApp.Interfaces;
using BankApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BankApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        /// <summary>
        /// Endpoint to create a user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Information of the newly-created user</returns>
        [HttpPost]
        [Route("api/users/add")]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            try
            {
                user.Validate();
                var updatedUser = await _usersService.AddUser(user);
                return CreatedAtAction("AddUser", new { id = updatedUser.AccountId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Creating user encountered an error: {ex.Message}");
            }
        }

    }
}
