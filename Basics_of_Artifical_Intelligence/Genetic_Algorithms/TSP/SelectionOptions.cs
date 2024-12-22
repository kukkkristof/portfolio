// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace TSP;

public class SelectionOptions
{
    public required int NumberOfRoutes { get; set; }
    public required int NumberOfElites { get; set; }
    public required SurvivabilityOptions SurvivabilityOptions { get; set; }
    
    public required SelectionType Type { get; init; }
    public enum SelectionType
    {
        Tournament,
        RouletteWheel
    }
    
}