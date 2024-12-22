using System.Diagnostics;
using static GeneticForFunctions.CrossoverOptions.CrossoverType;
using static GeneticForFunctions.ProblemFunctions;
using static GeneticForFunctions.SelectionOptions;
using static GeneticForFunctions.SurvivabilityOptions;

namespace GeneticForFunctions;

public static class Runner
{
    private static readonly Func<Chromosome, double>[] Functions =
    [
        Rastrigin,
        Booth,
        Levi
    ];
    
    public static void DebugRun()
    {
        GeneticEngine engine = new()
        {
            GenerationCount = 100,
            InitializationValue = -5,
            CrossoverOptions = new()
            {
                NumberOfCrossovers = 5,
                NumberOfCrossoverPoints = 1,
                Type = SinglePoint,
            },
            MutationOptions = new()
            {
                NumberOfMutations = 10,
                Type = MutationOptions.MutationType.SinglePoint
            },
            Problem = new()
            {
                ExpectedResult = 0,
                StepSize = 0.5,
                ValueRange = 10,
                GenomeCount = 2,
                Evaluate = Booth
            },
            SelectionOptions = new()
            {
                NumberOfElites = 1,
                NumberOfIndividuals = 30,
                SurvivabilityOptions = new()
                {
                    P=0.8,
                    Type = SurvivabilityType.Diverse
                },
                Type = SelectionType.Tournament
            },
        };
        
        engine.Run();
    }

