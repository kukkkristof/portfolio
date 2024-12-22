namespace TSP;

public static class Crossover
{
    public static Route Cross(Route parent1, Route parent2)
    {
        Route child = parent1.Copy;
        int size = Randomizer.Next(1, parent1.Size-1);
        int indexOfParent2 = Randomizer.Next(0, parent2.Size-size);
        
        int[] segment = parent2.Indices.ToList().GetRange(indexOfParent2, size).ToArray();
        int startOfChild = Randomizer.Next(0, parent1.Size - segment.Length);

        for (int i = 0; i < segment.Length; i++)
        {
            int childIndex = child.Indices.ToList().IndexOf(segment[i]);
            (child[startOfChild + i], child[childIndex]) = (child[childIndex], child[startOfChild + i]);
        }
        
        return child;
    }
}