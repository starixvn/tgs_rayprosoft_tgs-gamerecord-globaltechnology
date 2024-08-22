using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Tgs.GameRecord.Handle.Client;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.GlobalTechnology.Core.ApplicationServices;

internal class HandleRecordCommandService(
	ILogger<HandleRecordCommandService> logger,
	IOptions<PolicySettingsOptions> policySettingsOptions,
	IGameRecordHandleClient client) : ICommandService<HandleRecordCommand>
{
	private readonly PolicySettingsOptions _policySettingsOptions = policySettingsOptions.Value;

	public async ValueTask ExecuteAsync(HandleRecordCommand command, CancellationToken cancellationToken = default)
	{
		var source = new Handle.Common.ViewModels.GameRecordHandleViewModel
		{
			GameProviderCode = command.GameProviderCode,
			PlayerCode = command.Info.PlayerCode,
			ExternalRoundId = command.Info.ExternalBetId,
			ExternalBetId = command.Info.ExternalBetId,
			ExternalGameId = command.Info.ExternalGameId,
			ExternalCreatedOn = command.Info.ExternalCreatedOn.ToDateTimeOffset(),
			ExternalClosedOn = command.Info.ExternalClosedOn.ToDateTimeOffset(),
			ExternalPayoffOn = command.Info.ExternalClosedOn.ToDateTimeOffset(),
			ExternalResettleOn = DateTimeOffset.UnixEpoch,
			BetState = (command.Info.WinLoss.ToDecimal().GetValueOrDefault(0) > 0) ? Handle.Common.Protos.BetState.Win : Handle.Common.Protos.BetState.Loss,
			BetType = Handle.Common.Protos.BetType.Normal,
			FreeGameState = (command.Info.IsFreeGame || command.Info.IsBonusGame) ? Handle.Common.Protos.FreeGameState.FreeGame : Handle.Common.Protos.FreeGameState.None,
			GambleState = Handle.Common.Protos.GambleState.None,
			CashoutState = Handle.Common.Protos.CashoutState.None,
			ResettleState = Handle.Common.Protos.ResettleState.None,
			JackpotState = (command.Info.JackpotWin.ToDecimal().GetValueOrDefault(0) > 0) ? Handle.Common.Protos.JackpotState.Jackpot : Handle.Common.Protos.JackpotState.None,
			Bet = command.Info.Bet.ToDecimal().GetValueOrDefault(0),
			ValidBet = command.Info.Bet.ToDecimal().GetValueOrDefault(0),
			Settle = command.Info.Settle.ToDecimal().GetValueOrDefault(0),
			Commission = 0.0m,
			Turnover = command.Info.Bet.ToDecimal().GetValueOrDefault(0),
			WinLoss = command.Info.WinLoss.ToDecimal().GetValueOrDefault(0),
			Jackpot = command.Info.JackpotWin.ToDecimal().GetValueOrDefault(0),
			AdditionalData = command.Info.AdditionalData
		};

		var retryCount = _policySettingsOptions.RetryCount;
		var retrySeconds = _policySettingsOptions.RetrySeconds;
		var retryPolicy = Policy
			.Handle<HttpRequestException>()
			.WaitAndRetryAsync(
				retryCount: retryCount,
				retryAttempt => TimeSpan.FromSeconds(Math.Pow(retrySeconds, retryAttempt)),
				onRetry: (exception, retryTime, retryCount, context) => logger.LogWarning(exception, "{logWarning}",
					new
					{
						LogOn = DateTime.Now.ToString("u"),
						RetryTime = retryTime,
						RetryCount = retryCount,
						Data = new
						{
							source.GameProviderCode,
							source.PlayerCode,
							source.ExternalRoundId,
							source.ExternalBetId,
							source.ExternalGameId,
							ExternalCreatedOn = source.ExternalCreatedOn.ToString("u"),
							ExternalClosedOn = source.ExternalClosedOn.ToString("u"),
							source.BetState
						},
						exception.Message
					}));

		await retryPolicy.ExecuteAsync(() => client.HandleAsync(
			source: source,
			cancellationToken: cancellationToken).AsTask())
			.ConfigureAwait(false);
	}
}