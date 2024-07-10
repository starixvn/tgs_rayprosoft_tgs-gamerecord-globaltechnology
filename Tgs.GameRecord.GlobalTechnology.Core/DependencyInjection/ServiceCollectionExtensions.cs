using System.Reflection;
using Tgs.GameCommand.Client;
using Tgs.GameInterface.GlobalTechnology.Client;
using Tgs.GameRecord.GlobalTechnology.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameRecordCore(
		this IServiceCollection services,
		Action<GameCommandClientOptions, IServiceProvider> gameCommandClientOptions,
		Action<GameInterfaceGlobalTechnologyClientOptions, IServiceProvider> gameInterfaceGlobalTechnologyClientOptions)
		=> services
		.RegisterAllServiceUtilityTypes(
			Assembly.GetExecutingAssembly(),
			ServiceLifetime.Transient)
		.AddGameCommandClient(gameCommandClientOptions)
		.AddGameInterfaceGlobalTechnologyClient(gameInterfaceGlobalTechnologyClientOptions)
		.AddSingleton<IPullRecordDispatcher, PullRecordDispatcher>()
		.AddTransient<IHangfireUtensil, HangfireUtensil>();
}