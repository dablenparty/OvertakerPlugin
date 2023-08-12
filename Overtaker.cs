using AssettoServer.Server.Plugin;
using AssettoServer.Shared.Services;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace OvertakerPlugin;

public class Overtaker : CriticalBackgroundService, IAssettoServerAutostart
{
    public Overtaker(OvertakerConfiguration configuration, IHostApplicationLifetime applicationLifetime) : base(
        applicationLifetime)
    {
        Log.Debug("Overtaker plugin starting with minimum speed {MinimumSpeedKmh} km/h",
            configuration.MinimumSpeedKmh);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // basically, this is the main loop of the plugin
        // from what I can tell, it's equivalent to acMain() in the Python sdk
        return Task.CompletedTask;
    }
}
