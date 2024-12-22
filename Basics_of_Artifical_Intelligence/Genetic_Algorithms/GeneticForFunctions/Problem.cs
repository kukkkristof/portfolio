// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace GeneticForFunctions;

public class Problem
{
    public required double ExpectedResult { get; set; }
    public required double ValueRange { get; set; }
    public required int GenomeCount { get; set; }
    public required double StepSize { get; set; }
    public required Func<Chromosome, double> Evaluate { get; init; }

    public double GetFitness(Chromosome chromosome)
    {
        return 100.0 / (1 + Math.Abs( Evaluate(chromosome) - ExpectedResult ));
    }
    
}