using System.Collections;

class PathSearchAlgorithm
{
    /*
        Global, static members
    */
    public static Node StartNode = new("not set");
    public static Node EndNode = new("not set");

    /*
        Private members
    */
    private ComparerType _sortMode = 0;
    private IComparer<Path> Comparer = Comparer<Path>.Default;

    /*
        Public members
    */
    public string AlgorithmName = "Not named";

    /*
        Algorithm settings
    */
    public required List<Path> Queue { get; set; }
    public bool EmptyAgenda { get; set; }
    

    public bool SortBeforeInsert { get; set; }
    public ComparerType SortMode
    {
        get {return _sortMode; }
        set
        {
            switch(value)
            {
                case ComparerType.CostComparer: Comparer = new PathCostComparer(); break;
                case ComparerType.HeuristicComparer: Comparer = new PathHeuristicComparer(EndNode); break;
                case ComparerType.HeuristicAndCostComparer: Comparer = new PathHeuristicAndCostComparer(EndNode); break;
                default: Comparer = Comparer<Path>.Default; break;
            }
            _sortMode = value;
        }
    }
    

    public bool InsertToEnd { get; set; }


    public bool UseExtendedList { get; set; }
    public HashSet<Node> ExtendedList = new HashSet<Node>();


    public int Width = int.MaxValue;

    /*
        Analytic variables
    */
    public int StepCount = 0;
    public Path? Result = null;
    public TimeSpan Runtime;


    public override string ToString()
    {
        string outStr = $"{AlgorithmName}".PadRight(20,' ');
        outStr += $"{Result}".PadRight(20, ' ');
        outStr += $"{Runtime.TotalMilliseconds}".PadRight(20, ' ');
        outStr += StepCount;
        return outStr;
    }

    private void EmptyAgendaProcess(ref List<Path> newAddition)
    {
        while (Queue.Count > 0)
        {
            Path _path = DeQueue();
            if (_path.GetLast() == EndNode)
            {
                Result = _path;
                return;
            }
            StepCount++;
            if(InsertToEnd) newAddition.AddRange(Expand(_path));
            else newAddition.InsertRange(0, Expand(_path));
        }
    }

    private Path DeQueue()
    {
        Path path = Queue[0];
        Queue.RemoveAt(0);
        return path;
    }

    public void Execute()
    {
        while (Queue.Count > 0)
        {

            Path path = DeQueue();

            if (path.GetLast() == EndNode)
            {
                
                Result = path;
                return;
            }

            List<Path> newAddition = [.. Expand(path)];
            StepCount++;

            if(EmptyAgenda)
            {
                EmptyAgendaProcess(ref newAddition);
                if(Result != null) return;
            }
            
            InsertToQueue(newAddition);
            
            if(Width != int.MaxValue)
            {
                Queue = Queue.Slice(0, Math.Min(Width, Queue.Count));
            }

        }
        Result = new Path(new("NO PATH FOUND"));
    }

    private List<Path> Expand(Path _path)
    {
        List<Path> newAdditions = new List<Path>();

        Node endNode = _path.GetLast(); 
        if(UseExtendedList) ExtendedList.Add(endNode);
        bool opened = false;
        foreach(Edge edge in endNode.Edges)
        {
            
            if(edge.Cost == -1 || _path.Search(edge._node)) continue;

            if(UseExtendedList && ExtendedList.Contains(edge._node)) continue;
            opened = true;
            Path.CreateCopy(_path, out Path newPath);
            newPath.Insert(edge._node);
            newAdditions.Add(newPath);
        }

        if(!opened) StepCount--;

        return newAdditions;

    }

    private void InsertToQueue(List<Path> _pathsToInsert)
    {
        if(SortBeforeInsert && SortMode != 0)
        {
            _pathsToInsert.Sort(Comparer);
        }
        
        if(InsertToEnd) Queue.AddRange(_pathsToInsert);
        else Queue.InsertRange(0, _pathsToInsert);
        
        if(!SortBeforeInsert && SortMode != 0) {
            Queue.Sort(Comparer);
        }

    }
}