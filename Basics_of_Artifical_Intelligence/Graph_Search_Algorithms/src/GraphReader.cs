
class GraphReader
{
    string Path;
    public List<Graph> Read()
    {
        List<Graph> Graphs = new List<Graph>();

        StreamReader reader = new StreamReader(Path);
        bool edgeMode = true;
        bool heuristicMode = true;
        int currentID = -1;

        while (!reader.EndOfStream)
        {
            string? line = reader.ReadLine();
            if (line == null) throw new Exception("There was an error reading the file!");

            if (String.IsNullOrEmpty(line)) { edgeMode = false; heuristicMode = false; continue; }
            if (line[0] == '#') continue;

            if (line.StartsWith("graph"))
            {
                currentID = currentID + 1;
                edgeMode = false;
                heuristicMode = false;
                Graphs.Add(new(line.Split(' ')[1]));
                continue;
            }



            if (line.StartsWith("nodes"))
            {
                string[] elements = line.Split();
                for (int i = 1; i < elements.Length; i++)
                {
                    Graphs[currentID].Nodes.Add(new(elements[i]));
                }
            }
            if (line.Equals("edges")) { edgeMode = true; continue; }
            if (line.Equals("heuristic-start"))
            {
                edgeMode = false;
                heuristicMode = true;
                continue;
            }

            if (line.Equals("heuristic-end"))
            {
                heuristicMode = false;
                continue;
            }

            if (edgeMode)
            {
                string[] elements = line.Split(' ');

                int nodeID = -1;
                int adjacentID = -1;

                for (int i = 0; i < Graphs[currentID].Nodes.Count; i++)
                {
                    Node node = Graphs[currentID].Nodes[i];
                    if (node.Name == elements[0]) { nodeID = i; if (adjacentID != -1) break; }
                    if (node.Name == elements[1]) { adjacentID = i; if (nodeID != -1) break; }
                }
                float cost = 0;
                if (elements.Length == 3) cost = float.Parse(elements[2]);

                Node a = Graphs[currentID].Nodes[nodeID];
                Node b = Graphs[currentID].Nodes[adjacentID];
                a.Edges.Add(new(b){Cost = cost});
                b.Edges.Add(new(a){Cost = cost});

                continue;
            }

            if (heuristicMode)
            {
                string[] elements = line.Split(' ');
                List<float> costs = new List<float>();
                List<string> names = new List<string>();

                for (int i = 1; i < elements.Length; i++)
                {
                    names.Add(elements[i].Split('-')[0]);
                    costs.Add(float.Parse(elements[i].Split('-')[1]));
                }
                Node? a = null;
                for (int i = 0; i < Graphs[currentID].Nodes.Count; i++)
                {
                    Node node = Graphs[currentID].Nodes[i];
                    if (node.Name == elements[0]) { a = node; break; }
                }

                if (a == null) throw new Exception($"Graph '{Graphs[currentID].Name}' does not contain a node named {elements[0]}");

                for (int nameID = 0; nameID < names.Count; nameID++)
                {
                    string name = names[nameID];
                    Node? b = Graphs[currentID].SearchNodeByName(name);
                    if (b == null) throw new Exception($"Graph '{Graphs[currentID].Name}' does not contain a node named {name}");

                    if (b.AdjacentTo(a))
                    {
                        for (int i = 0; i < b.Edges.Count; i++)
                        {
                            if (b.Edges[i]._node == a)
                            {
                                b.Edges[i].Heuristic = costs[nameID];
                                break;
                            }
                        }
                    }
                    else
                    {
                        b.Edges.Add(new(a){Heuristic = costs[nameID]});
                    }
                }
            }
        }
        return Graphs;
    }

    public GraphReader(string _path) { Path = _path; }
}