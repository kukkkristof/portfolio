namespace GeneticForFunctions;

public static class Crossover
{
    public static Chromosome Cross(Chromosome parent1, Chromosome parent2, CrossoverOptions options, Problem problem)
    {

        switch (options.Type)
        {
            case CrossoverOptions.CrossoverType.SinglePoint:
                return SinglePoint(parent1, parent2, problem);
            case CrossoverOptions.CrossoverType.MultiPoint:
                return MultiPoint(parent1, parent2, options, problem);
            case CrossoverOptions.CrossoverType.Uniform:
                return Uniform(parent1, parent2, problem);
            case CrossoverOptions.CrossoverType.PathRelinking:
                return PathRelinking(parent1, parent2, problem);
        }
        
        throw new NotImplementedException();
    }

    private static Chromosome SinglePoint(Chromosome parent1, Chromosome parent2, Problem problem)
    {
        int crossoverPoint = Helper.Randomizer.Next(0, parent1.Size);
        
        Chromosome child = new()
        {
            Values = parent1.Values[..crossoverPoint].Concat(parent2.Values[crossoverPoint..]).ToArray()
        };
        child.Value = problem.Evaluate(child);
        child.Fitness = problem.GetFitness(child);
        return child;
    }

    private static Chromosome MultiPoint(Chromosome parent1, Chromosome parent2, CrossoverOptions options, Problem problem)
    {
        if(options.NumberOfCrossoverPoints > parent1.Size) throw new Exception("Number of crossover points must be less than size of parent1");
        
        List<int> indices = Enumerable.Range(0, options.NumberOfCrossoverPoints).ToList();
        List<int> cuts = [];

        for (int i = 0; i < options.NumberOfCrossoverPoints; i++)
        {
            int idx = Helper.Randomizer.Next(0, indices.Count);
            cuts.Add(indices[idx]);
            indices.RemoveAt(idx);
        }
        
        Chromosome child = parent1.Copy;

        bool useParent2 = false;
        
        for (int i = 0; i < child.Size; i++)
        {
            if(cuts.Contains(i)) useParent2 = !useParent2;

            if (useParent2) child[i] = parent2[i];
        }

        child.Value = problem.Evaluate(child);
        child.Fitness = problem.GetFitness(child);
        
        return child;
    }

    private static Chromosome Uniform(Chromosome parent1, Chromosome parent2, Problem problem)
    {
        Chromosome child = parent1.Copy;

        for (int i = 0; i < child.Size; i++)
        {
            if (Helper.Randomizer.Next(0, 2) == 1)
            {
                child[i] = parent2[i];
            }
        }
        
        child.Value = problem.Evaluate(child);
        child.Fitness = problem.GetFitness(child);

        return child;
    }

    private static Chromosome PathRelinking(Chromosome parent1, Chromosome parent2, Problem problem)
    {
        double sum = 0;
        for(int i = 0; i < parent1.Size; i++)
        {
            sum += Math.Pow(parent1[i] - parent2[i], 2);
        }
        int cutNumbers = (int)Math.Min(1, Math.Round(Math.Sqrt(sum)));

        double[] cuts = new double[cutNumbers];

        for(int cut = 0; cut < cutNumbers; cut++)
        {
            cuts[cut] = 1.0f / (cutNumbers + 1);
        }
        
        List<Chromosome> children = [parent1, parent2];
        for (int cut = 0; cut < cutNumbers; cut++)
        {
            children.Add(LinearInterpolation(parent1, parent2, cuts[cut], problem));
        }

        int bestIDx = 0;
        for(int i = 1; i < children.Count; i++)
        {
            if (children[i].Fitness > children[bestIDx].Fitness)
            {
                bestIDx = i;
            }
        }

        return children[bestIDx].Copy;
    }
    
    private static Chromosome LinearInterpolation(Chromosome parent1, Chromosome parent2, double t, Problem problem)
    {
        Chromosome child = new(){Values = new double[parent1.Size]};
        for(int i = 0; i < parent1.Size; i++)
        {
            child[i] = parent1[i] * (1 - t) + parent2[i] * t;
        }
        
        child.Value = problem.Evaluate(child);
        child.Fitness = problem.GetFitness(child);
        
        return child;
    }
    
}