using System;
using System.Collections.Generic;
using System.IO;


public class Node : IComparable<Node>
{
    public int I { get; set; }
    public int J { get; }
    public Node(int i, int j)
    {
        I = i;
        J = j;
    }
    internal string Show()
    {
        return $"[{I}][{J}]";
    }

    public int CompareTo(Node other)
    {
        return I.CompareTo(other.I);
    }
}

public class Rover
{
    static int Heuristic(Node a, Node end)
    {
        return Math.Abs(a.I - end.I) + Math.Abs(a.J - end.J);
    }

    static (Dictionary<Node, Node>, int) Dejkstra(Node startNode, Node endNode,
        Dictionary<Node, (int, Node)[]> graph)
    {
        List<(int, Node)> nodesQueue = new List<(int, Node)>() { (0, startNode) };
        Dictionary<Node, int> fuelVisit = new Dictionary<Node, int>() { { startNode, 0 } };
        Dictionary<Node, Node> visitedFrom = new Dictionary<Node, Node>() { { startNode, null } };

        while (nodesQueue.Count != 0)
        {
            Node currNode = nodesQueue[0].Item2;
            nodesQueue.RemoveAt(0);            
            if (currNode == endNode) break;  //end of algorithm 

            var nextNodes = graph[currNode];
            foreach ((int, Node) nextNode in nextNodes)
            {
                int neighbourFuel = nextNode.Item1;
                Node neighbourNode = nextNode.Item2;
                var updateFuel = fuelVisit[currNode] + neighbourFuel + 1; //+1 point fuel on move

                if (!fuelVisit.ContainsKey(neighbourNode) ||
                    updateFuel < fuelVisit[neighbourNode])
                {
                    int priority = updateFuel + Heuristic(neighbourNode, endNode); //update to *A-algorithm
                    fuelVisit[neighbourNode] = updateFuel;
                    nodesQueue.Add((priority, neighbourNode));
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
        Dictionary<Node, (int, Node)[]> graph = new Dictionary<Node, (int, Node)[]>();

        CheckNextNode check = (i, j) =>
        (0 <= i && i < rows) && (0 <= j && j < cols);

        Node[,] nodes = new Node[rows,cols];
        for(int i=0; i<rows;i++)
            for(int j=0; j<cols;j++)
                nodes[i, j] = new Node(i, j);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {               
                List<(int, Node)> arr = new List<(int, Node)>();
                (int di, int dj)[] sides = { (-1, 0), (1, 0), (0, -1), (0, 1) };
                foreach (var (di, dj) in sides)
                {
                    if (check(i + di, j + dj))
                        arr.Add((Math.Abs(map[i + di, j + dj] - map[i, j]),
                            nodes[i + di, j + dj]));
                }
                graph.Add(nodes[i,j], arr.ToArray());
            }
        }

        Node start = nodes[0, 0];
        Node end = nodes[rows - 1, cols - 1];
        var dejkstra = Dejkstra(start, end, graph);
        var visited = dejkstra.Item1;
        int fuel = dejkstra.Item2;

        Node currNode = end;
        string path = $"{currNode.Show()}";
        int steps = 0;

        while (currNode != start)
        {
            currNode = visited[currNode];
            path = path.Insert(0, $"{currNode.Show()}->");
            steps++;
        }

        using (StreamWriter sw = new StreamWriter("path-plan.txt"))
        {
            sw.WriteLine(path);
            sw.WriteLine($"steps: {steps}");
            sw.WriteLine($"fuel: {fuel}");
        }
    }

    static void Main(string[] args)
    {
    }
}
