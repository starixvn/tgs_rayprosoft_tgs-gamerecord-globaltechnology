using Tgs.GameInterface.GlobalTechnology.Protos;

namespace Tgs.GameRecord.GlobalTechnology.Core.ApplicationServices;

public record HandleRecordCommand(
	string GameProviderCode,
	GameRecordInfo Info);