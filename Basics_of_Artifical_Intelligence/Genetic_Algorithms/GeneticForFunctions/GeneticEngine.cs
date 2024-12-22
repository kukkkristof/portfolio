// ReSharper disable PropertyCanBeMadeInitOnly.Global

using System.Diagnostics;
using System.Reflection;

namespace GeneticForFunctions;

public class GeneticEngine
{
    
    public required int GenerationCount { get; set; }
    
    public required Problem Problem { get; set; }
    
    public required MutationOptions MutationOptions { get; set; }
    public required CrossoverOptions CrossoverOptions { get; set; }
    public required SelectionOptions SelectionOptions { get; set; }
    
    private Population _population = new();

    public bool DumpResult { get; set; }
    public string? DumpFilePath { get; set; } 
    public bool AppendMode { get; set; }
    public string? ProblemName { get; set; }
    public bool DumpEverything { get; set; }
    public required double InitializationValue { get; init; }

    private Chromosome _firstBest;

    private double _runtime = 0; // ms 
    
    private Chromosome[] _bests = [];
    private Chromosome[] _worsts = [];
    private double[] _averages = [];
    private double[] _deviations = [];
    
    public object Get(string propertyName)
    {
        PropertyInfo? propertyInfo = this.GetType().GetProperty(propertyName);
        
        if (propertyInfo != null)
        {
            return propertyInfo.GetValue(this); // Return the value of the field
        }

        throw new ArgumentException($"Property '{propertyName}' not found.");
    }
    
    
    public void Run()
    {
        Init();
        Stopwatch watch = new();
        watch.Start();
        for (int generation = 0; generation < GenerationCount; generation++)
        {
            Mutate();
            Cross();
            Select();

            if (DumpEverything)
            {
                _bests[generation] = _population.Best.Copy;
                _worsts[generation] = _population.Worst.Copy;
                _averages[generation] = _population.Average;
                _deviations[generation] = _population.Deviation;
            }
            
        }
        watch.Stop();

        _runtime = watch.Elapsed.TotalMilliseconds;
        if (DumpResult)
        {
            Dump();
        }
        else
        {
            Console.WriteLine("BEST: {0}", _population.Best);
        }
        
    }
    
    private void Init()
    {
        _population = new Population();
        for (int i = 0; i < SelectionOptions.NumberOfIndividuals; i++)
        {
            double[] values = new double[Problem.GenomeCount];
            for (int j = 0; j < values.Length; j++)
            {
                values[j] = InitializationValue;
            }
            Chromosome chromosome = new()
            {
                Values = values
            };
            chromosome.Value = Problem.Evaluate(chromosome);
            chromosome.Fitness = Problem.GetFitness(chromosome);
            
            _population.Individuals.Add(chromosome);
        }
        _firstBest = _population.Individuals.OrderBy(x => x.Fitness).First();
        if (DumpEverything)
        {
            _bests = new Chromosome[GenerationCount];
            _worsts = new Chromosome[GenerationCount];
            _averages = new double[GenerationCount];
            _deviations = new double[GenerationCount];
        }
        
    }

    private void Mutate()
    {
        for (int mutation = 0; mutation < MutationOptions.NumberOfMutations; mutation++)
        {
            _population.Individuals.Add(
                Mutation.Mutate(_population.Random,
                    MutationOptions,
                    Problem)
            );
        }
    }

    private void Cross()
    {
        for (int crossover = 0; crossover < CrossoverOptions.NumberOfCrossovers; crossover++)
        {
            _population.Individuals.Add(
                Crossover.Cross(
                    _population.Random,
                    _population.Random,
                    CrossoverOptions,
                    Problem)
            );
        }
    }

    private void Select()
    {
        _population = Selection.Select(_population, SelectionOptions);
    }

    private void Dump()
    {
        if(DumpFilePath == null) throw new Exception("There is no dump file path!");

        List<string> pathFolders = DumpFilePath.Split('/').ToList();
        
        for (int i = 0; i < pathFolders.Count-1; i++)
        {
            if (!Directory.Exists(string.Join("/",pathFolders.GetRange(0,i+1))))
            {
                Directory.CreateDirectory(string.Join("/",pathFolders.GetRange(0,i+1)));
            }
        }
        
        
        if (!DumpEverything)
        {
            StreamWriter writer = new(DumpFilePath, AppendMode);

            if (!AppendMode)
                writer.WriteLine($"Problem\tPopulation Size\tGenerations\tStep Size\tElites\tMutations\tCrossovers" +
                                 $"\tFirst\tBest\tFitness\tCrossover Points\tRuntime (ms)");

            writer.WriteLine(
                $"{ProblemName}" +
                $"\t{SelectionOptions.NumberOfIndividuals}" +
                $"\t{GenerationCount}" +
                $"\t{Problem.StepSize}" +
                $"\t{SelectionOptions.NumberOfElites}" +
                $"\t{MutationOptions.NumberOfMutations}" +
                $"\t{CrossoverOptions.NumberOfCrossovers}" +
                $"\tf({string.Join(", ", _firstBest.Values)}) = {_firstBest.Value}" +
                $"\tf({string.Join(", ", _population.Best.Values)}) = {_population.Best.Value}" +
                $"\t{_population.Best.Fitness}" +
                $"\t{CrossoverOptions.NumberOfCrossoverPoints}" +
                $"\t{_runtime}"
            );

            writer.Flush();
            writer.Close();
        }
        else
        {
            if (_bests == null ||
                _worsts == null ||
                _averages == null ||
                _deviations == null)
            {
                throw new Exception("There is an error with the dump data!");
            }
            
            StreamWriter writer = new(DumpFilePath);

            writer.WriteLine("Problem\tPopulation Size\tGeneration\tStep Size\tElites" +
                             "\tMutations\tCrossovers\tBest\tWorst\tBest Fitness\tWorst Fitness\tAverage\tDeviation" +
                             "\tCrossover Points" +
                             "\tRuntime (ms)");

            
            
            for (int gen = 0; gen < GenerationCount; gen++)
            {


                writer.WriteLine(
                    $"{ProblemName}\t{SelectionOptions.NumberOfIndividuals}" +
                    $"\t{gen + 1}\t{Problem.StepSize}\t{MutationOptions.NumberOfMutations}" +
                    $"\t{CrossoverOptions.NumberOfCrossovers}" +
                    $"\t{SelectionOptions.NumberOfElites}" +
                    $"\tf({string.Join(", ", _bests[gen].Values)}) = {_bests[gen].Value}" +
                    $"\tf({string.Join(", ", _worsts[gen].Values)}) = {_worsts[gen].Value}" +
                    $"\t{_bests[gen].Fitness}" +
                    $"\t{_worsts[gen].Fitness}" +
                    $"\t{_averages[gen]}" +
                    $"\t{_deviations[gen]}" +
                    $"\t{CrossoverOptions.NumberOfCrossoverPoints}" +
                    $"\t{_runtime}"
                );
            }

            writer.Flush();
            writer.Close();
        }

    }
    
}