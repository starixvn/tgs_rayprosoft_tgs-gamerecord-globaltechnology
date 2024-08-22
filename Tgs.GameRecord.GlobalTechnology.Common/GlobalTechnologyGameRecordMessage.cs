using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tgs.GameRecord.GlobalTechnology.Common;

public record GlobalTechnologyGameRecordMessage
{
	/// <summary>
	/// The game provider code
	/// </summary>
	[Required]
	[DefaultValue(typeof(string), "GT_PT")]
	public string GameProviderCode { get; init; } = default!;
	/// <summary>
	/// The data API URL
	/// </summary>
	[Required]
	[DataType(DataType.Url)]
	[DefaultValue(typeof(Uri), "http://gm-stage-data.gmgiantgold.com/")]
	public Uri DataApiUrl { get; init; } = default!;
	/// <summary>
	/// The brand identifier
	/// </summary>
	[Required]
	[DefaultValue(typeof(int), "908")]
	public int BrandId { get; init; } = default!;
	/// <summary>
	/// The secret key
	/// </summary>
	[Required]
	[DefaultValue(typeof(string), "SMT1ZCVRI3uqtOXNV93q")]
	public string SecretKey { get; init; } = default!;
	/// <summary>
	/// The start time
	/// </summary>
	[Required]
	[DataType(DataType.DateTime)]
	[DefaultValue(typeof(DateTimeOffset), "2024-07-08T15:00:00+08:00")]
	public DateTimeOffset StartTime { get; init; }
	/// <summary>
	/// The end time
	/// </summary>
	[Required]
	[DataType(DataType.DateTime)]
	[DefaultValue(typeof(DateTimeOffset), "2024-07-08T15:15:00+08:00")]
	public DateTimeOffset EndTime { get; init; }
}