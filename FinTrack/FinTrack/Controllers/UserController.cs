using BCrypt.Net;
using FinTrack.Data;
using FinTrack.DTOs;
using FinTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            // Validate the incoming model.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Test comment to check repo
            var existingUser = _context.Users.FirstOrDefault(u => u.UserName.ToLower() == registerDTO.UserName.ToLower());

            if (existingUser != null)
            {
                return Conflict("Email is ALready registered");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

            var newUser = new User
            {
                Email = registerDTO.Email,
                Password = hashedPassword,
                FirstName = registerDTO.Firstname,
                LastName = registerDTO.Lastname,
                Address = registerDTO.Address,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.UserName,
                IsActive = true
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var userRole = _context.Roles.FirstOrDefault(x => x.Name == "User");

            if (userRole != null)
            {
                var newUserRole = new UserRole
                {
                    UserId = newUser.Id,
                    RoleId = userRole.Id
                };

                _context.UserRoles.Add(newUserRole);
                await _context.SaveChangesAsync();
            }

            //return CreatedAtAction(nameof(GetProfile), new { id = newUser.Id }, new { message = "User registered successfully." });

            return Created($"api/User/{newUser.Id}", new
            {
                userId=newUser.Id,
                message = "User Registered"
            });
        }

        [HttpGet("GetProfile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email);

            if (emailClaim == null)
            {
                return Unauthorized(new { message = "Invalid token: Email claim missing." });
            }

            string userEmail = emailClaim.Value;

            var user =await _context.Users.Include(ur => ur.UserRole).ThenInclude(r => r.Role).FirstOrDefaultAsync(x =>x.Email.ToLower() == userEmail.ToLower());

            var profileDTO = new ProfileDTO
            {
                Email = user.Email,
                Firstname = user.FirstName,
                Lastname = user?.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                UserName = user.UserName,
                Roles = user.UserRole.Select(ur => ur.Role.Name).ToList(),
            };

            return Ok(profileDTO);
        }

        [HttpPut("UpdateProfile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO updateProfileDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailClaim = User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Email);

            if(emailClaim == null)
            {
                return Unauthorized(new {message = "Email is unauthorized"});
            }

            string email = emailClaim.Value;

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

            if(user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            if(updateProfileDTO.Firstname != null)
            {
                user.FirstName = updateProfileDTO.Firstname;
            }

            if (updateProfileDTO.Lastname != null)
            {
                user.LastName = updateProfileDTO.Lastname;
            }

            if (updateProfileDTO.Email != null)
            {
                var emailExists = await _context.Users
                    .AnyAsync(u => u.Email.ToLower() == updateProfileDTO.Email.ToLower() && u.Id != user.Id);
                if (emailExists)
                {
                    return Conflict(new { message = "Email is already in use by another account." });
                }
                user.Email = updateProfileDTO.Email;
            }

            if (updateProfileDTO.Password != null)
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(updateProfileDTO.Password);
                user.Password = hashedPassword;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new {message = "Profile updated succesfully"});
        }
    }
}
