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

class CannotStartMovement : Exception
{
    public CannotStartMovement(string msg) : base(msg)
    {

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
        int diagCountMove = 0;
        

        while (nodesQueue.Count != 0)
        {
            Node currNode = nodesQueue[0].Item2;
            nodesQueue.RemoveAt(0);            
            if (currNode == endNode) break;  //end of algorithm 

            var nextNodes = graph[currNode];
            foreach ((int, Node) nextNode in nextNodes)
            {
                bool inDiagonal = false;
                int neighbourFuel = nextNode.Item1;
                Node neighbourNode = nextNode.Item2;

                var updateFuel = fuelVisit[currNode] + neighbourFuel + 1; //+1 point fuel on move
                if (Math.Abs(neighbourNode.I - currNode.I) == 1 &&   //check diagonal
                    Math.Abs(neighbourNode.J - currNode.J) == 1)
                {
                    if (diagCountMove % 2 != 0) updateFuel++;
                    inDiagonal = true;
                }


                if (!fuelVisit.ContainsKey(neighbourNode) ||
                    updateFuel < fuelVisit[neighbourNode])
                {
                    int priority = updateFuel + Heuristic(neighbourNode, endNode); //update to *A-algorithm
                    fuelVisit[neighbourNode] = updateFuel;
                    nodesQueue.Add((priority, neighbourNode));
                    nodesQueue.Sort();
                    visitedFrom[neighbourNode] = currNode;
                    if(inDiagonal) diagCountMove++;

                }
            }
        }

        return (visitedFrom, fuelVisit[endNode]);
    }


    delegate bool CheckNextNode(int x, int y);
    public static void CalculateRoverPath(string[,] map)
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
                (int di, int dj)[] sides = { (-1, 0), (1, 0), (0, -1), (0, 1),
                                            (-1, -1), (-1, 1), (1, -1), (1, 1)};

                foreach (var (di, dj) in sides)
                {
                    if (check(i + di, j + dj))
                    {
                        try
                        {
                            int x = Convert.ToInt32(map[i + di, j + dj]);
                            int y = Convert.ToInt32(map[i, j]);
                            arr.Add((Math.Abs(x - y), nodes[i + di, j + dj]));
                        }
                        catch
                        {
                            if (map[i + di, j + dj] != "X" || map[i, j] != "X")
                                throw new Exception("Matrix contains not only numbers and a sign \"X\"");
                            else continue;
                        }
                                                
                    }

                        
                        
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
        CalculateRoverPath(new string[,] {
            { "0", "-2", "3", "4", "1" },
            { "2", "3", "4", "4", "1" },
            { "3", "4", "5", "6", "-2" },
            { "4", "5", "6", "7", "1" },
            { "6", "7", "8", "7", "1" }
        });

        Console.ReadKey();
    }
}
