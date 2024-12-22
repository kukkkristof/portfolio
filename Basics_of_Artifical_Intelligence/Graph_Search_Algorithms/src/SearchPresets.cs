class SearchPreset
{
    public static PathSearchAlgorithm DFS_PRESET {
        get {
        return new() {AlgorithmName = "dfs",
        Queue = [new(PathSearchAlgorithm.StartNode)]};
        }
    }
    public static PathSearchAlgorithm BFS_PRESET {
        get {
        return new() {AlgorithmName = "bfs",
        Queue = [new(PathSearchAlgorithm.StartNode)],

        InsertToEnd = true};
        }
    }
    public static PathSearchAlgorithm BEST_FIRST_PRESET {
        get {
        return new() {AlgorithmName = "best-first",
        Queue = [new(PathSearchAlgorithm.StartNode)],

        SortMode = ComparerType.HeuristicComparer};
        }
    }
    public static PathSearchAlgorithm HILL_CLIMBING_PRESET {
        get {
        return new() {AlgorithmName = "hill-climbing",
        Queue = [new(PathSearchAlgorithm.StartNode)],

        SortBeforeInsert = true,
        SortMode = ComparerType.HeuristicComparer};
        }
    }
    public static PathSearchAlgorithm BEAM_PRESET {
        get {
        return new() {AlgorithmName = "beam",
        Queue = [new(PathSearchAlgorithm.StartNode)],

        InsertToEnd = true,
        EmptyAgenda = true,
        SortMode = ComparerType.HeuristicComparer,
        Width = 2};
        }
    }
    public static PathSearchAlgorithm BRANCH_AND_BOUND_PRESET {
        get {
        return new() {AlgorithmName = "b & b",
        Queue = [new(PathSearchAlgorithm.StartNode)],

        InsertToEnd = true,
        SortMode = ComparerType.CostComparer};
        }
    }
    public static PathSearchAlgorithm BRANCH_AND_BOUND_EXT_LIST_PRESET {
        get {
        return new() {AlgorithmName = "b & b + ext set",
        Queue = [new(PathSearchAlgorithm.StartNode)],

        InsertToEnd = true,
        UseExtendedList = true,
        SortMode = ComparerType.CostComparer,
        };
        }
    }
    public static PathSearchAlgorithm BRANCH_AND_BOUND_HEUR_PRESET {
        get {
        return new() {AlgorithmName = "b & b + heuristic",
        Queue = [new(PathSearchAlgorithm.StartNode)],

        InsertToEnd = true,
        SortMode = ComparerType.HeuristicAndCostComparer};
        }
    }
    public static PathSearchAlgorithm A_STAR_PRESET {
        get {
        return new() {AlgorithmName = "A*",
        Queue = [new(PathSearchAlgorithm.StartNode)],

        UseExtendedList = true,
        InsertToEnd = true,
        SortMode = ComparerType.HeuristicAndCostComparer};
        }
    }
}