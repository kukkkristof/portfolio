namespace TSP;

public class City(double[] coords)
{
    public double[] Coordinates { get; set; } = coords;

    public double this[int index]
    {
        get => Coordinates[index];
        set => Coordinates[index] = value;
    }
    
}