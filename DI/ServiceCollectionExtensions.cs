using MeteoEmulator.Handlers;
using MeteoEmulator.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace MeteoEmulator.DI
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStateStorage(this IServiceCollection collection)
            => collection.AddSingleton<StateStorage>();

        public static IServiceCollection AddHandlers(this IServiceCollection collection)
            => collection.AddSingleton<DataSender>()
                .AddSingleton<DataGenerator>();
    }
}
