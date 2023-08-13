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

    public abstract int ScoreAction(List<TickState> stateHistory);
}
