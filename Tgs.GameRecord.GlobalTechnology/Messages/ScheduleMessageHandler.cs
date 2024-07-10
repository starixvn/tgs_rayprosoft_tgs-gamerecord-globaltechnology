using System.Text;
using System.Text.Json;
using Tgs.GameRecord.GlobalTechnology.Common;
using Tgs.GameRecord.GlobalTechnology.Core;
using Tgs.GameRecord.GlobalTechnology.Core.ApplicationServices;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.GlobalTechnology.Messages;

public class ScheduleMessageHandler(
	ILogger<ScheduleMessageHandler> logger,
	IPullRecordDispatcher dispatcher) : IMessageHandler<ReadOnlyMemory<byte>>
{
	public ValueTask HandleAsync(string subject, ReadOnlyMemory<byte> data, IEnumerable<MessageHeaderValue>? headerValues, CancellationToken cancellationToken = default)
	{
		var json = Encoding.UTF8.GetString(data.Span);
		var message = JsonSerializer.Deserialize<GlobalTechnologyGameRecordMessage>(json)
			?? throw new NullReferenceException("The request data resolves to a null value");

		logger.LogInformation("{logInformation}",
			JsonSerializer.Serialize(new
			{
				LogOn = DateTime.Now.ToString("u"),
				Subject = subject,
				Data = new
				{
					message.GameProviderCode,
					message.DataApiUrl,
					message.BrandId,
					message.SecretKey,
					StartTime = message.StartTime.ToString("u"),
					EndTime = message.EndTime.ToString("u")
				},
				Message = "This receive message handler."
			}));

		return dispatcher.PushCommandAsync(
			command: new PullRecordCommand(
				GameProviderCode: message.GameProviderCode,
				ApiUrl: message.DataApiUrl,
				BrandId: message.BrandId,
				SecretKey: message.SecretKey,
				StartTime: message.StartTime,
				EndTime: message.EndTime),
			cancellationToken: cancellationToken);
	}
}