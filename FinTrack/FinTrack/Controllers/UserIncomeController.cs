using AuthenticationServer.DTOs;
using AuthenticationServer.Helpers;
using AuthenticationServer.Models;
using AuthenticationServer.Services.Cache;
using FinTrack.Data;
using FinTrack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserIncomeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRedisCacheService _cache;
        private readonly UserIncomeHelper _userIncomeHelper;

        public UserIncomeController(ApplicationDbContext context, IRedisCacheService cache)
        {
            _context = context;
            _cache = cache;
            _userIncomeHelper = new UserIncomeHelper();
        }

        [HttpPost("AddIncome")]
        public async Task<IActionResult> AddIncome([FromBody] UserIncomeDTO userIncomeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == userIncomeDTO.UserId);
            if (!userExists)
                return BadRequest(new { message = $"UserId {userIncomeDTO.UserId} does not exist." });

            var newUserIncome = new UserIncome
            {
                UserId = userIncomeDTO.UserId,
                Amount = userIncomeDTO.Amount,
                Currency = userIncomeDTO.Currency,
                Category = userIncomeDTO.Category,
                IsActive = userIncomeDTO.IsActive,
                Date = userIncomeDTO.Date,
                Note = userIncomeDTO.Note
            };

            _context.UserIncomes.Add(newUserIncome);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("GetById", Name = "GetUserIncomesByUserId")]
        public ActionResult<UserIncomeResponseDTO> GetUserIncomesByUserId([FromQuery] int userId, [FromQuery] string? category = null, [FromQuery] int? day = null, [FromQuery] int? month = null, [FromQuery] int? year = null)
        {
            var userIncome = _context.UserIncomes.Where(x => x.UserId == userId);
            var filteredUserIncome = _userIncomeHelper.FilterDataWithParams(userIncome, category, day, month, year);

            if (!filteredUserIncome.Any())
            {
                return NotFound(new { message = $"Income with UserID {userId} not found." });
            }

            var monthlyAmount = _userIncomeHelper.CalculateMonthlyTotalIncome(userIncome);
            var incomes = filteredUserIncome.Select(x => new UserIncomeDTO
            {
                Id = x.Id,
                UserId = x.UserId,
                Amount = x.Amount,
                Currency = x.Currency,
                Category = x.Category,
                IsActive = x.IsActive,
                Date = x.Date,
                Note = x.Note
            });

            var incomeByCategory = _userIncomeHelper.CalculateCategoryWiseIncome(userIncome);
            string highestIncomeCategory = _userIncomeHelper.CalculateHighestIncomeCategory(userIncome);

            var response = new UserIncomeResponseDTO
            {
                TotalIncomeAmount = monthlyAmount,
                Incomes = incomes,
                IncomeByCategory = incomeByCategory,
                HighestIncomeCategory = highestIncomeCategory
            };

            return Ok(response);
        }

        [HttpPut("UpdateIncome/{id}")]
        public async Task<IActionResult> UpdateIncome([FromRoute(Name = "id")] int incomeId, [FromBody] UserIncomeDTO userIncomeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingIncome = await _context.UserIncomes.FindAsync(incomeId);
            if (existingIncome == null)
            {
                return NotFound(new { message = $"Income with ID {incomeId} not found." });
            }

            existingIncome.Amount = userIncomeDTO.Amount;
            existingIncome.Currency = userIncomeDTO.Currency;
            existingIncome.Category = userIncomeDTO.Category;
            existingIncome.IsActive = userIncomeDTO.IsActive;
            existingIncome.Date = userIncomeDTO.Date;
            existingIncome.Note = userIncomeDTO.Note;

            _context.UserIncomes.Update(existingIncome);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("DeleteIncome/{id}")]
        public async Task<IActionResult> DeleteIncome([FromRoute(Name = "id")] int incomeId)
        {
            var existingIncome = await _context.UserIncomes.FindAsync(incomeId);
            if (existingIncome == null)
            {
                return NotFound(new { message = $"Income with ID {incomeId} not found." });
            }

            _context.UserIncomes.Remove(existingIncome);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
