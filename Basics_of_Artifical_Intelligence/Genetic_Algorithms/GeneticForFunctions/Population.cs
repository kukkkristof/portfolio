namespace GeneticForFunctions;
                                      
public class Population               
{
    public List<Chromosome> Individuals { get; set; } = [];

    public Chromosome Best
    {
        get
        {
            if(Individuals == null) throw new Exception("The are no individuals");
            int bestIndex = 0;
            for (int i = 1; i < Individuals.Count; i++)
            {
                if (Individuals[i].Fitness > Individuals[bestIndex].Fitness)
                {
                    bestIndex = i;
                }
            }
            return Individuals[bestIndex];
        }
    }
    
    public Chromosome Worst
    {
        get
        {
            if(Individuals == null) throw new Exception("The are no individuals");
            int worstIndex = 0;
            for (int i = 1; i < Individuals.Count; i++)
            {
                if (Individuals[i].Fitness < Individuals[worstIndex].Fitness)
                {
                    worstIndex = i;
                }
            }
            return Individuals[worstIndex];
        }
    }
    
    public double Average
    {
        get
        {
            return Individuals.Average(x => x.Fitness);
        }
    }

    public double Deviation
    {
        get
        {
            double average = Average;
            double sum = Individuals.Sum(x => Math.Pow(x.Fitness - average, 2));
            double result = Math.Sqrt( sum / Individuals.Count - 1 );
            return double.IsNaN(result) ? 0 : result;
        }
    }
    

    public Chromosome Random => Individuals[Helper.Randomizer.Next(0, Individuals.Count)];

    public void Sort()
    {
        Individuals = Individuals.OrderByDescending(x => x.Fitness).ToList();
    }

    public override string ToString()
    {
        return string.Join("\n", Individuals);
    }
    
}