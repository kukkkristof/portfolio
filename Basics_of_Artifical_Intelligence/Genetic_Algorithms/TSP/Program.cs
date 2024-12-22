using System.Diagnostics;
using TSP;

GeneticEngine engine = new GeneticEngine()
{
    Problem = new()
    {
        Cities = Problem.GenerateRandomCities(30, 2, -10, 10)
    },
    GenerationCount = 50,
    MutationCount = 25,
    CrossoverCount = 25,
    SelectionOptions = new()
    {
        NumberOfElites = 5,
        NumberOfRoutes = 50,
        SurvivabilityOptions = new()
        {
            P = 0.8,
            Type = SurvivabilityOptions.SurvivabilityType.Diverse
        },
        Type = SelectionOptions.SelectionType.RouletteWheel
    }
};

Stopwatch time = Stopwatch.StartNew();
time.Start();
engine.Run();
time.Stop();
Console.WriteLine($"Time elapsed: {time.ElapsedMilliseconds} ms");
