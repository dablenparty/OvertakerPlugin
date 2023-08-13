using System.Numerics;
using AssettoServer.Network.Tcp;
using AssettoServer.Server;
using AssettoServer.Server.Plugin;
using AssettoServer.Shared.Services;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace OvertakerPlugin;

public class Overtaker : CriticalBackgroundService, IAssettoServerAutostart
{
    private readonly ILogger _logger = Log.ForContext<Overtaker>();
    private readonly OvertakerConfiguration _configuration;
    private readonly EntryCarManager _entryCarManager;
    private readonly Dictionary<string, int> _scores = new();

    public Overtaker(OvertakerConfiguration configuration, EntryCarManager entryCarManager,
        IHostApplicationLifetime applicationLifetime) : base(
        applicationLifetime)
    {
        _configuration = configuration;
        _entryCarManager = entryCarManager;
        _entryCarManager.ClientConnected += (sender, _) => sender.FirstUpdateSent += OnClientFirstUpdateSent;
        _entryCarManager.ClientDisconnected += OnClientDisconnected;
        // TODO: load scores from file
    }

    private void OnClientDisconnected(ACTcpClient sender, EventArgs args)
    {
        // TODO: save score to file
        _scores.Remove(sender.HashedGuid);
    }

    private void OnClientFirstUpdateSent(ACTcpClient sender, EventArgs args)
    {
        _scores.Add(sender.HashedGuid, 0);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // maybe this is possible, maybe not
            // TODO: keep track of player controlled cars separately for more efficient access
            foreach (var entryCar in _entryCarManager.EntryCars)
            {
                var speedKmh = OvertakerUtils.KmhFromAcVelocity(entryCar.Status.Velocity);
                if (entryCar.AiControlled || speedKmh < _configuration.MinimumSpeedKmh)
                    continue;
                // this is just proof-of-concept at the moment showing how to get nearby cars using Microsoft's
                // (dogshit) built-in Vector3 class
                var nearbyCars = _entryCarManager.EntryCars
                    .Where(c => c != entryCar && Vector3.Distance(c.Status.Position, entryCar.Status.Position) <=
                        _configuration.MinimumDistanceMeters)
                    .ToList();
                if (nearbyCars.Count == 0)
                    continue;
                // TODO: score overtakes
            }
        }

        // TODO: save scores to file
        return Task.CompletedTask;
    }
}
