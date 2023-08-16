using AssettoServer.Network.Tcp;
using AssettoServer.Server;
using AssettoServer.Server.Plugin;
using AssettoServer.Shared.Services;
using Microsoft.Extensions.Hosting;
using OvertakerPlugin.Actions;
using OvertakerPlugin.State;
using Serilog;

namespace OvertakerPlugin;

public class Overtaker : CriticalBackgroundService, IAssettoServerAutostart
{
    private readonly ActionHistory _actionHistory;
    private readonly EntryCarManager _entryCarManager;
    private readonly ILogger _logger = Log.ForContext<Overtaker>();
    private readonly Dictionary<string, uint> _scores = new();

    public Overtaker(OvertakerConfiguration configuration, EntryCarManager entryCarManager,
        IHostApplicationLifetime applicationLifetime) : base(
        applicationLifetime)
    {
        _entryCarManager = entryCarManager;
        _entryCarManager.ClientConnected += (sender, _) =>
        {
            sender.FirstUpdateSent += OnClientFirstUpdateSent;
            sender.Collision += OnClientCollision;
        };
        _entryCarManager.ClientDisconnected += OnClientDisconnected;
        _actionHistory = new ActionHistory(configuration);
        _logger.Information("Loaded {ConfigType}: {@Config}", nameof(OvertakerConfiguration), configuration);
        // TODO: load scores from file
    }

    private void OnClientCollision(ACTcpClient sender, CollisionEventArgs args)
    {
        // TODO: save score to file
        _scores[sender.HashedGuid] = 0;
        // environment collision
        // if (args.TargetCar is null)
        //     return;
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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                StateHistory.NewTickHappened(_entryCarManager);
                // maybe this is possible, maybe not
                // TODO: keep track of player controlled cars separately for more efficient access
                // TODO: parallelize this. HEAVILY CONSIDER ASYNC/AWAIT
                var scoreUpdates = _actionHistory.ScoreAllActions();
                foreach (var (key, value) in scoreUpdates)
                    if (_scores.TryGetValue(key, out var score))
                        _scores[key] = score + value;
                    else
                        _scores.Add(key, value);
            }
            // TODO: save scores to file
        }, stoppingToken);
    }
}
