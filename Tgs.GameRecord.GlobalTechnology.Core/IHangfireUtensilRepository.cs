using Tgs.GameRecord.GlobalTechnology.Core.JobApplicationServices;

namespace Tgs.GameRecord.GlobalTechnology.Core;

public interface IHangfireUtensilRepository
{
	public Task JobCheckAsync();

	public Task JobExecuteAsync(JobExecuteCommand source);

	public Task JobInitAsync();
}
