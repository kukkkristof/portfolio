class Edge(Node __node)
{
    public Node _node = __node;
    private float? _cost;
    private float? _heuristicCost;

    public float Cost
    {
        get { return _cost ?? -1; }
        set { _cost = value; }
    }

    public float Heuristic
    {
        get { return _heuristicCost ?? -1; }
        set { _heuristicCost = value; }
    }

}