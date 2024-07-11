using System.Text.Json;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.GlobalTechnology.Client;
using Tgs.GameInterface.GlobalTechnology.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.GlobalTechnology.Core.ApplicationServices;

internal class PullRecordCommandService(
	ILogger<PullRecordCommandService> logger,
	IGameInterfaceGlobalTechnologyClient client,
	ICommandService<HandleRecordCommand> service) : ICommandService<PullRecordCommand>
{
	public async ValueTask ExecuteAsync(PullRecordCommand command, CancellationToken cancellationToken = default)
	{
		var page = 1;

		IAsyncEnumerable<GameRecordInfo> gameRecords;

		do
		{
			gameRecords = client.GameRecordAsync(
				dataApiUrl: command.ApiUrl,
				brandId: command.BrandId,
				secretKey: command.SecretKey,
				startTime: command.StartTime,
				endTime: command.EndTime,
				page: page,
				pageSize: 5000,
				cancellationToken: cancellationToken);

			await foreach (var gameRecord in gameRecords)
			{
				try
				{
					await service.ExecuteAsync(
						command: new(
							GameProviderCode: command.GameProviderCode,
							Info: gameRecord),
						cancellationToken: cancellationToken)
						.ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					logger.LogError(ex, "{logError}",
						JsonSerializer.Serialize(new
						{
							LogOn = DateTime.Now.ToString("u"),
							Data = new
							{
								gameRecord.PlayerCode,
								gameRecord.ExternalBetId,
								ExternalCreatedOn = gameRecord.ExternalCreatedOn.ToDateTimeOffset(),
								ExternalClosedOn = gameRecord.ExternalClosedOn.ToDateTimeOffset()
							},
							Message = "An exception occurred in this game wager."
						}));
				}
			}

			page++;
		} while (await gameRecords.AnyAsync(
			cancellationToken: cancellationToken)
		.ConfigureAwait(false));
	}
}