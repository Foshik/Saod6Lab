namespace Saod6Lab.GraphInput;

public class Graph
{
    public List<Node> Nodes { get; set; } = new List<Node>();

    public void AddNode(int id)
    {
        Node newNode = new Node { Id = id };
        Nodes.Add(newNode);
    }

    public void AddEdge(int sourceId, int targetId)
    {
        Node source = Nodes.Find(n => n.Id == sourceId);
        Node target = Nodes.Find(n => n.Id == targetId);

        if (source != null && target != null)
        {
            source.Neighbors.Add(target);
            target.Neighbors.Add(source);
        }
        else
        {
            throw new Exception($"Не удалось найти узлы с ID {sourceId} и {targetId}");
        }
    }

    public static Graph GenerateRandomGraph(int numberOfNodes, int numberOfEdges)
    {
        Graph graph = new Graph();

        // Добавляем узлы
        for (int i = 1; i <= numberOfNodes; i++)
        {
            graph.AddNode(i);
        }

        // Создаем список всех возможных ребер
        List<(int, int)> possibleEdges = new List<(int, int)>();
        for (int i = 1; i <= numberOfNodes; i++)
        {
            for (int j = i + 1; j <= numberOfNodes; j++)
            {
                possibleEdges.Add((i, j));
            }
        }

        // Перемешиваем список ребер и добавляем первые `numberOfEdges` ребер в граф
        ShuffleList(possibleEdges);
        for (int i = 0; i < numberOfEdges; i++)
        {
            var (source, target) = possibleEdges[i];
            graph.AddEdge(source, target);
        }

        return graph;
    }

    private static void ShuffleList<T>(IList<T> list)
    {
        Random rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


    public static void PrintGraphAsText(Graph graph)
    {
        Console.WriteLine("Граф:");
        Console.WriteLine("Узлы:");
        foreach (Node node in graph.Nodes)
        {
            Console.WriteLine($"- {node.Id}");
        }

        Console.WriteLine("\nРебра:");
        HashSet<(int, int)> processedEdges = new HashSet<(int, int)>();
        foreach (Node node in graph.Nodes)
        {
            foreach (Node neighbor in node.Neighbors)
            {
                int sourceId = Math.Min(node.Id, neighbor.Id);
                int targetId = Math.Max(node.Id, neighbor.Id);
                if (!processedEdges.Contains((sourceId, targetId)))
                {
                    Console.WriteLine($"- {sourceId} -> {targetId}");
                    processedEdges.Add((sourceId, targetId));
                }
            }
        }
    }
    public static int[,] GetIncidenceMatrix(Graph graph)
    {
        int numNodes = graph.Nodes.Count;
        int numEdges = 0;
        foreach (Node node in graph.Nodes)
        {
            numEdges += node.Neighbors.Count;
        }

        numEdges /= 2; // Каждое ребро учитывается дважды

        int[,] incidenceMatrix = new int[numNodes, numEdges];

        int edgeIndex = 0;
        foreach (Node node in graph.Nodes)
        {
            foreach (Node neighbor in node.Neighbors)
            {
                if (node.Id < neighbor.Id)
                {
                    incidenceMatrix[node.Id - 1, edgeIndex] = 1;
                    incidenceMatrix[neighbor.Id - 1, edgeIndex] = 1;
                    edgeIndex++;
                }
            }
        }

        return incidenceMatrix;
    }

    public static void PrintIncidenceMatrix(Graph graph)
    {
        int[,] incidenceMatrix = GetIncidenceMatrix(graph);
        int numNodes = incidenceMatrix.GetLength(0);
        int numEdges = incidenceMatrix.GetLength(1);

        Console.WriteLine("Матрица инцидентности:");

        // Печатаем заголовок
        Console.Write("   ");
        int maxLength = 0;
        string[] edgeLabels = new string[numEdges];
        for (int j = 0; j < numEdges; j++)
        {
            string edgeLabel = $" e{j + 1}";
            edgeLabels[j] = edgeLabel;
            int labelLength = edgeLabel.Length;
            Console.Write(edgeLabel.PadRight(labelLength + 3));
            if (labelLength > maxLength)
            {
                maxLength = labelLength;
            }
        }

        Console.WriteLine();

        // Печатаем строки матрицы
        for (int i = 0; i < numNodes; i++)
        {
            string nodeLabel = $"v{i + 1}";
            Console.Write(nodeLabel.PadRight(maxLength));
            for (int j = 0; j < numEdges; j++)
            {
                string cellValue = $"{incidenceMatrix[i, j]}";
                if (j == i)
                {
                    cellValue = cellValue.PadLeft(maxLength + 1);
                }
                else
                {
                    cellValue = cellValue.PadLeft(maxLength + 1);
                }

                Console.Write(cellValue);
                if (j < numEdges - 1)
                {
                    Console.Write(" ");
                }
            }

            Console.WriteLine();
        }
    }
    public static List<(int, int)> GetEdgeList(Graph graph)
    {
        List<(int, int)> edgeList = new List<(int, int)>();
        HashSet<(int, int)> uniqueEdges = new HashSet<(int, int)>();

        foreach (Node node in graph.Nodes)
        {
            foreach (Node neighbor in node.Neighbors)
            {
                // Добавляем ребро только если оно еще не было добавлено
                if (node.Id < neighbor.Id && !uniqueEdges.Contains((node.Id, neighbor.Id)))
                {
                    edgeList.Add((node.Id, neighbor.Id));
                    uniqueEdges.Add((node.Id, neighbor.Id));
                    uniqueEdges.Add((neighbor.Id, node.Id)); // Добавляем оба направления ребра
                }
            }
        }

        return edgeList;
    }

    public static void PrintEdgeList(Graph graph)
    {
        var edgeList = GetEdgeList(graph);
        Console.WriteLine("Список ребер:");
        Console.WriteLine("-------------");

        foreach ((int from, int to) in edgeList)
        {
            Console.WriteLine($"({from}, {to})");
        }

        Console.WriteLine("-------------");
    }
}
