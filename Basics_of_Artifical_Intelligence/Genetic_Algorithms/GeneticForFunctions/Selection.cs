namespace GeneticForFunctions;

public static class Selection
{
    public static Population Select(Population startingPopulation, SelectionOptions options)
    {

        switch (options.Type)
        {
            case SelectionOptions.SelectionType.RouletteWheel:
                return RouletteWheel(startingPopulation, options);
            case SelectionOptions.SelectionType.Tournament:
                return Tournament(startingPopulation, options);
        }
        
        throw new NotImplementedException();
    }

    private static Population Tournament(Population startingPopulation,
        SelectionOptions options)
    {
        Population newPopulation = new()
        {
            Individuals = startingPopulation.Individuals.GetRange(0, options.NumberOfElites)
        };

        for (int i = options.NumberOfElites; i < options.NumberOfIndividuals; i++)
        {
            Chromosome participant1 = startingPopulation.Random;
            Chromosome participant2 = startingPopulation.Random;

            newPopulation.Individuals.Add(
                participant1.Fitness >= participant2.Fitness ? participant1 : participant2
            );
        }

        return newPopulation;
    }
    
    private static Population RouletteWheel(Population startingPopulation,
        SelectionOptions options)
    {

        startingPopulation.Sort();
        
        Population newPopulation = new Population
        {
            Individuals = startingPopulation.Individuals.GetRange(0, options.NumberOfElites)
        };

        double[] chances = Survivability.Chances(startingPopulation, options.SurvivabilityOptions);
        for (int i = options.NumberOfElites; i < options.NumberOfIndividuals; i++)
        {
            double randomChance = Helper.Randomizer.Next(0.0, 1.0);
            double accumulativeChance = 0;

            for (int j = 0; j < chances.Length; j++)
            {
                accumulativeChance += chances[j];
                if (accumulativeChance >= randomChance)
                {
                    newPopulation.Individuals.Add(startingPopulation.Individuals[j]);
                    break;
                }
            }
            
        }
        
        return newPopulation;
    }
}
