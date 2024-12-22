// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace GeneticForFunctions;

public class SelectionOptions
{
    public required int NumberOfIndividuals { get; set; }
    public required int NumberOfElites { get; set; }
    public required SurvivabilityOptions SurvivabilityOptions { get; set; }
    
    public required SelectionType Type { get; init; }
    public enum SelectionType
    {
        Tournament,
        RouletteWheel
    }
    
}