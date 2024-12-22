namespace GeneticForFunctions;

public static class Helper
{
    private static readonly Random Random = new Random();

    public static class Randomizer
    {
        public static double Next(double min, double max)
        {
            return Random.NextDouble() * (max - min) + min;
        }
        public static int Next(int min, int max)
        {
            return Random.Next(min, max);
        }
        
    }
}