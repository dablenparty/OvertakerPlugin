using OvertakerPlugin.State;

namespace OvertakerPlugin.Actions;

public abstract class Action
{
    protected readonly OvertakerConfiguration Configuration;
    public abstract string Name { get; init; }
    public abstract int Value { get; init; }


    protected Action(OvertakerConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Scores an action based on the state history. This method should return a Dictionary of score updates, meaning
    /// the key is the client GUID hash and the value will be added to that client's total current score.
    /// </summary>
    /// <param name="stateHistory">State history. See StateHistory.NeedsHistory for how to populate this.</param>
    /// <returns>Score updates</returns>
    public abstract Dictionary<string, uint> ScoreAction(List<TickState> stateHistory);
}
