namespace Tgs.GameRecord.GlobalTechnology.Core.ApplicationServices;

public record PullRecordCommand(
	string GameProviderCode,
	Uri ApiUrl,
	int BrandId,
	string SecretKey,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime);
