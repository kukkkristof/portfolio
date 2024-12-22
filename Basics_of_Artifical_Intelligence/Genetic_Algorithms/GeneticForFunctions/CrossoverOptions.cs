namespace GeneticForFunctions;

public class CrossoverOptions
{
    public required int NumberOfCrossovers { get; set; }
    public required int NumberOfCrossoverPoints { get; set; }
    public required CrossoverType Type { get; set; }
    
    public enum CrossoverType
    {
        SinglePoint,
        MultiPoint,
        Uniform,
        PathRelinking
    }
    
}