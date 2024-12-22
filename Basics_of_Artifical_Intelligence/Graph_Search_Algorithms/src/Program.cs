using System.Diagnostics;

List<Graph> Graphs = new GraphReader("../graphs.txt").Read();

Console.Clear();



//TimeAll(Graphs[0], 0, 11);
// TimeAll(Graphs[1], 0, 3);
// TimeAll(Graphs[2], 0, 3);
// TimeAll(Graphs[3], 0, 7);
 TimeAll(Graphs[4], 0, 5);
// TimeAll(Graphs[5], 0, 4);
// TimeAll(Graphs[6], 0, 5);


void TimeAlgorithm(PathSearchAlgorithm _algorithm)
{
    Stopwatch _algorithmTime = Stopwatch.StartNew();

    _algorithm.Execute();

    _algorithmTime.Stop();
    _algorithm.Runtime = _algorithmTime.Elapsed;

    Console.WriteLine(_algorithm);
}

void TimeAll(Graph _graph, int startIndex, int endIndex)
{
    Node startNode = _graph.Nodes[startIndex];
    Node endNode = _graph.Nodes[endIndex];

    PathSearchAlgorithm.StartNode = startNode;
    PathSearchAlgorithm.EndNode = endNode;

    Warmup();

    string routeString = $"{startNode} -> {endNode}";

    Console.WriteLine($"{_graph}".PadRight(80- routeString.Length, ' ')
                     +$"{routeString}\n"
                     +"\n".PadLeft(81, '_')
                     +"Algorithm".PadRight(20, ' ')
                     +"Result".PadRight(20, ' ')
                     +"Exec. time (ms)".PadRight(20, ' ')
                     +"Extension count\n"
                     +"".PadRight(80, '_'));


    TimeAlgorithm(SearchPreset.DFS_PRESET);
    TimeAlgorithm(SearchPreset.BFS_PRESET);
    TimeAlgorithm(SearchPreset.HILL_CLIMBING_PRESET);
    TimeAlgorithm(SearchPreset.BEAM_PRESET);
    TimeAlgorithm(SearchPreset.BEST_FIRST_PRESET);
    TimeAlgorithm(SearchPreset.BRANCH_AND_BOUND_PRESET);
    TimeAlgorithm(SearchPreset.BRANCH_AND_BOUND_EXT_LIST_PRESET);
    TimeAlgorithm(SearchPreset.BRANCH_AND_BOUND_HEUR_PRESET);
    TimeAlgorithm(SearchPreset.A_STAR_PRESET);
    Console.WriteLine("\n\n");
}

/*
This warmup sequence is used for JIT runtime optimization.

When building the app, these can be discarded.

However for testing purposes, it is neccesarry to get
accurate measuerements for runtime!
*/

void Warmup()
{
PathSearchAlgorithm MinimumSetting = new()
{
    Queue = [new(Graphs[2].Nodes[0])],
    Width = int.MaxValue,
    EmptyAgenda = false,
    UseExtendedList = false,
    SortBeforeInsert = false,
    InsertToEnd = false,
    SortMode = 0,
};

PathSearchAlgorithm MaximumSetting1 = new()
{
    Queue = [new(Graphs[2].Nodes[0])],
    Width = int.MaxValue,
    EmptyAgenda = true,
    UseExtendedList = true,
    SortBeforeInsert = true,
    InsertToEnd = true,
    SortMode = ComparerType.CostComparer,
};
PathSearchAlgorithm MaximumSetting2 = new()
{
    Queue = [new(Graphs[2].Nodes[0])],
    Width = int.MaxValue,
    EmptyAgenda = true,
    UseExtendedList = true,
    SortBeforeInsert = true,
    InsertToEnd = true,
    SortMode = ComparerType.HeuristicComparer,
};
PathSearchAlgorithm MaximumSetting3 = new()
{
    Queue = [new(Graphs[2].Nodes[0])],
    Width = int.MaxValue,
    EmptyAgenda = true,
    UseExtendedList = true,
    SortBeforeInsert = true,
    InsertToEnd = true,
    SortMode = ComparerType.HeuristicAndCostComparer,
};
MinimumSetting.Execute();
MaximumSetting1.Execute();
MaximumSetting2.Execute();
MaximumSetting3.Execute();
}