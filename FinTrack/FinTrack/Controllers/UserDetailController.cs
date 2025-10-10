using FinTrack.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResourceServer.Models;

namespace AuthenticationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserDetailController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
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

        // Creates a new product.
        [HttpPost("Add")]
        public IActionResult AddUserDetail([FromBody] UserDetail userDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.UserDetails.Add(userDetail);

            return CreatedAtAction(nameof(UserDetail), new { message = "UserDetails added successfully" });
        }
    }
}
