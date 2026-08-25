using AuthenticationServer.DTOs;
using AuthenticationServer.Helpers;
using FinTrack.Data;

namespace AuthenticationServer.Services
{
    public class DashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly DashboardHelper _dashboardHelper;
        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
            _dashboardHelper = new DashboardHelper(context);
        }

        public async Task<DashboardDTO?> GetDashboardCalculations(int userId)
        {
            var recentTransactions = _dashboardHelper.GetRecentTransactions(userId);

            return new DashboardDTO
            {
                RecentTransactions = recentTransactions
            };
        }
    }
}
