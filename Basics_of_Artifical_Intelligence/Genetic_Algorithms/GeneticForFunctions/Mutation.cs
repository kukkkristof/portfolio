namespace GeneticForFunctions;

public static class Mutation
{
    public static Chromosome Mutate(Chromosome chromosome, MutationOptions options, Problem problem)
    {

        switch (options.Type)
        {
            case MutationOptions.MutationType.SinglePoint:
                return Single(chromosome, problem);
                
        }
        
        throw new NotImplementedException();
    }

    private static Chromosome Single(Chromosome chromosome, Problem problem)
    {
        Chromosome child = chromosome.Copy;

        int mutationPoint = Helper.Randomizer.Next(0, child.Size);

        child[mutationPoint] = Math.Clamp(
            child[mutationPoint] + Helper.Randomizer.Next(-problem.StepSize, problem.StepSize),
            -problem.ValueRange,
            problem.ValueRange
        );

        child.Value = problem.Evaluate(child);
        child.Fitness = problem.GetFitness(child);
        
        return child;
    }
}