using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.GlobalTechnology.Core.ApplicationServices;

internal class HandleRecordCommandService(
	ILogger<HandleRecordCommandService> logger,
	IMessageSender messageSender) : ICommandService<HandleRecordCommand>
{
	public async ValueTask ExecuteAsync(HandleRecordCommand command, CancellationToken cancellationToken = default)
	{
		var request = new Handle.Common.Protos.GameRecordHandleRequest
		{
			GameProviderCode = command.GameProviderCode,
			PlayerCode = command.Info.PlayerCode,
			ExternalRoundId = command.Info.ExternalBetId,
			ExternalBetId = command.Info.ExternalBetId,
			ExternalGameId = command.Info.ExternalGameId,
			ExternalCreatedOn = command.Info.ExternalCreatedOn,
			ExternalClosedOn = command.Info.ExternalClosedOn,
			ExternalPayoffOn = command.Info.ExternalClosedOn,
			ExternalResettleOn = DateTimeOffset.UnixEpoch.ToTimestamp(),
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

		await messageSender.PublishAsync(
			subject: Handle.Common.GameRecordHandleNatsConst.GameRecordHandleRequest,
			data: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		logger.LogInformation("LogOn:{logOn} Subject:{subject} Mesaage:This publication requests information.({request})",
			DateTime.Now.ToString("o"),
			Handle.Common.GameRecordHandleNatsConst.GameRecordHandleRequest,
			new
			{
				request.GameProviderCode,
				request.PlayerCode,
				request.ExternalRoundId,
				request.ExternalBetId,
				request.ExternalGameId,
				ExternalCreatedOn = request.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = request.ExternalClosedOn.ToDateTimeOffset(),
				request.BetState
			});
	}
}