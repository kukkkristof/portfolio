namespace TSP;

public class Routes
{
    public List<Route> RouteList { get; set; } = [];
    
    public Route Best => RouteList.OrderByDescending(x=> x.Fitness).First();
    public Route Worst => RouteList.OrderByDescending(x=> x.Fitness).Last();
    public Route Random => RouteList[Randomizer.Next(0, RouteList.Count)];

    public void Sort()
    {
        RouteList = RouteList.OrderByDescending(x=> x.Fitness).ToList();
    }
    
}