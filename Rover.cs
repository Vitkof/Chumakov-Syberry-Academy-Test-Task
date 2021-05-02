using System;
using System.Collections.Generic;

public class Rover
{
    static Dictionary<string, string> Dejkstra(string startNode, string endNode,
        Dictionary<string, (int, string)[]> graph)
    {
        List<(int, string)> nodesQueue = new List<(int, string)>() { (0, startNode) };
        Dictionary<string, int> fuelVisit = new Dictionary<string, int>() { { startNode, 0 } };
        Dictionary<string, string> visitedFrom = new Dictionary<string, string>() { { startNode, null } };

        while (nodesQueue.Count != 0)
        {
            string currNode = nodesQueue[0].Item2;
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
                    nodesQueue.Add((updateFuel, neighbourNode));
                    nodesQueue.Sort();
                    visitedFrom[neighbourNode] = currNode;
                }
            }
        }

        return visitedFrom;
    }

    delegate bool CheckNextNode(int x, int y);
    public static void CalculateRoverPath(int[,] map)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);
        Dictionary<string, (int, string)[]> graph = new Dictionary<string, (int, string)[]>();

        CheckNextNode check = delegate (int i, int j)
        {
            return (0 <= i && i < rows) 
            && (0 <= j && j < cols)
            ? true
            : false;
        };
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                (int, string)[] arr = new (int, string)[4];
                if (check(i - 1, j)) { arr[0] = (map[i - 1, j], $"[{i - 1}][{j}]"); };
                if (check(i, j + 1)) { arr[1] = (map[i, j + 1], $"[{i}][{j + 1}]"); };
                if (check(i + 1, j)) { arr[2] = (map[i + 1, j], $"[{i + 1}][{j}]"); };
                if (check(i, j - 1)) { arr[3] = (map[i, j - 1], $"[{i}][{j - 1}]"); };
                graph.Add($"[{i}][{j}]", arr);                
            }
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");

        Dictionary<string, (int, string)[]> gr1 = new Dictionary<string, (int, string)[]>()
        {
            {"A", new (int, string)[] { (2, "B"), (3, "F") } },
            {"B", new (int, string)[] { (2, "A"), (2, "C") } },
            {"C", new (int, string)[] { (2, "B"), (2, "D") } },
            {"D", new (int, string)[] { (2, "C"), (1, "F") } },
            {"F", new (int, string)[] { (3, "A"), (1, "D") } }
        };
        string start = "A";
        string end = "D";
        var visited = Dejkstra(start, end, gr1);

        string currNode = end;
        Console.Write($"{currNode}");

        while (currNode != start)
        {
            currNode = visited[currNode];
            Console.Write($"->{currNode}");
        }

        Console.ReadKey();
    }
}
