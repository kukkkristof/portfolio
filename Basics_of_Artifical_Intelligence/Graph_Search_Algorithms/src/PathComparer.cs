public enum ComparerType
{
    None = 0,
    CostComparer = 1,
    HeuristicComparer = 2,
    HeuristicAndCostComparer = 3
}

class PathCostComparer : IComparer<Path>
{
    public int Compare(Path? x, Path? y)
    {

        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        float first_cost = x.GetTotalCost();
        float second_cost = y.GetTotalCost();

        return first_cost.CompareTo(second_cost);
    }
}

class PathHeuristicComparer : IComparer<Path>
{
    public Node Target { get; set; }

    public PathHeuristicComparer(Node _target)
    {
        Target = _target;
    }

    public int Compare(Path? x, Path? y)
    {

        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        float first_heuristic = x.GetLast().HeuristicToTarget(Target);
        float second_heuristic = y.GetLast().HeuristicToTarget(Target);

        return first_heuristic.CompareTo(second_heuristic);
    }
}



class PathHeuristicAndCostComparer : IComparer<Path>
{

    public Node Target {get; set;}

    public PathHeuristicAndCostComparer(Node _Target)
    {
        Target = _Target;
    }

    public int Compare(Path? x, Path? y)
    {

        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        float first_heuristic = x.GetLast().HeuristicToTarget(Target);
        float second_heuristic = y.GetLast().HeuristicToTarget(Target);

        float first_cost = x.GetTotalCost();
        float second_cost = y.GetTotalCost();

        float x_value = first_heuristic  + first_cost;
        float y_value = second_heuristic + second_cost;

        return x_value.CompareTo(y_value);

    }
}