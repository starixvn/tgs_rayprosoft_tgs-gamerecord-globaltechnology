using Tgs.GameRecord.GlobalTechnology.Core.ApplicationServices;

namespace Tgs.GameRecord.GlobalTechnology.Core;

public interface IPullRecordDispatcher
{
	ValueTask PushCommandAsync(PullRecordCommand command, CancellationToken cancellationToken = default);
}