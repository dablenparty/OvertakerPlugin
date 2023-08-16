using System.Reflection;
using CommandLine;
using OvertakerPlugin.State;
using Serilog;

namespace OvertakerPlugin.Actions;

public class ActionHistory
{
    private readonly ILogger _logger = Log.ForContext<ActionHistory>();
    private readonly Dictionary<string, AbstractOvertakerAction> _registeredActions = new();

    public ActionHistory(OvertakerConfiguration configuration)
    {
        _registeredActions.Add("Overtake", new OvertakeAction(configuration));
    }

    public Dictionary<byte, uint> ScoreAllActions()
    {
        var scoreUpdates = new Dictionary<byte, uint>();
        foreach (var (name, action) in _registeredActions)
        {
            // this really really long line gets the StateCount value from the NeedsHistoryAttribute on the first
            // parameter of the ScoreAction method, or 1 if the attribute is not present
            var statesNeeded = action.GetType().GetMethod(nameof(AbstractOvertakerAction.ScoreAction))?.GetParameters().FirstOrDefault()
                ?.GetCustomAttribute(typeof(StateHistory.NeedsHistoryAttribute), true)
                .Cast<StateHistory.NeedsHistoryAttribute>()?.StateCount ?? 1;
            // actions depend on there being a certain number of states in the history; so, if there aren't enough
            // states, we can't score the action
            var availableStateCount = StateHistory.CurrentHistory.TickStates.Count;
            if (availableStateCount < statesNeeded)
            {
                _logger.Debug(
                    "\"{ActionName}\" action needs {StatesNeeded} states, but only {StatesAvailable} are available",
                    name, statesNeeded, availableStateCount);
                continue;
            }

            var stateHistory = StateHistory.CurrentHistory.TickStates
                .Take(statesNeeded)
                .ToList();
            var innerScoreUpdates = action.ScoreAction(stateHistory);
            if (innerScoreUpdates.Count == 0)
                continue;
            // TODO: add to history
            _logger.Debug("Action {ActionName} scored {ScoreUpdates}", name, innerScoreUpdates);
            foreach (var (key, value) in innerScoreUpdates)
            {
                
            }
        }

        return scoreUpdates;
    }
}
