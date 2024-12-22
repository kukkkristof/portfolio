namespace GeneticForFunctions;

public static class Survivability
{
    public static double[] Chances(Population population, SurvivabilityOptions options)
    {

        switch (options.Type)
        {
            case SurvivabilityOptions.SurvivabilityType.Relative:
                return Relative(population);
            case SurvivabilityOptions.SurvivabilityType.Ranked:
                return Ranked(population);
            case SurvivabilityOptions.SurvivabilityType.Diverse:
                return Diverse(population, options);
        }
        
        throw new NotImplementedException();
    }

    private static double[] Relative(Population population)
    {
        double totalFitness = population.Individuals.Select(x => x.Fitness).Sum();
        return population.Individuals.Select(x => x.Fitness / totalFitness ).ToArray();
    }

    private static double[] Ranked(Population population)
    {
        double sum = Enumerable.Range(1, population.Individuals.Count).Sum();
        return Enumerable.Range(1, population.Individuals.Count).OrderDescending().Select(x => x / sum).ToArray();
    }
    
    private static double[] Diverse(Population population, SurvivabilityOptions options)
    {
        double average = population.Individuals.Select(x => x.Fitness).Average();
        double[] differences = population.Individuals.Select(x => Math.Abs(x.Fitness - average)).ToArray();

        double p = options.P;
        
        List<int> indices = Enumerable.Range(0, population.Individuals.Count).ToList();
        indices.Sort((x,y) => differences[y].CompareTo(differences[x]));


        double[] chances = indices.Select(x =>
            Math.Pow(1 - p, x) * (x==indices.Max()?1:p)
        ).ToArray();
        
        return chances;

    }
    
}