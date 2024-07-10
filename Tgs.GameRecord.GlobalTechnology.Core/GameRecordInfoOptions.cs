namespace Tgs.GameRecord.GlobalTechnology.Core;

public record GameRecordInfoOptions
{
	public string GameProviderCode { get; set; } = default!;
	public IEnumerable<string> GameProviderCodes { get; init; } = default!;
	public string HangfirePrefix { get; set; } = "gamerecord-awc-hangfire:";
	public string HangfireLocking { get; set; } = "gamerecord-awc-locking:";

	public double IntervalMinutes { get; init; } = 3;
	public double IntervalMinutesMax { get; init; } = 60;
	public double IntervalMinutesMinimum { get; init; } = 1;
}