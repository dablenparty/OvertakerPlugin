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
    private readonly EntryCarManager _entryCarManager;
    private readonly ILogger _logger = Log.ForContext<Overtaker>();
    private readonly Dictionary<byte, ActionHistory> _scores = new();

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
        ActionRunner.RegisterAction(new OvertakeAction(configuration));
        _logger.Information("Loaded {ConfigType}: {@Config}", nameof(OvertakerConfiguration), configuration);
        // TODO: load scores from file
    }

    private void OnClientCollision(ACTcpClient sender, CollisionEventArgs args)
    {
        // TODO: save score to file
        _scores[sender.SessionId] = new ActionHistory();
    }

    private void OnClientDisconnected(ACTcpClient sender, EventArgs args)
    {
        // TODO: save score to file
        var actionHistory = _scores[sender.SessionId];
        _scores.Remove(sender.SessionId);
        _logger.Information("Client {SessionId} disconnected. {History}", sender.SessionId, actionHistory.ToString());
    }

    private void OnClientFirstUpdateSent(ACTcpClient sender, EventArgs args)
    {
        // TODO: load high score from file
        _scores.Add(sender.SessionId, new ActionHistory());
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
            await Task.Run(() =>
            {
                StateHistory.NewTickHappened(_entryCarManager);
                // maybe this is possible, maybe not
                // TODO: keep track of player controlled cars separately for more efficient access
                // TODO: parallelize this. HEAVILY CONSIDER ASYNC/AWAIT
                var scoreUpdates = ActionRunner.Instance.ScoreAllActions();
                foreach (var (key, value) in scoreUpdates)
                    _scores[key] += value;
                // TODO: save scores to file
            }, stoppingToken);
    }
}
