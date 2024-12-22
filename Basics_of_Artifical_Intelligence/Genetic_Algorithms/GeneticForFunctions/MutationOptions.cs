// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace GeneticForFunctions;

public class MutationOptions
{

    public required int NumberOfMutations { get; set; }
    public required MutationType Type { get; set; }
    
    public enum MutationType
    {
        SinglePoint,
        FullPoint
    }
    
}