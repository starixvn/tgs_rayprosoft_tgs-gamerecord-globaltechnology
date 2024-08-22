using System.Text.Json.Nodes;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Protos;
using Tgs.GameRecord.GlobalTechnology.Core.ApplicationServices;
using Tgs.GameRecord.Handle.Client;

namespace Tgs.GameRecord.GlobalTechnology.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

		var gameRecordInfo = new GameRecordInfo
		{
			PlayerCode = "4ucmqplxb029",
			ExternalBetId = "live_87615669175",
			ExternalGameId = "ubal",
			ExternalCreatedOn = new DateTimeOffset(2024, 07, 09, 02, 50, 39, 000, TimeSpan.Zero).ToTimestamp(),
			ExternalClosedOn = new DateTimeOffset(2024, 07, 09, 02, 50, 52, 000, TimeSpan.Zero).ToTimestamp(),
			IsFreeGame = false,
			IsBonusGame = false,
			Bet = 10.0m,
			Settle = 20.0m,
			WinLoss = 10.0m,
			JackpotContribution = 0.0m,
			JackpotWin = 0.0m,
			AdditionalData = JsonNode.Parse("""
			{
				"roundId": "live_87615669175",
				"playerId": "4ucmqplxb029",
				"brandId": 908,
				"providerCode": "pt",
				"sessionId": "25e1afd4d45c46ffb8dbbbd75fc5da7b",
				"roundType": "normal",
				"roundStatus": "end",
				"gameCode": "ubal",
				"currency": "CNY",
				"platform": "mobile",
				"bets": 10,
				"wins": 20,
				"cancels": 0,
				"transIn": 0,
				"transOut": 0,
				"balanceEnd": 99.55,
				"bonusBalanceEnd": 0,
				"rake": 0,
				"startTime": "2024-07-09 02:50:39.000",
				"endTime": "2024-07-09 02:50:52.000",
				"jpc": 0,
				"jpw": 0,
				"additionalData": {
					"type": "json",
					"value": "{\"launchAlias\":\"7bal_loungebaccarat\",\"tableId\":\"1609\"}",
					"null": true
				},
				"detailUrl": "https://extstg3-cashier01.ptstaging.eu/getgamehistoryurl.php?casino=ptstaging3.33&username=1CLUB__PEG_4ucmqplxb029&ngscode=21484360&remotesessionenddate=2024-07-09+02%3A50%3A52&token=7ae5b3bc0129689f29ce281dfb6608865e0c6c9a4e25155f1f29946d67448ab4&language=EN&showFullHistoryPerBet=false",
				"gameType": "live"
			}
			""")!.ToJsonString()
		};

		var command = new HandleRecordCommand(
			GameProviderCode: "GT_PT",
			Info: gameRecordInfo);

		var service = new HandleRecordCommandService(
			faceLogger,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await service.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
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
				BetState = Handle.Common.Protos.BetState.Win,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = command.Info.Bet.ToDecimal().GetValueOrDefault(0),
				ValidBet = command.Info.Bet.ToDecimal().GetValueOrDefault(0),
				Settle = command.Info.Settle.ToDecimal().GetValueOrDefault(0),
				Commission = 0.0m,
				Turnover = command.Info.Bet.ToDecimal().GetValueOrDefault(0),
				WinLoss = command.Info.WinLoss.ToDecimal().GetValueOrDefault(0),
				Jackpot = command.Info.JackpotWin.ToDecimal().GetValueOrDefault(0),
				AdditionalData = command.Info.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
