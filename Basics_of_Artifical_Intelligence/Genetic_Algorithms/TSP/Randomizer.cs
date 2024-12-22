namespace TSP;

public static class Randomizer
{
    private static readonly Random Random = new Random();

    public static int Next(int min, int max)
    {
        return Random.Next(min, max);
    }
    
    public static double Next(double min, double max)
    {
        return Random.NextDouble() * (max - min) + min;
    }
    
}