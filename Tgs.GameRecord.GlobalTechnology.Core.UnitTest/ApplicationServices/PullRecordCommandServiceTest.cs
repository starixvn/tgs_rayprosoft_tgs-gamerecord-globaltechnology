using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Client;
using Tgs.GameInterface.GlobalTechnology.Protos;

using Tgs.GameRecord.GlobalTechnology.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameRecord.GlobalTechnology.Core.UnitTest.ApplicationServices;

public class PullRecordCommandServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var faceLogger = NullLogger<PullRecordCommandService>.Instance;
		var fakeClient = Substitute.For<IGameInterfaceGlobalTechnologyClient>();
		var fakeService = Substitute.For<ICommandService<HandleRecordCommand>>();

		var command = new PullRecordCommand(
			GameProviderCode: "GT_PT",
			ApiUrl: new Uri("http://gm-stage-data.gmgiantgold.com/"),
			BrandId: 908,
			SecretKey: "SMT1ZCVRI3uqtOXNV93q",
			StartTime: DateTimeOffset.Parse("2024-07-08T15:00:00+08:00"),
			EndTime: DateTimeOffset.Parse("2024-07-08T15:15:00+08:00"));

		_ = fakeClient.GameRecordAsync(
			dataApiUrl: Arg.Is(command.ApiUrl),
			brandId: Arg.Is(command.BrandId),
			secretKey: Arg.Is(command.SecretKey),
			startTime: Arg.Is(command.StartTime),
			endTime: Arg.Is(command.EndTime),
			page: Arg.Any<int>(),
			pageSize: Arg.Any<int>(),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new[] { new GameRecordInfo() }.ToAsyncEnumerable(), Enumerable.Empty<GameRecordInfo>().ToAsyncEnumerable());

		var sut = new PullRecordCommandService(
			faceLogger,
			fakeClient,
			fakeService);

		await sut.ExecuteAsync(command);

		_ = fakeService
			.Received(1)
			.ExecuteAsync(
			command: Arg.Is<HandleRecordCommand>(p => p.GameProviderCode == command.GameProviderCode),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Empty()
	{
		var faceLogger = NullLogger<PullRecordCommandService>.Instance;
		var fakeClient = Substitute.For<IGameInterfaceGlobalTechnologyClient>();
		var fakeService = Substitute.For<ICommandService<HandleRecordCommand>>();

		var command = new PullRecordCommand(
			GameProviderCode: "GT_PT",
			ApiUrl: new Uri("http://gm-stage-data.gmgiantgold.com/"),
			BrandId: 908,
			SecretKey: "SMT1ZCVRI3uqtOXNV93q",
			StartTime: DateTimeOffset.Parse("2024-07-08T15:00:00+08:00"),
			EndTime: DateTimeOffset.Parse("2024-07-08T15:15:00+08:00"));

		_ = fakeClient.GameRecordAsync(
			dataApiUrl: Arg.Is(command.ApiUrl),
			brandId: Arg.Is(command.BrandId),
			secretKey: Arg.Is(command.SecretKey),
			startTime: Arg.Is(command.StartTime),
			endTime: Arg.Is(command.EndTime),
			page: Arg.Any<int>(),
			pageSize: Arg.Any<int>(),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(Enumerable.Empty<GameRecordInfo>().ToAsyncEnumerable());

		var sut = new PullRecordCommandService(
			faceLogger,
			fakeClient,
			fakeService);

		await sut.ExecuteAsync(command);

		_ = fakeService
			.Received(0)
			.ExecuteAsync(
			command: Arg.Is<HandleRecordCommand>(p => p.GameProviderCode == command.GameProviderCode),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
