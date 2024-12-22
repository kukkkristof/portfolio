namespace TSP;

public class SurvivabilityOptions
{
    public required double P { get; init; }

    public required SurvivabilityType Type { get; init; }
    
    public enum SurvivabilityType
    {
        Relative,
        Ranked,
        Diverse
    }
}