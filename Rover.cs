using System;

public class Rover
{
    static Dictionary<string, string> Dejkstra(string startNode, string endNode, 
        Dictionary<string, (int, string)[]> graph)
    {
        SortedList<int, string> nodesQueue = new SortedList<int, string>() { { 0, startNode } };
        Dictionary<string, int> fuelVisit = new Dictionary<string, int>() { { startNode, 0 } };
        Dictionary<string, string> visitedFrom = new Dictionary<string, string>() { { startNode, null } };
        
        return visitedFrom;
    }
    
    public static void CalculateRoverPath(int[,] map)
    {
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
    }
}
