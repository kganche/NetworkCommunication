using Api1.Data;
using Api1.Dtos;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api1.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    [HttpGet("User/{id}")]
    public ActionResult<User> GetUserById(Guid id)
    {
        var user = DataStore.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet("Users")]
    public ActionResult<IEnumerable<User>> GetUsers()
    {
        var users = DataStore.GetAllUsers();
        return Ok(users);
    }

    [HttpPost("User")]
    public IActionResult CreateUser(UserRequest user)
    {
        if (user == null)
        {
            return BadRequest();
        }

        var createdUser = DataStore.CreateUser(user.FullName, user.Email);
        return Ok(createdUser);
    }
}
