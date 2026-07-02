using AuthenticationServer.DTOs;
using AuthenticationServer.Services.Cache;
using FinTrack.Data;
using FinTrack.DTOs;
using FinTrack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceServer.Models;

namespace AuthenticationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserExpenseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRedisCacheService _cache;

        public UserExpenseController(ApplicationDbContext context, IRedisCacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpPost("AddExpense")]
        public async Task<IActionResult> AddExpense([FromBody] UserExpenseDTO userExpenseDTO)
        {
            // Validate the incoming model.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == userExpenseDTO.UserId);
            if (!userExists)
                return BadRequest(new { message = $"UserId {userExpenseDTO.UserId} does not exist." });

            var newUserExpense = new UserExpense
            {
                UserId = userExpenseDTO.UserId,
                Amount = userExpenseDTO.Amount,
                Currency = userExpenseDTO.Currency,
                Category = userExpenseDTO.Category,
                IsActive = userExpenseDTO.IsActive,
                Date = userExpenseDTO.Date,
                Note = userExpenseDTO.Note
            };

            _context.UserExpenses.Add(newUserExpense);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
