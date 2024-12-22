using System.Security.Cryptography;

namespace TSP;

public class Problem
{
    public required List<City> Cities { get; init; }

    public int CityCount => Cities.Count;
    
    public double Distance(City a, City b)
    {
        double distance = 0;
        for (int i = 0; i < a.Coordinates.Length; i++)
        {
            distance += Math.Pow(a.Coordinates[i] - b.Coordinates[i], 2);
        }
        
        return Math.Sqrt(distance);
    }

    public double GetValue(Route route)
    {
        double distance = 
            Distance(Cities[route[0]],
                Cities[route[route.Size-1]]);

        for (int i = 1; i < route.Size; i++)
        {
            distance += Distance(Cities[route[i - 1]], Cities[route[i]]);
        }

        return distance;
    }
    
    public double GetFitness(Route route)
    {

        return 1.0 / (1 + GetValue(route));

    }

    public static List<City> GenerateRandomCities(
        int numberOfCities,
        int dimensions,
        double minimum,
        double maximum) {
        
        List<City> cities = new List<City>();
        for (int _ = 0; _ < numberOfCities; _++)
        {
            City city = new(new double[dimensions]);
            for (int dim = 0; dim < dimensions; dim++)
            {
                city[dim] = Randomizer.Next(minimum, maximum);
            }
            cities.Add(city);
        }
        return cities;
    }
}