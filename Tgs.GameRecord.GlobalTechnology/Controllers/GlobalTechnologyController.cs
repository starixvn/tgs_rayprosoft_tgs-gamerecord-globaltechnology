using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tgs.GameRecord.GlobalTechnology.Common;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.GlobalTechnology.Controllers;

[Route("api/gt")]
[ApiController]

public class GlobalTechnologyController : ControllerBase
{
	/// <summary>
	/// Games the record manual asynchronous.
	/// </summary>
	/// <param name="messageSender">The message sender.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("manual")]
	public ValueTask GameRecordManualAsync(
			[FromServices] IMessageSender messageSender,
			[FromBody] GlobalTechnologyGameRecordMessage source)
			=> messageSender.PublishAsync(
				subject: GlobalTechnologyNatsConst.ManualPublish,
				data: JsonSerializer.SerializeToUtf8Bytes(source),
				cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the record schedule asynchronous.
	/// </summary>
	/// <param name="messageSender">The message sender.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("schedule")]
	public ValueTask GameRecordScheduleAsync(
		[FromServices] IMessageSender messageSender,
		[FromBody] GlobalTechnologyGameRecordMessage source)
		=> messageSender.PublishAsync(
			subject: GlobalTechnologyNatsConst.SchedulePublish,
			data: JsonSerializer.SerializeToUtf8Bytes(source),
			cancellationToken: HttpContext.RequestAborted);
}
