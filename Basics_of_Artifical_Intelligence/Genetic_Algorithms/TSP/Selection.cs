namespace TSP;

public static class Selection
{
    public static Routes Select(Routes startingRoutes, SelectionOptions options)
    {

        switch (options.Type)
        {
            case SelectionOptions.SelectionType.RouletteWheel:
                return RouletteWheel(startingRoutes, options);
            case SelectionOptions.SelectionType.Tournament:
                return Tournament(startingRoutes, options);
        }
        
        throw new NotImplementedException();
    }

    private static Routes Tournament(Routes startingRoutes,
        SelectionOptions options)
    {
        Routes newRoutes = new()
        {
            RouteList = startingRoutes.RouteList.GetRange(0, options.NumberOfElites)
        };

        for (int i = options.NumberOfElites; i < options.NumberOfRoutes; i++)
        {
            Route participant1 = startingRoutes.Random;
            Route participant2 = startingRoutes.Random;

            newRoutes.RouteList.Add(
                participant1.Fitness >= participant2.Fitness ? participant1 : participant2
            );
        }

        return newRoutes;
    }
    
    private static Routes RouletteWheel(Routes startingRoutes,
        SelectionOptions options)
    {

        startingRoutes.Sort();
        
        Routes newRoutes = new Routes
        {
            RouteList = startingRoutes.RouteList.GetRange(0, options.NumberOfElites)
        };

        double[] chances = Survivability.Chances(startingRoutes, options.SurvivabilityOptions);
        for (int i = options.NumberOfElites; i < options.NumberOfRoutes; i++)
        {
            double randomChance = Randomizer.Next(0.0, 1.0);
            double accumulativeChance = 0;

            for (int j = 0; j < chances.Length; j++)
            {
                accumulativeChance += chances[j];
                if (accumulativeChance >= randomChance)
                {
                    newRoutes.RouteList.Add(startingRoutes.RouteList[j]);
                    break;
                }
            }
            
        }
        
        return newRoutes;
    }
}
