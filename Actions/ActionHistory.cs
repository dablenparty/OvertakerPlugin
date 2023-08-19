using System.Collections;
using Serilog;

namespace OvertakerPlugin.Actions;

public class ActionHistory : IEnumerable<ActionScore>
{
    private static readonly ILogger Logger = Log.ForContext<ActionHistory>();
    private readonly List<ActionScore> _scores = new();

    public ActionScore this[int index] => _scores[index];

    public uint TotalScore => _scores.Aggregate(0u, (current, score) => current + score.Score);

    public static ActionHistory operator +(ActionHistory first, ActionHistory other)
    {
        first.Extend(other);
        return first;
    }

    public override string ToString()
    {
        return $"ActionHistory: {TotalScore} points, {_scores.Count} actions";
    }

    public void Add(ActionScore score)
    {
        _scores.Add(score);
    }

    public void Extend(ActionHistory other)
    {
        _scores.AddRange(other._scores);
    }

    public IEnumerator<ActionScore> GetEnumerator()
    {
        return _scores.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
