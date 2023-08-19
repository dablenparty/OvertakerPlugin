using System.Reflection;
using CommandLine;
using OvertakerPlugin.State;
using OvertakerPlugin.Utility;
using Serilog;

namespace OvertakerPlugin.Actions;

public class ActionRunner : LazySingleton<ActionRunner>
{
    private static readonly ILogger Logger = Log.ForContext<ActionRunner>();
    private readonly Dictionary<string, AbstractOvertakerAction> _registeredActions = new();

    private ActionRunner()
    {
    }

    public static void RegisterAction(AbstractOvertakerAction action)
    {
        Instance._registeredActions[action.Name] = action;
    }

    /// <summary>
    ///     Scores every registered action for every player.
    /// </summary>
    /// <returns>Players and their score updates, associated by session ID</returns>
    public Dictionary<byte, ActionHistory> ScoreAllActions()
    {
        var scoreUpdates = new Dictionary<byte, ActionHistory>();
        foreach (var (name, action) in _registeredActions)
        {
            // this really really long line gets the StateCount value from the NeedsHistoryAttribute on the first
            // parameter of the ScoreAction method, or 1 if the attribute is not present
            var statesNeeded = action.GetType().GetMethod(nameof(AbstractOvertakerAction.ScoreAction))?.GetParameters()
                .FirstOrDefault()
                ?.GetCustomAttribute(typeof(StateHistory.NeedsHistoryAttribute), true)
                ?.Cast<StateHistory.NeedsHistoryAttribute>()?.StateCount ?? 1;
            // actions depend on there being a certain number of states in the history; so, if there aren't enough
            // states, we can't score the action
            var availableStateCount = StateHistory.Instance.TickStates.Count;
            if (availableStateCount < statesNeeded)
            {
                Logger.Warning(
                    "\"{ActionName}\" action needs {StatesNeeded} states, but only {StatesAvailable} are available",
                    name, statesNeeded, availableStateCount);
                continue;
            }

            var stateHistory = StateHistory.Instance.TickStates
                .Reverse()
                .Take(statesNeeded)
                .ToList();
            var innerScoreUpdates = action.ScoreAction(stateHistory);
            if (innerScoreUpdates.Count == 0)
                continue;
            Logger.Debug("Action {ActionName} scored {ScoreUpdates}", name, innerScoreUpdates);
            foreach (var (sessionId, scoreUpdate) in innerScoreUpdates)
                if (scoreUpdates.TryGetValue(sessionId, out var existingScoreUpdate))
                    existingScoreUpdate.Add(scoreUpdate);
                else
                    scoreUpdates[sessionId] = new ActionHistory {scoreUpdate};
        }

        return scoreUpdates;
    }
}
