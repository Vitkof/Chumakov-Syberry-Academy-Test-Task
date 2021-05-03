using System;
using System.Collections.Generic;
using System.IO;

public class Rover
{
    static (Dictionary<string, string>, int) Dejkstra(string startNode, string endNode,
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

        return (visitedFrom, fuelVisit[endNode]);
    }

    delegate bool CheckNextNode(int x, int y);
    public static void CalculateRoverPath(int[,] map)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);
        Dictionary<string, (int, string)[]> graph = new Dictionary<string, (int, string)[]>();

        CheckNextNode check = (i, j) =>
        (0 <= i && i < rows) && (0 <= j && j < cols);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                List<(int, string)> arr = new List<(int, string)>();
                (int di, int dj)[] sides = { (-1, 0), (1, 0), (0, -1), (0, 1) };
                foreach (var (di, dj) in sides)
                {
                    if (check(i + di, j + dj))
                        arr.Add((Math.Abs(map[i + di, j + dj] - map[i, j]),
                            $"[{i + di}][{j + dj}]"));
                }
                graph.Add($"[{i}][{j}]", arr.ToArray());
            }
        }

        string start = "[0][0]";
        string end = $"[{rows - 1}][{cols - 1}]";
        var dejkstra = Dejkstra(start, end, graph);
        var visited = dejkstra.Item1;
        int fuel = dejkstra.Item2;

        string currNode = end;
        string path = $"{currNode}";
        int steps = 0;

        while (currNode != start)
        {
            currNode = visited[currNode];
            path = path.Insert(0, $"{currNode}->");
            steps++;
        }

        using (StreamWriter sw = new StreamWriter("path-plan.txt", true))
        {
            sw.WriteLine(path);
            sw.WriteLine($"steps: {steps}");
            sw.WriteLine($"fuel: {fuel}");
        }
    }

    static void Main(string[] args)
    {
        CalculateRoverPath(new int[,] {
            { 0, 2, 3, 4, 1 },
            { 2, 3, 4, 4, 1 },
            { 3, 4, 5, 6, 2 },
            { 4, 5, 6, 7, 1 },
            { 6, 7, 8, 7, 1 }
        });
        CalculateRoverPath(new int[,] {
            { 0, 1, 1, 1, 0 },
            { 1, 1, 3, 1, 1 },
            { 0, 1, 1, 1, 0 },
            { 0, 0, 0, 0, 0 },
        });
        Console.ReadKey();
    }
}
