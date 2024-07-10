using System.Text;
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
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///			{
	///				"gameProviderCode": "GT_PT",
	///				"dataApiUrl": "http://gm-stage-data.gmgiantgold.com/",
	///				"brandId": 908,
	///				"secretKey": "SMT1ZCVRI3uqtOXNV93q",
	///				"startTime": "2024-07-08T15:00:00+08:00",
	///				"endTime": "2024-07-08T15:15:00+08:00"
	///			}
	/// </remarks>
	[HttpPost("manual")]
	public ValueTask GameRecordManualAsync(
			[FromServices] IMessageSender messageSender,
			[FromBody] GlobalTechnologyGameRecordMessage source)
			=> messageSender.PublishAsync(
				subject: GlobalTechnologyNatsConst.ManualPublish,
				data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
				cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the record schedule asynchronous.
	/// </summary>
	/// <param name="messageSender">The message sender.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///			{
	///				"gameProviderCode": "GT_PT",
	///				"dataApiUrl": "http://gm-stage-data.gmgiantgold.com/",
	///				"brandId": 908,
	///				"secretKey": "SMT1ZCVRI3uqtOXNV93q",
	///				"startTime": "2024-07-08T15:00:00+08:00",
	///				"endTime": "2024-07-08T15:15:00+08:00"
	///			}
	/// </remarks>
	[HttpPost("schedule")]
	public ValueTask GameRecordScheduleAsync(
		[FromServices] IMessageSender messageSender,
		[FromBody] GlobalTechnologyGameRecordMessage source)
		=> messageSender.PublishAsync(
			subject: GlobalTechnologyNatsConst.SchedulePublish,
			data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
			cancellationToken: HttpContext.RequestAborted);
}
