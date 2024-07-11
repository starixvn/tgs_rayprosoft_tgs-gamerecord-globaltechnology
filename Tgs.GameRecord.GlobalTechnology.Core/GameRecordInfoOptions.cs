namespace Tgs.GameRecord.GlobalTechnology.Core;

public record GameRecordInfoOptions
{
	public string GameProviderCode { get; set; } = default!;
	public IEnumerable<string> GameProviderCodes { get; init; } = default!;
	public string HangfirePrefix { get; set; } = "gamerecord-gt-hangfire:";
	public string HangfireLocking { get; set; } = "gamerecord-gt-locking:";

	public double IntervalMinutes { get; init; } = 5;
	public double IntervalMinutesMax { get; init; } = 15;
	public double IntervalMinutesMinimum { get; init; } = 1;
}