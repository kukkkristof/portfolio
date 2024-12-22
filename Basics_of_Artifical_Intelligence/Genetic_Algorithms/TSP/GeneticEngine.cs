namespace TSP;

public class GeneticEngine
{
    public required Problem Problem { get; init; }

    public required int GenerationCount { get; init; }
    public required int MutationCount { get; init; }
    public required int CrossoverCount { get; init; }
    
    public required SelectionOptions SelectionOptions { get; init; }
    
    private Routes _routes;
    
    public void Run()
    {

        Init();
        
        for (int generation = 0; generation < GenerationCount; generation++)
        {
            for (int mutation = 0; mutation < MutationCount; mutation++)
            {
                _routes.RouteList.Add(Mutation.Mutate(_routes.Random));
                _routes.RouteList[^1].Fitness
                    = Problem.GetFitness(_routes.RouteList[^1]);
                _routes.RouteList[^1].Value
                    = Problem.GetValue(_routes.RouteList[^1]);
            }
            
            for (int crossover = 0; crossover < CrossoverCount; crossover++)
            {
                _routes.RouteList.Add(Crossover.Cross(_routes.Random, _routes.Random));
                _routes.RouteList[^1].Fitness
                    = Problem.GetFitness(_routes.RouteList[^1]);
                _routes.RouteList[^1].Value
                    = Problem.GetValue(_routes.RouteList[^1]);
            }
            
            _routes = Selection.Select(_routes, SelectionOptions);
        }

        Console.WriteLine(_routes.Best);
        
    }

    private void Init()
    {
        _routes = new Routes();
        
        for (int individual = 0; individual < SelectionOptions.NumberOfRoutes; individual++)
        {
            List<int> indices = Enumerable.Range(0, Problem.CityCount).ToList();

            Route newRoute = new()
            {
                Indices = new int[Problem.CityCount]
            };

            for (int i = 0; i < Problem.CityCount; i++)
            {
                int randomIndex = Randomizer.Next(0, indices.Count);
                newRoute.Indices[i] = indices[randomIndex];
                indices.RemoveAt(randomIndex);
            }

            newRoute.Fitness = Problem.GetFitness(newRoute);
            newRoute.Value = Problem.GetValue(newRoute);
            
            _routes.RouteList.Add(newRoute);
            
        }
    }
    
}