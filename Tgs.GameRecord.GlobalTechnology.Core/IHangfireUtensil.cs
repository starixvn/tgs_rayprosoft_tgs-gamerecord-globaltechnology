using Tgs.GameRecord.GlobalTechnology.Core.JobApplicationServices;

namespace Tgs.GameRecord.GlobalTechnology.Core;

public interface IHangfireUtensil
{
	public Task JobCheckAsync();

	public Task JobExecuteAsync(JobExecuteCommand source);

	public Task JobInitAsync();
}