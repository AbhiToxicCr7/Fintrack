using AuthenticationServer.DTOs;
using AuthenticationServer.Helpers;
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
        private readonly UserExpenseHelper _userExpenseHelper;

        public UserExpenseController(ApplicationDbContext context, IRedisCacheService cache)
        {
            _context = context;
            _cache = cache;
            _userExpenseHelper = new UserExpenseHelper();
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
        public ActionResult<IEnumerable<UserExpenseDTO>> GetProductById([FromQuery] int userId, [FromQuery] string? category = null, [FromQuery] int? day = null, [FromQuery] int? month = null, [FromQuery] int? year = null)
        {
            //var exp = _context.UserExpenses.ToList();
            var userExpense = _context.UserExpenses.Where(x => x.UserId == userId);

            var filteredUserExpense = _userExpenseHelper.FilterDataWithParams(userExpense, category, day, month, year);
            if (filteredUserExpense == null)
            {
                return NotFound(new { message = $"Expense with UserID {userId} not found." });
            }

            var monthlyAmount = _userExpenseHelper.CalculateMonthlyTotalExpense(userExpense);

            var expenses = filteredUserExpense.Select(x => new UserExpenseDTO
            {
                Amount = x.Amount,
                Currency = x.Currency,
                Category = x.Category,
                Date = x.Date,
                Note = x.Note,
                UserId = x.UserId,
                Id = x.Id
            });

            var expByCategory = _userExpenseHelper.CalculateCategoryWiseExpense(userExpense, category);
            string highestSpentCategory = _userExpenseHelper.CaculateHighestSpentCategory(userExpense);

            var response = new UserExpenseResponseDTO
            {
                TotalExpenseAmount = monthlyAmount,
                Expenses = expenses,
                ExpenseByCategory = expByCategory,
                HighestSpentCategory = highestSpentCategory
            };

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
