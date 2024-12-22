namespace TSP;

public class Route
{
    public int[] Indices { get; set; }

    public int Size => Indices.Length;

    public double Fitness { get; set; }
    public double Value { get; set; }
    
    public Route Copy => new() { Indices = Indices[..], Fitness = this.Fitness };

    public int this[int index]
    {
        get => Indices[index];
        set => Indices[index] = value;
    }

    public override string ToString()
    {
        return $"[{string.Join(", ", Indices)}] : {Fitness} / {Value}";
    }
}