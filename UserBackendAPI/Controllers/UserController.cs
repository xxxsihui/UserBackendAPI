using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UserBackendAPI.Models;
using UserCRUDBackendAPI.Models;

namespace UserCRUDBackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController: ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(ApplicationDbContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult RegisterUser([FromBody] BaseUserModel user)
        {
            try
            {
                // Check if username not null
                if (user.UserName == null)
                {
                    return StatusCode(400, "Invalid user name");
                }

                // Add data into database
                User createdUser = new User()
                {
                    Username = user.UserName,
                    Mail = user.Mail,
                    PhoneNumber = user.PhoneNumber,
                    Skillsets = user.Skillsets,
                    Hobby = user.Hobby,
                };

                _context.Users.Add(createdUser);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetUser), new {userId = createdUser.Id}, $"User with id: {createdUser.Id} created.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured");
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpPut]
        public ActionResult UpdateUser([FromBody] BaseUserModel updatedUser, int userId)
        {
            try
            {
                // Check invalid user id
                if (userId <= 0) {
                    return StatusCode(400, "Invalid user id");
                }

                User? user = this._context.Users.Where(u => u.Id == userId).FirstOrDefault();

                // Check user id exists in database
                if (user == null)
                {
                    return NotFound($"There is no user found for id: {userId}");
                }

                // Update database
                user.PhoneNumber = updatedUser.PhoneNumber;
                user.Mail = updatedUser.Mail;
                user.Username = updatedUser.UserName;
                user.Hobby = updatedUser.Hobby;
                user.Skillsets = updatedUser.Skillsets;

                _context.SaveChanges();

                return Ok(user);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured");
                return StatusCode(500, "An unexpected error has occurred.");
            }
            return Ok();
        }

        [HttpDelete]
        public ActionResult DeleteUser(int userId)
        {
            try
            {
                // Check invalid user id
                if (userId <= 0)
                {
                    return StatusCode(400, "Invalid user id");
                }

                User? user = this._context.Users.Where(u => u.Id == userId).FirstOrDefault();

                // Check user id exists in database
                if (user == null)
                {
                    return NotFound($"There is no user found for id: {userId}");
                }

                // Remove user from db
                _context.Remove(user);
                _context.SaveChanges();

                return Ok("User has been removed");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured");
                return StatusCode(500, "An unexpected error has occurred.");
            }

            return Ok();
        }

        [HttpGet("{userId}")]
        public ActionResult GetUser(int userId)
        {
            try
            {
                // Check invalid user id
                if(userId <= 0)
                {
                    return StatusCode(400, "Invalid user id");
                }

                BaseUserModel? user = this._context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => new BaseUserModel
                    {
                        UserName = u.Username,
                        PhoneNumber = u.PhoneNumber ?? "",
                        Mail = u.Mail ?? "",
                        Skillsets = u.Skillsets ?? "",
                        Hobby = u.Hobby ?? ""
                    }).FirstOrDefault();

                // Check user id exists in database
                if (user== null)
                {
                    return NotFound($"There is no user found for id: {userId}");
                }

                return Ok(user);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured");
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }

        [HttpGet]
        public ActionResult ListUser()
        {
            try
            {
                List<User> users = this._context.Users.ToList();

                return Ok(users);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured");
                return StatusCode(500, "An unexpected error has occurred.");
            }
        }
    }
}
