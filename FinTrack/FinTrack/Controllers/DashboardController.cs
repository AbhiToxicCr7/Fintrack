using AuthenticationServer.Services.Cache;
using AuthenticationServer.Services;
using FinTrack.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthenticationServer.DTOs;

namespace AuthenticationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly DashboardService _dashboardService;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
            _dashboardService = new DashboardService(context);
        }

        [HttpGet("GetDashboardData", Name = "GetUserDashboardData")]
        public async Task<ActionResult<DashboardDTO>> GetDashboardData([FromQuery] int userId)
        {
            var response = await _dashboardService.GetDashboardCalculations(userId);

            if (response == null)
            {
                return NotFound(new
                {
                    message = $"No data found for UserID {userId}."
                });
            }

            return Ok(response);
        }
    }
}
