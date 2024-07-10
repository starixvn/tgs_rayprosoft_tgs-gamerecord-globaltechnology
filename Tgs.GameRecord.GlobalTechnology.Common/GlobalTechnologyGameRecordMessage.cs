using System.ComponentModel.DataAnnotations;

namespace Tgs.GameRecord.GlobalTechnology.Common;

public record GlobalTechnologyGameRecordMessage
{
	/// <summary>
	/// The game provider code
	/// </summary>
	/// <example>GT_PT</example>
	[Required]
	public string GameProviderCode { get; init; } = default!;
	/// <summary>
	/// The data API URL
	/// </summary>
	/// <example>http://gm-stage-data.gmgiantgold.com/</example>
	[Required]
	[DataType(DataType.Url)]
	public Uri DataApiUrl { get; init; } = default!;
	/// <summary>
	/// The brand identifier
	/// </summary>
	/// <example>908</example>
	[Required]
	public int BrandId { get; init; } = default!;
	/// <summary>
	/// The secret key
	/// </summary>
	/// <example>SMT1ZCVRI3uqtOXNV93q</example>
	[Required]
	public string SecretKey { get; init; } = default!;
	/// <summary>
	/// The start time
	/// </summary>
	/// <example>2024-07-08T15:00:00+08:00</example>
	[Required]
	[DataType(DataType.DateTime)]
	public DateTimeOffset StartTime { get; init; }
	/// <summary>
	/// The end time
	/// </summary>
	/// <example>2024-07-08T15:15:00+08:00</example>
	[Required]
	[DataType(DataType.DateTime)]
	public DateTimeOffset EndTime { get; init; }
}