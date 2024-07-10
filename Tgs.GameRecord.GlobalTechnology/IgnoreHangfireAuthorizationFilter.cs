using Hangfire.Dashboard;

namespace Tgs.GameRecord.GlobalTechnology;

public class IgnoreHangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
	public bool Authorize(DashboardContext context) => true;
}
