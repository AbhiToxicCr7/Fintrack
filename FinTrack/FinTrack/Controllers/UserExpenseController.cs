using AuthenticationServer.DTOs;
using AuthenticationServer.Helpers;
using AuthenticationServer.Services;
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
        private readonly UserExpenseService _userExpenseService;

        public UserExpenseController(ApplicationDbContext context, IRedisCacheService cache)
        {
            _context = context;
            _cache = cache;
            _userExpenseService = new UserExpenseService(context);   
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

        [HttpGet("GetById", Name = "GetUserExpensesByUserId")]
        public async Task<ActionResult<UserExpenseResponseDTO>> GetUserExpenses([FromQuery] int userId, [FromQuery] string? category = null, [FromQuery] int? day = null, [FromQuery] int? month = null, [FromQuery] int? year = null)
        {
            var response = await _userExpenseService.GetUserExpenses(
                userId,
                category,
                day,
                month,
                year);

            if (response == null)
            {
                return NotFound(new
                {
                    message = $"No expenses found for UserID {userId}."
                });
            }

            return Ok(response);
        }

        [HttpPut("UpdateExpense/{id}")]
        public async Task<IActionResult> UpdateExpense([FromRoute(Name = "id")] int expenseId, [FromBody] UserExpenseDTO userExpenseDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingExpense = await _context.UserExpenses.FindAsync(expenseId);
            if (existingExpense == null)
            {
                return NotFound(new { message = $"Expense with ID {expenseId} not found." });
            }

            existingExpense.Amount = userExpenseDTO.Amount;
            existingExpense.Currency = userExpenseDTO.Currency;
            existingExpense.Category = userExpenseDTO.Category;
            existingExpense.IsActive = userExpenseDTO.IsActive;
            existingExpense.Date = userExpenseDTO.Date;
            existingExpense.Note = userExpenseDTO.Note;

            _context.UserExpenses.Update(existingExpense);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("DeleteExpense/{id}")]
        public async Task<IActionResult> DeleteExpense([FromRoute(Name = "id")] int expenseId)
        {
            var existingExpense = await _context.UserExpenses.FindAsync(expenseId);
            if (existingExpense == null)
            {
                return NotFound(new { message = $"Expense with ID {expenseId} not found." });
            }

            _context.UserExpenses.Remove(existingExpense);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
