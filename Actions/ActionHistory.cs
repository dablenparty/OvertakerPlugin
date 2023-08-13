using Serilog;

namespace OvertakerPlugin.Actions;

public class ActionHistory
{
    private readonly ILogger _logger = Log.ForContext<ActionHistory>();
    private readonly Dictionary<string, Action> _registeredActions = new();

    public ActionHistory(OvertakerConfiguration configuration)
    {
        _registeredActions.Add("Overtake", new OvertakeAction(configuration));
    }
}
