namespace TSP;

public static class Survivability
{
    public static double[] Chances(Routes population, SurvivabilityOptions options)
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

    private static double[] Relative(Routes routes)
    {
        double totalFitness = routes.RouteList.Select(x => x.Fitness).Sum();
        return routes.RouteList.Select(x => x.Fitness / totalFitness ).ToArray();
    }

    private static double[] Ranked(Routes routes)
    {
        double sum = Enumerable.Range(1, routes.RouteList.Count).Sum();
        return Enumerable.Range(1, routes.RouteList.Count).OrderDescending().Select(x => x / sum).ToArray();
    }
    
    private static double[] Diverse(Routes routes, SurvivabilityOptions options)
    {
        double average = routes.RouteList.Select(x => x.Fitness).Average();
        double[] differences = routes.RouteList.Select(x => Math.Abs(x.Fitness - average)).ToArray();

        double p = options.P;
        
        List<int> indices = Enumerable.Range(0, routes.RouteList.Count).ToList();
        indices.Sort((x,y) => differences[y].CompareTo(differences[x]));


        double[] chances = indices.Select(x =>
            Math.Pow(1 - p, x) * (x==indices.Max()?1:p)
        ).ToArray();
        
        return chances;

    }
    
}