namespace S1.SabaSports.OddsDirectAPI.Models;

public class Matches
{
    public IEnumerable<Event> Events { get; init; } = default!;

    public IEnumerable<Market> Markets { get; init; } = default!;
}