    public static void RunTests()
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        ElitismTestRun();
        PopulationTestRun();
        GenerationTestRun();
        StepSizeTestRun();
        MutationTestRun();
        CrossoverTestRun();
        ConvergenceTestRun();
        SurvivabilityTestRun();
        SelectionTestRun();
        RastriginDimensionTestRun();
        watch.Stop();
        Console.WriteLine($"Tests finished in {watch.Elapsed.TotalSeconds} seconds!");
    }
    
    public static void ElitismTestRun()
    {
        Console.WriteLine("Running Elitism Test");
        foreach (Func<Chromosome, double> function in Functions)
        {
            for (int i = 0; i <= 30; i += 3)
            {
                GeneticEngine engine = new()
                {
                    GenerationCount = 100,
                    InitializationValue = -5,
                    CrossoverOptions = new()
                    {
                        NumberOfCrossovers = 25,
                        NumberOfCrossoverPoints = 1,
                        Type = SinglePoint,
                    },
                    MutationOptions = new()
                    {
                        NumberOfMutations = 30,
                        Type = MutationOptions.MutationType.SinglePoint
                    },
                    Problem = new()
                    {
                        ExpectedResult = 0,
                        StepSize = 1,
                        ValueRange = 10,
                        GenomeCount = 2,
                        Evaluate = function
                    },
                    SelectionOptions = new()
                    {
                        NumberOfElites = i,
                        NumberOfIndividuals = 100,
                        SurvivabilityOptions = new()
                        {
                            P = 0.8,
                            Type = SurvivabilityType.Diverse
                        },
                        Type = SelectionType.Tournament
                    },
                    DumpResult = true,
                    AppendMode = !(i == 0 && function == Functions[0]),
                    DumpFilePath = "out/data/Elitism.csv",
                    ProblemName = function.Method.Name
                };

                engine.Run();
            }
        }
    }
    
    public static void PopulationTestRun()
    {
        Console.WriteLine("Running Population Test");
        foreach (Func<Chromosome, double> function in Functions)
        {
            int[] populationSizes = [5, 10, 20, 50, 100];
            
            for (int i = 0; i < populationSizes.Length; i++)
            {
                int populationSize = populationSizes[i];
                GeneticEngine engine = new()
                {
                    GenerationCount = 100,
                    InitializationValue = -5,
                    CrossoverOptions = new()
                    {
                        NumberOfCrossovers = 25,
                        NumberOfCrossoverPoints = 1,
                        Type = SinglePoint,
                    },
                    MutationOptions = new()
                    {
                        NumberOfMutations = 30,
                        Type = MutationOptions.MutationType.SinglePoint
                    },
                    Problem = new()
                    {
                        ExpectedResult = 0,
                        StepSize = 1,
                        ValueRange = 10,
                        GenomeCount = 2,
                        Evaluate = function
                    },
                    SelectionOptions = new()
                    {
                        NumberOfElites = 1,
                        NumberOfIndividuals = populationSize,
                        SurvivabilityOptions = new()
                        {
                            P = 0.8,
                            Type = SurvivabilityType.Diverse
                        },
                        Type = SelectionType.Tournament
                    },
                    DumpResult = true,
                    AppendMode = !(i == 0 && function == Functions[0]),
                    DumpFilePath = "out/data/Population.csv",
                    ProblemName = function.Method.Name
                };

                engine.Run();
            }
        }
    }
    
    public static void GenerationTestRun()
    {
        Console.WriteLine("Running Generation Test");
        foreach (Func<Chromosome, double> function in Functions)
        {
            int[] generationCounts = [5, 10, 20, 50, 100];
            
            for (int i = 0; i < generationCounts.Length; i++)
            {
                int generationCount = generationCounts[i];
                GeneticEngine engine = new()
                {
                    GenerationCount = generationCount,
                    InitializationValue = -5,
                    CrossoverOptions = new()
                    {
                        NumberOfCrossovers = 25,
                        NumberOfCrossoverPoints = 1,
                        Type = SinglePoint,
                    },
                    MutationOptions = new()
                    {
                        NumberOfMutations = 30,
                        Type = MutationOptions.MutationType.SinglePoint
                    },
                    Problem = new()
                    {
                        ExpectedResult = 0,
                        StepSize = 1,
                        ValueRange = 10,
                        GenomeCount = 2,
                        Evaluate = function
                    },
                    SelectionOptions = new()
                    {
                        NumberOfElites = 1,
                        NumberOfIndividuals = 100,
                        SurvivabilityOptions = new()
                        {
                            P = 0.8,
                            Type = SurvivabilityType.Diverse
                        },
                        Type = SelectionType.Tournament
                    },
                    DumpResult = true,
                    AppendMode = !(i == 0 && function == Functions[0]),
                    DumpFilePath = "out/data/Generation.csv",
                    ProblemName = function.Method.Name
                };

                engine.Run();
            }
        }
    }
    
    public static void StepSizeTestRun()
    {
        Console.WriteLine("Running StepSize Test");
        foreach (Func<Chromosome, double> function in Functions)
        {
            double[] stepSizes = [0.1, 0.2, 0.5, 1, 1.5, 2];
            
            for (int i = 0; i < stepSizes.Length; i++)
            {
                double stepSize = stepSizes[i];
                GeneticEngine engine = new()
                {
                    GenerationCount = 100,
                    InitializationValue = -5,
                    CrossoverOptions = new()
                    {
                        NumberOfCrossovers = 25,
                        NumberOfCrossoverPoints = 1,
                        Type = SinglePoint,
                    },
                    MutationOptions = new()
                    {
                        NumberOfMutations = 30,
                        Type = MutationOptions.MutationType.SinglePoint
                    },
                    Problem = new()
                    {
                        ExpectedResult = 0,
                        StepSize = stepSize,
                        ValueRange = 10,
                        GenomeCount = 2,
                        Evaluate = function
                    },
                    SelectionOptions = new()
                    {
                        NumberOfElites = 1,
                        NumberOfIndividuals = 100,
                        SurvivabilityOptions = new()
                        {
                            P = 0.8,
                            Type = SurvivabilityType.Diverse
                        },
                        Type = SelectionType.Tournament
                    },
                    DumpResult = true,
                    AppendMode = !(i == 0 && function == Functions[0]),
                    DumpFilePath = "out/data/Stepsize.csv",
                    ProblemName = function.Method.Name
                };

                engine.Run();
            }
        }
    }

    public static void MutationTestRun()
    {
        Console.WriteLine("Running Mutation Test");
        foreach (Func<Chromosome, double> function in Functions)
        {
            int[] mutationCounts = [5, 10, 20, 50, 100];
            
            for (int i = 0; i < mutationCounts.Length; i++)
            {
                int mutationCount = mutationCounts[i];
                GeneticEngine engine = new()
                {
                    GenerationCount = 50,
                    InitializationValue = -5,
                    CrossoverOptions = new()
                    {
                        NumberOfCrossovers = 25,
                        NumberOfCrossoverPoints = 1,
                        Type = SinglePoint,
                    },
                    MutationOptions = new()
                    {
                        NumberOfMutations = mutationCount,
                        Type = MutationOptions.MutationType.SinglePoint
                    },
                    Problem = new()
                    {
                        ExpectedResult = 0,
                        StepSize = 1,
                        ValueRange = 10,
                        GenomeCount = 2,
                        Evaluate = function
                    },
                    SelectionOptions = new()
                    {
                        NumberOfElites = 1,
                        NumberOfIndividuals = 100,
                        SurvivabilityOptions = new()
                        {
                            P = 0.8,
                            Type = SurvivabilityType.Diverse
                        },
                        Type = SelectionType.Tournament
                    },
                    DumpResult = true,
                    AppendMode = !(i == 0 && function == Functions[0]),
                    DumpFilePath = "out/data/Mutation.csv",
                    ProblemName = function.Method.Name
                };

                engine.Run();
            }
        }
    }
    
    public static void CrossoverTestRun()
    {
        Console.WriteLine("Running Crossover Test");
        foreach (Func<Chromosome, double> function in Functions)
        {
            int[] crossoverCounts = [5, 10, 20, 50, 100];
            
            for (int i = 0; i < crossoverCounts.Length; i++)
            {
                int crossoverCount = crossoverCounts[i];
                GeneticEngine engine = new()
                {
                    GenerationCount = 50,
                    InitializationValue = -5,
                    CrossoverOptions = new()
                    {
                        NumberOfCrossovers = crossoverCount,
                        NumberOfCrossoverPoints = 1,
                        Type = SinglePoint,
                    },
                    MutationOptions = new()
                    {
                        NumberOfMutations = 30,
                        Type = MutationOptions.MutationType.SinglePoint
                    },
                    Problem = new()
                    {
                        ExpectedResult = 0,
                        StepSize = 1,
                        ValueRange = 10,
                        GenomeCount = 2,
                        Evaluate = function
                    },
                    SelectionOptions = new()
                    {
                        NumberOfElites = 1,
                        NumberOfIndividuals = 100,
                        SurvivabilityOptions = new()
                        {
                            P = 0.8,
                            Type = SurvivabilityType.Diverse
                        },
                        Type = SelectionType.Tournament
                    },
                    DumpResult = true,
                    AppendMode = !(i == 0 && function == Functions[0]),
                    DumpFilePath = "out/data/Crossover.csv",
                    ProblemName = function.Method.Name
                };

                engine.Run();
            }
        }
    }
    
    public static void ConvergenceTestRun()
    {
        
        foreach (Func<Chromosome, double> function in Functions)
        {
            string convergenceName = $"{function.Method.Name}Convergence";
            Console.WriteLine($"Running {convergenceName} Test");
            GeneticEngine engine = new()
            {
                GenerationCount = 200,
                InitializationValue = -5,
                CrossoverOptions = new()
                {
                    NumberOfCrossovers = 50,
                    NumberOfCrossoverPoints = 1,
                    Type = SinglePoint,
                },
                MutationOptions = new()
                {
                    NumberOfMutations = 50,
                    Type = MutationOptions.MutationType.SinglePoint
                },
                Problem = new()
                {
                    ExpectedResult = 0,
                    StepSize = 0.8,
                    ValueRange = 10,
                    GenomeCount = 2,
                    Evaluate = function
                },
                SelectionOptions = new()
                {
                    NumberOfElites = 1,
                    NumberOfIndividuals = 100,
                    SurvivabilityOptions = new()
                    {
                        P = 0.8,
                        Type = SurvivabilityType.Ranked
                    },
                    Type = SelectionType.RouletteWheel
                },
                DumpResult = true,
                DumpEverything = true,
                DumpFilePath = $"out/data/{convergenceName}.csv",
                ProblemName = function.Method.Name
            };

            engine.Run();

        }
    }

    public static void SurvivabilityTestRun()
    {
        Console.WriteLine("Running Survivability Test");
        foreach (Func<Chromosome, double> function in Functions)
        {
            SurvivabilityType[] types =
            [
                SurvivabilityType.Relative,
                SurvivabilityType.Ranked,
                SurvivabilityType.Diverse
            ];
            for (int i = 0; i < types.Length; i++)
            {
                SurvivabilityType type = types[i];
                GeneticEngine engine = new()
                {
                    GenerationCount = 50,
                    InitializationValue = -5,
                    CrossoverOptions = new()
                    {
                        NumberOfCrossovers = 25,
                        NumberOfCrossoverPoints = 1,
                        Type = SinglePoint,
                    },
                    MutationOptions = new()
                    {
                        NumberOfMutations = 30,
                        Type = MutationOptions.MutationType.SinglePoint
                    },
                    Problem = new()
                    {
                        ExpectedResult = 0,
                        StepSize = 1,
                        ValueRange = 10,
                        GenomeCount = 2,
                        Evaluate = function
                    },
                    SelectionOptions = new()
                    {
                        NumberOfElites = 0,
                        NumberOfIndividuals = 100,
                        SurvivabilityOptions = new()
                        {
                            P = 0.8,
                            Type = type
                        },
                        Type = SelectionType.Tournament
                    },
                    DumpResult = true,
                    AppendMode = !(i == 0 && function == Functions[0]),
                    DumpFilePath = "out/data/Survivability.csv",
                    ProblemName = function.Method.Name
                };

                engine.Run();
            }
        }
    }

    public static void SelectionTestRun()
    {
        Console.WriteLine("Running Selection Test");
        foreach (Func<Chromosome, double> function in Functions)
        {
            SelectionType[] types =
            [
                SelectionType.Tournament,
                SelectionType.RouletteWheel
            ];
            for (int i = 0; i < types.Length; i++)
            {
                SelectionType type = types[i];
                GeneticEngine engine = new()
                {
                    GenerationCount = 50,
                    InitializationValue = -5,
                    CrossoverOptions = new()
                    {
                        NumberOfCrossovers = 25,
                        NumberOfCrossoverPoints = 1,
                        Type = SinglePoint,
                    },
                    MutationOptions = new()
                    {
                        NumberOfMutations = 30,
                        Type = MutationOptions.MutationType.SinglePoint
                    },
                    Problem = new()
                    {
                        ExpectedResult = 0,
                        StepSize = 1,
                        ValueRange = 10,
                        GenomeCount = 2,
                        Evaluate = function
                    },
                    SelectionOptions = new()
                    {
                        NumberOfElites = 0,
                        NumberOfIndividuals = 100,
                        SurvivabilityOptions = new()
                        {
                            P = 0.8,
                            Type = SurvivabilityType.Diverse
                        },
                        Type = type
                    },
                    DumpResult = true,
                    AppendMode = !(i == 0 && function == Functions[0]),
                    DumpFilePath = "out/data/Selection.csv",
                    ProblemName = function.Method.Name
                };

                engine.Run();
            }
        }
    }

    public static void RastriginDimensionTestRun()
    {
        Console.WriteLine("Running Dimension Test");
        int[] dimensions = [2, 3, 4, 5, 10, 100];

        CrossoverOptions.CrossoverType[] types =
        {
            SinglePoint,
            MultiPoint, // For two points
            MultiPoint,
            PathRelinking,
        };
        
        for (int i = 0; i < dimensions.Length; i++)
        {
            int dimension = dimensions[i];
            for (int j = 0; j < types.Length; j++)
            {
                CrossoverOptions.CrossoverType type = types[j];
                int crossoverPoints = 1;

                if (j == 1)
                {
                    crossoverPoints = 2;
                }
                else if (j == 2)
                {
                    crossoverPoints = int.Max(dimension / 2, 2);
                }
                
                GeneticEngine engine = new()
                {
                    GenerationCount = 100,
                    InitializationValue = -5,
                    CrossoverOptions = new()
                    {
                        NumberOfCrossovers = 25,
                        NumberOfCrossoverPoints = crossoverPoints,
                        Type = type,
                    },
                    MutationOptions = new()
                    {
                        NumberOfMutations = 30,
                        Type = MutationOptions.MutationType.SinglePoint
                    },
                    Problem = new()
                    {
                        ExpectedResult = 0,
                        StepSize = 1,
                        ValueRange = 5.12,
                        GenomeCount = dimension,
                        Evaluate = Functions[0]
                    },
                    SelectionOptions = new()
                    {
                        NumberOfElites = 1,
                        NumberOfIndividuals = 100,
                        SurvivabilityOptions = new()
                        {
                            P = 0.8,
                            Type = SurvivabilityType.Relative
                        },
                        Type = SelectionType.RouletteWheel
                    },
                    DumpResult = true,
                    AppendMode = !(i == 0 && type == types[0]),
                    DumpFilePath = "out/data/RastriginDimension.csv",
                    ProblemName = Functions[0].Method.Name
                };

                engine.Run();
            }
        }

    }

}