﻿using OvertakerPlugin.State;

namespace OvertakerPlugin.Actions;

public abstract class AbstractOvertakerAction
{
    protected readonly OvertakerConfiguration Configuration;


    protected AbstractOvertakerAction(OvertakerConfiguration configuration)
    {
        Configuration = configuration;
    }

    public abstract string Name { get; init; }
    public abstract int Value { get; init; }

    /// <summary>
    ///     Scores an action based on the state history. This method should return a Dictionary of score updates, meaning
    ///     the key is the client session ID and the value will be added to that client's total current score.
    /// </summary>
    /// <param name="stateHistory">State history. See StateHistory.NeedsHistory for how to populate this.</param>
    /// <returns>Score updates</returns>
    public abstract Dictionary<byte, uint> ScoreAction(List<TickState> stateHistory);
}
