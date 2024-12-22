namespace TSP;

public static class Mutation
{
    private static readonly Random _random = new Random();
    public static Route Mutate(Route parent)
    {
        Route child = parent.Copy;
        
        int idx1 = _random.Next(0, parent.Size);
        int idx2 = _random.Next(0, parent.Size);
        while (idx1 == idx2) idx2 = _random.Next(0, parent.Size);
        
        (child[idx1], child[idx2]) = (child[idx1], child[idx2]);
        
        return child;
    }
    
}