# Search

## Terminology

- Informed vs. Uninformed
  - Whether there is some evaluation function $f(x)$ that help guide your search. Except for
    BFS, DFS, and British Museum all the other searches we studied in this class are
    informed in some way.
- Complete vs. Incomplete
  - If there exists a solution (path from s to g) the algorithm will find it.
- Optimal vs. Non-optimal
  - The solution found is also the best one (best counted by the cost of the path).

## Generic Search Algorithm:

<pre>
function Search(graph, start, goal):
    0. Initialize
        agenda = [ [start] ]
        <b style="color:red">extended_list = []</b>
    while agenda is not empty:
        1. path = agenda.pop(0) # get first element from agenda & return it
        2. if is-path-to-goal(path, goal)
            return path
        3. otherwise extend the current path <b style="color:red">if not already extended</b>
            for each connected node
                make a new path (don't add paths with loops!)
    4. add new paths from 3 to agenda and reorganize agenda
        (algorithms differ here see table below)
    fail!
</pre>

The code in <b style="color:red">red</b> only applies if you are using an extended list.

- **Agenda** keeps track of all the paths under consideration, and the way it is maintained is
  the key to the difference between most of the search algorithms.
- **Loops in paths**: Thou shall not create or consider paths with cycles in step 3.
- **Extended list** is the list of nodes that has undergone "extension" (step 3).
  Using an extended list is an optional optimization that could be applied to all algorithms.
  (some with implications, see A\*) In some literature extended list is also referred to as
  "closed" list, and the agenda the "open" list.
- **Backtracking**: When we talk about DFS or DFS variants (like Hill Climbing) we talk about
  with or without "backtracking". You can think of backtracking in terms of the agenda. If
  we make our agenda size 1, then this is equivalent to having no backtracking. Having
  agenda size > 1 means we have some partial path to go back on, and hence we can
  backtrack.
- **Exiting the search**: Non-optimal searches may actually exit when it finds or adds a path
  with a goal node to the agenda (at step 3). But optimal searches must only exit when the
  path is the first removed from the agenda (step 1,2).

| Search Algorithm           | Properties                                                                                                                                                                                           | Required Parameters                                                                                               | What it does with the agenda in step 4.                                                                                                                      |
| :------------------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :---------------------------------------------------------------------------------------------------------------- | :----------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Breadth-First Search       | Uninformed, Nonoptimal (Exception: Optimal only if you are counting total path length), Complete                                                                                                     |                                                                                                                   | Add all new paths to the BACK of the agenda, like a queue (FIFO)                                                                                             |
| Depth-First Search         | Uninformed, Non-optimal, Incomplete                                                                                                                                                                  |                                                                                                                   | Add all new paths to the FRONT of the agenda, like a stack (FILO)                                                                                            |
| Best-First Search          | Depending on definition of $f(x)$. If $f(x) = h(x)$ (estimated distance to goal) then likely not optimal, and potentially incomplete.                                                                | $f(x)$ to sort the entire agenda. by                                                                              | Keep entire agenda sorted by $f(x)$.                                                                                                                         |
| Hill Climbing              | Non-optimal, Incomplete, Like DFS with a heuristic                                                                                                                                                   | $f(x)$ to sort the newly added paths by.                                                                          | 1. Keep only newly added paths sorted by $f(x)$<br> 2. Add sorted new paths to the FRONT of agenda                                                           |
| Beam Search                | Like BFS but expand nodes in $f(x)$ order. Incomplete for small $w$; Complete and like BFS for $w = \inf$. Non-optimal When $w = 1$, Beam search is analogous to Hill Climbing without backtracking. | 1. the beam width $w$<br> 2. $f(x)$ to sort the top paths by.                                                     | 1. Keep only $w$ top paths that are of length $n$. (So keep a sorted list of paths for every path length)<br> 2. Keep only top $w$ paths as sorted by $f(x)$ |
| Branch & Bound             | Optimal                                                                                                                                                                                              | $g(x) = c(s, x) =$ the cost of path from $s$ to node $x$. $f(x) = g(x) + 0$                                       | Sort paths by $f(x)$                                                                                                                                         |
| Branch & Bound w Heuristic | Optimal if $h$ is admissible                                                                                                                                                                         | $f(x) = g(x) + h(x,g) h(x,g)$ is the estimate of the cost from $x$ to $g$. $h(x)$ must be an admissible heuristic | Sort paths by $f(x)$                                                                                                                                         |
| A\*                        | Optimal if $h$ is consistent                                                                                                                                                                         | $f(x) = g(x) + h(x) h(x)$ must be a consistent heuristic                                                          | Sort paths by $f(x)$                                                                                                                                         |

### Definitions:

- $f(x)$ is the total cost of the path that your algorithm uses to rank paths.
- $g(x)$ is the cost of the path so far.
- $h(x)$ is the (under)estimate of the remaining cost to the goal $g$ node.
- $f(x) = g(x) + h(x)$
- $c(x, y)$ is the actual cost to go from node $x$ to node $y$.

#### Admissible Heuristic:

For all nodes $x$ in Graph, $h(x) <= c(n, g)$, i.e. the heuristic is an underestimate of the actual cost/distance to the goal.

#### Consistent Heuristic:

- For edges in an undirected graph, where $m$ is connected to $n$: $|h(m) - h(n)| \leq c(m, n)$
- For edges in a directed graph $n$ is a descendent of $m$ or $m \rightarrow n$: $h(m) - h(n) \leq c(m,n)$
- You can verify consistency by checking each edge and see if difference between $h$ values on an edge $\leq$ the actual edge cost.

Consistency implies Admissibility.
If you can verify consistency, then the heuristic must be admissible.
But Admissibility does not imply Consistency!
