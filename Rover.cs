using System;
using System.Collections.Generic;

public class Rover
{
    static Dictionary<string, string> Dejkstra(string startNode, string endNode,
        Dictionary<string, (int, string)[]> graph)
    {
        SortedList<int, string> nodesQueue = new SortedList<int, string>() { { 0, startNode } };
        Dictionary<string, int> fuelVisit = new Dictionary<string, int>() { { startNode, 0 } };
        Dictionary<string, string> visitedFrom = new Dictionary<string, string>() { { startNode, null } };

        while (nodesQueue.Count != 0)
        {
            string currNode = nodesQueue.Values[0];
            nodesQueue.RemoveAt(0);
            if (currNode == endNode) break;  //end of algorithm 

            var nextNodes = graph[currNode];
            foreach ((int, string) nextNode in nextNodes)
            {
                int neighbourFuel = nextNode.Item1;
                string neighbourNode = nextNode.Item2;
                var updateFuel = fuelVisit[currNode] + neighbourFuel + 1; //+1 point fuel on move

                if (!fuelVisit.ContainsKey(neighbourNode) ||
                    updateFuel < fuelVisit[neighbourNode])
                {
                    fuelVisit[neighbourNode] = updateFuel;
                    nodesQueue.Add(updateFuel, neighbourNode);
                    visitedFrom[neighbourNode] = currNode;
                }
            }
        }

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
