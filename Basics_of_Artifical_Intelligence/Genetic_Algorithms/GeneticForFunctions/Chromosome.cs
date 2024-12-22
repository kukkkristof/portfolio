namespace GeneticForFunctions;

public class Chromosome
{
    public double[] Values { get; init; } = [0];
    public double Fitness { get; set; }
    public double Value { get; set; }
    
    public int Size => Values.Length;
    
    public Chromosome Copy => new(){Values = this.Values[..], Fitness = this.Fitness, Value = this.Value};

    public double this[int index]
    {
        get => Values[index];
        set => Values[index] = value;
    }

    public override string ToString()
    {
        return "[" + string.Join(" ", Values.Select(x => $"{(x > 0? "+":"")}{x:0.000}")) + "] | "
               + Fitness.ToString("0.000") + " | "
               + Value.ToString("0.000");
    }
}