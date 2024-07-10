namespace Tgs.GameRecord.GlobalTechnology;

public record RedisOptions
{
	public string ConnectionString { get; set; } = "localhost";
}
