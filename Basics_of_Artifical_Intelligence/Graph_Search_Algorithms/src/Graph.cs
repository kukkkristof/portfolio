class Graph(string name)
{
    public string Name = name;
    public List<Node> Nodes = new List<Node>();

    public Node? SearchNodeByName(string name)
    {
        foreach (Node node in Nodes) { if (node.Name == name) return node; }
        return null;
    }

    public override string ToString()
    {
        return Name;
    }

}