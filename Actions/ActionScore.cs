namespace OvertakerPlugin.Actions;

public readonly struct ActionScore
{
    public required uint Score { get; init; }
    public required DateTime HappenedAt { get; init; }

    public override string ToString()
    {
        return $"ActionScore: {Score}@{HappenedAt}";
    }
}
