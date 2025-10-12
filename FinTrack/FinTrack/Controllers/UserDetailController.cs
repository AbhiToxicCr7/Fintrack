using AuthenticationServer.DTOs;
using FinTrack.Data;
using FinTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceServer.Models;

namespace AuthenticationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserDetailController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserDetailController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<UserDetail>> GetAllUserDetails()
        {
            var userDetail = _context.UserDetails;
            return Ok(userDetail);
        }

        // Retrieves a specific detail by UserID.
        [HttpGet("GetById/{id}", Name = "GetUserDetailByUserId")]
        public ActionResult<UserDetail> GetProductById(int userId)
        {
            var userDetail = _context.UserDetails.FirstOrDefault(ud => ud.Id == userId);
            if (userDetail == null)
            {
                return NotFound(new { message = $"Detail with UserID {userId} not found." });
            }

            return Ok(userDetail);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddUserDetail([FromBody] UserDetailDTO userDetailDTO)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            var userExists = await _context.Users.AnyAsync(u => u.Id == userDetailDTO.UserId);
            if (!userExists)
                return BadRequest(new { message = $"UserId {userDetailDTO.UserId} does not exist." });

            var alreadyHas = await _context.UserDetails.AnyAsync(ud => ud.UserId == userDetailDTO.UserId);
            if (alreadyHas)
                return Conflict(new { message = $"UserId {userDetailDTO.UserId} already has a UserDetail." });

            var newUserDetail = new UserDetail
            {
                UserId = userDetailDTO.UserId,
                Profession = userDetailDTO.Profession,
                JobTitle = userDetailDTO.JobTitle,
                AnnualSalary = userDetailDTO.AnnualSalary,
                MonthlyIncome = userDetailDTO.MonthlyIncome,
                MonthlyExpenses = userDetailDTO.MonthlyExpenses,
                MonthlyInvestment = userDetailDTO.MonthlyInvestment
            };

            _context.UserDetails.Add(newUserDetail);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
