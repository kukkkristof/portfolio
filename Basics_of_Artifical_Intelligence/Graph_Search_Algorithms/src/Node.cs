class Node(string name)
{
    public string Name = name;
    public List<Edge> Edges = new List<Edge>();

    public bool AdjacentTo(Node _node)
    {
        foreach(Edge edge in Edges) if(edge._node == _node) return true;

        return false;
    }

    public float HeuristicToTarget(Node _target)
    {
        if(Name == _target.Name) return 0;
        for (int i = 0; i < Edges.Count; i++)
        {
            if (Edges[i]._node == _target)
            {
                return Edges[i].Heuristic == -1 ? 0 : Edges[i].Heuristic;
            }
        }
        return float.MaxValue;
    }

    public override string ToString()
    {
        return Name;
    }

}