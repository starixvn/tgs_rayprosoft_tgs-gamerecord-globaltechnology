namespace Tgs.GameRecord.GlobalTechnology.Core.JobApplicationServices;

public record JobExecuteCommand(
	string GameProviderCode,
	Uri DataApiUrl,
	int BrandId,
	string SecretKey);