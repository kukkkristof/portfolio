namespace GeneticForFunctions;

public static class ProblemFunctions
{
    public static double Rastrigin(Chromosome chromosome)
    {
        {
            double a = 10;
            double n = chromosome.Size;
            double result = a * n;
            for (int i = 0; i < chromosome.Size; i++)
            {
                double xi = chromosome[i];
                result += xi * xi - a * Math.Cos(2 * Math.PI * xi);
            }

            return result;
        }
    }

    public static double Booth(Chromosome chromosome)
    {
        double x = chromosome[0];
        double y = chromosome[1];

        return Math.Pow(x + 2 * y - 7, 2) + Math.Pow(2 * x + y - 5, 2);
    }

    public static double Levi(Chromosome chromosome)
    {
        double x = chromosome[0];
        double y = chromosome[1];

        double segment1 = Math.Pow(Math.Sin(3 * double.Pi * x), 2);
        double segment2 = (x-1)*(x-1)*Math.Pow(Math.Sin(3 * double.Pi * y), 2);
        double segment3 = (y-1)*(y-1)*Math.Pow(1 + Math.Sin(2 * double.Pi * y), 2);
    
        return segment1 + segment2 + segment3;
    }
}