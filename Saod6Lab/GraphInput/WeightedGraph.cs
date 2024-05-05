using Saod6Lab.GraphInput;

public class WeightedGraph
{
    public List<Node> Nodes { get; set; } = new List<Node>();
    public List<Edge> Edges { get; set; } = new List<Edge>();

    public WeightedGraph(Graph graph)
    {
        // Копируем узлы из исходного графа
        Nodes = new List<Node>(graph.Nodes);

        // Создаем ребра между связанными узлами и присваиваем им случайный вес
        Random random = new Random();
        foreach (Node node in graph.Nodes)
        {
            foreach (Node neighbor in node.Neighbors)
            {
                int weight = random.Next(1, 31);
                Edges.Add(new Edge { Source = node, Target = neighbor, Weight = weight });
                Edges.Add(new Edge { Source = neighbor, Target = node, Weight = weight });
            }
        }
    }
    public void CopyFromGraph(Graph graph)
    {
        // Копируем узлы из исходного графа
        foreach (Node node in graph.Nodes)
        {
            if (!Nodes.Contains(node))
            {
                Nodes.Add(node);
            }
        }

        // Создаем ребра между связанными узлами и присваиваем им случайный вес
        Random random = new Random();
        foreach (Node node in graph.Nodes)
        {
            foreach (Node neighbor in node.Neighbors)
            {
                Edge existingEdge = Edges.FirstOrDefault(e => (e.Source == node && e.Target == neighbor) || (e.Source == neighbor && e.Target == node));
                if (existingEdge == null)
                {
                    int weight = random.Next(1, 31);
                    Edges.Add(new Edge { Source = node, Target = neighbor, Weight = weight });
                    Edges.Add(new Edge { Source = neighbor, Target = node, Weight = weight });
                }
            }
        }
    }


   public static void PrintEdges(WeightedGraph graph)
   {
       HashSet<(int, int)> processedEdges = new HashSet<(int, int)>();
   
       Console.WriteLine("Список ребер:");
       Console.WriteLine("-------------");
   
       foreach (Edge edge in graph.Edges)
       {
           int fromId = edge.Source.Id;
           int toId = edge.Target.Id;
   
           // Убедимся, что мы еще не обрабатывали это ребро
           if (!processedEdges.Contains((fromId, toId)))
           {
               Console.WriteLine($"{fromId} -> {toId} весит {edge.Weight}");
               processedEdges.Add((fromId, toId));
               processedEdges.Add((toId, fromId));
           }
       }
   
       Console.WriteLine("-------------");
   }
   public static List<Edge> AlgorithmKruskal(WeightedGraph graph)
   {
       // Сортируем ребра по возрастанию веса
       List<Edge> sortedEdges = graph.Edges.OrderBy(e => e.Weight).ToList();

       // Создаем список для минимального остовного дерева
       List<Edge> minimumSpanningTree = new List<Edge>();

       // Создаем массив для отслеживания множеств
       int[] sets = Enumerable.Range(0, graph.Nodes.Count).ToArray();

       // Обходим ребра в порядке возрастания веса
       foreach (Edge edge in sortedEdges)
       {
           int sourceRoot = FindRoot(sets, edge.Source.Id, graph.Nodes.Count);
           int targetRoot = FindRoot(sets, edge.Target.Id, graph.Nodes.Count);

           // Если корни разные, значит ребро не создает цикл, добавляем его в минимальное остовное дерево
           if (sourceRoot != targetRoot)
           {
               minimumSpanningTree.Add(edge);
               Union(sets, sourceRoot, targetRoot, graph.Nodes.Count);
           }
           else
           {
               // Ребро создает цикл, пропускаем его
               continue;
           }
       }

       return minimumSpanningTree;
   }

   private static int FindRoot(int[] sets, int node, int setsLength)
   {
       // Находим корень множества, к которому принадлежит узел
       while (node >= 0 && node < setsLength && sets[node] != node)
       {
           if (sets[node] < 0 || sets[node] >= setsLength)
               return -1; // Возвращаем -1, если индекс выходит за пределы массива
           sets[node] = sets[sets[node]]; // Сжатие пути
           node = sets[node];
       }
       return node;
   }

   private static void Union(int[] sets, int x, int y, int setsLength)
   {
       int xRoot = FindRoot(sets, x, setsLength);
       int yRoot = FindRoot(sets, y, setsLength);

       // Проверяем, что корни находятся в пределах массива
       if (xRoot < 0 || xRoot >= setsLength || yRoot < 0 || yRoot >= setsLength)
           return;

       sets[xRoot] = yRoot; // Объединяем множества
   }



   public static void PrintAlgorithKarsky(WeightedGraph graph)
   {
       List<Edge> minimumSpanningTree = AlgorithmKruskal(graph);
       HashSet<(int, int)> processedEdges = new HashSet<(int, int)>();

       Console.WriteLine("Минимальное остовное дерево:");
       Console.WriteLine("-------------");

       foreach (Edge edge in minimumSpanningTree)
       {
           int sourceId = edge.Source.Id;
           int targetId = edge.Target.Id;

           // Проверяем, чтобы ребро не было уже обработано в обратном направлении
           if (sourceId < targetId && !processedEdges.Contains((sourceId, targetId)))
           {
               Console.WriteLine($"{sourceId} -> {targetId} весит {edge.Weight}");
               processedEdges.Add((sourceId, targetId));
               processedEdges.Add((targetId, sourceId));
           }
       }

       Console.WriteLine("-------------");
   }
   public static Graph FindMinimumSpanningTree(Graph graph, int startNodeId)
   {
       // Создаем новый граф для минимального остовного дерева
       Graph mstGraph = new Graph();

       // Создаем словарь для хранения расстояний до узлов
       Dictionary<int, int> distances = new Dictionary<int, int>();

       // Создаем список посещенных узлов
       HashSet<int> visited = new HashSet<int>();

       // Инициализируем расстояния до всех узлов как бесконечность, кроме стартового узла
       foreach (var node in graph.Nodes)
       {
           distances[node.Id] = int.MaxValue;
       }
       distances[startNodeId] = 0;

       // Приоритетная очередь для хранения узлов с минимальными расстояниями
       PriorityQueue<int, int> queue = new PriorityQueue<int, int>();
       queue.Enqueue(startNodeId, 0);

       while (queue.Count > 0)
       {
           // Извлекаем узел с минимальным расстоянием
           int currentNodeId = queue.Dequeue();

           // Если узел уже посещен, пропускаем его
           if (visited.Contains(currentNodeId))
               continue;

           // Помечаем узел как посещенный
           visited.Add(currentNodeId);

           // Добавляем узел в минимальное остовное дерево
           mstGraph.AddNode(currentNodeId);

           // Рассматриваем соседей текущего узла
           var currentNode = graph.Nodes.Find(n => n.Id == currentNodeId);
           foreach (var neighbor in currentNode.Neighbors)
           {
               int neighborId = neighbor.Id;

               // Обновляем расстояния до соседей, если они меньше текущих
               if (!visited.Contains(neighborId) && distances[neighborId] > distances[currentNodeId] + 1)
               {
                   distances[neighborId] = distances[currentNodeId] + 1;
                   queue.Enqueue(neighborId, distances[neighborId]);

                   // Добавляем ребро в минимальное остовное дерево
                   mstGraph.AddEdge(currentNodeId, neighborId);
               }
           }
       }

       return mstGraph;
   }
public static List<Edge> AlgorithmDijkstraPrim(WeightedGraph graph)
{
    int startNodeId = graph.Nodes[0].Id;
    // Создаем список для минимального остовного дерева
    List<Edge> minimumSpanningTree = new List<Edge>();

    // Создаем словарь для хранения расстояний до узлов
    Dictionary<int, int> distances = new Dictionary<int, int>();

    // Создаем словарь для хранения родителей узлов
    Dictionary<int, int> parents = new Dictionary<int, int>();

    // Создаем список посещенных узлов
    HashSet<int> visited = new HashSet<int>();

    // Инициализируем расстояния до всех узлов как бесконечность, кроме стартового узла
    foreach (var node in graph.Nodes)
    {
        distances[node.Id] = int.MaxValue;
        parents[node.Id] = -1;
    }
    distances[startNodeId] = 0;

    // Приоритетная очередь для хранения узлов с минимальными расстояниями
    PriorityQueue<int, int> queue = new PriorityQueue<int, int>();
    queue.Enqueue(startNodeId, 0);

    while (queue.Count > 0)
    {
        // Извлекаем узел с минимальным расстоянием
        int currentNodeId = queue.Dequeue();

        // Если узел уже посещен, пропускаем его
        if (visited.Contains(currentNodeId))
            continue;

        // Помечаем узел как посещенный
        visited.Add(currentNodeId);

        // Рассматриваем соседей текущего узла
        var currentNode = graph.Nodes.Find(n => n.Id == currentNodeId);
        foreach (var neighbor in currentNode.Neighbors)
        {
            int neighborId = neighbor.Id;
            int edgeWeight = graph.Edges.Find(e => (e.Source.Id == currentNodeId && e.Target.Id == neighborId) || (e.Source.Id == neighborId && e.Target.Id == currentNodeId)).Weight;

            // Обновляем расстояния до соседей, если они меньше текущих
            if (!visited.Contains(neighborId) && distances[neighborId] > edgeWeight)
            {
                distances[neighborId] = edgeWeight;
                parents[neighborId] = currentNodeId;
                queue.Enqueue(neighborId, distances[neighborId]);
            }
        }
    }

    // Формируем минимальное остовное дерево
    foreach (var node in graph.Nodes)
    {
        if (parents[node.Id] != -1)
        {
            minimumSpanningTree.Add(new Edge
            {
                Source = graph.Nodes.Find(n => n.Id == parents[node.Id]),
                Target = node,
                Weight = distances[node.Id]
            });
        }
    }

    return minimumSpanningTree;
}
public static void PrintAlgorithmDijkstraPrim(WeightedGraph graph)
{
    List<Edge> minimumSpanningTree = AlgorithmDijkstraPrim(graph);

    // Перемешиваем список ребер
    minimumSpanningTree = ShuffleList(minimumSpanningTree);

    Console.WriteLine("Минимальное остовное:");
    foreach (var edge in minimumSpanningTree)
    {
        Console.WriteLine($"Ребро: {edge.Source.Id} -> {edge.Target.Id}, Вес: {edge.Weight}");
    }
}

private static List<Edge> ShuffleList(List<Edge> list)
{
    // Создаем новый список, чтобы не модифицировать исходный
    List<Edge> shuffledList = new List<Edge>(list);

    // Перемешиваем список
    Random rng = new Random();
    int n = shuffledList.Count;
    while (n > 1)
    {
        n--;
        int k = rng.Next(n + 1);
        Edge value = shuffledList[k];
        shuffledList[k] = shuffledList[n];
        shuffledList[n] = value;
    }

    return shuffledList;
}

    public static List<List<Edge>> FindFundamentalCycles(WeightedGraph graph)
    {
        // Шаг 1: Построение остовного дерева графа
        List<Edge> minimumSpanningTree = AlgorithmKruskal(graph);

        // Шаг 2: Добавление ребер, не входящих в остовное дерево, к остовному дереву для получения фундаментального множества циклов
        List<List<Edge>> fundamentalCycles = new List<List<Edge>>();
        HashSet<(int, int)> treeEdges = new HashSet<(int, int)>();

        // Добавляем ребра остовного дерева в HashSet
        foreach (var edge in minimumSpanningTree)
        {
            treeEdges.Add((edge.Source.Id, edge.Target.Id));
            treeEdges.Add((edge.Target.Id, edge.Source.Id));
        }

        // Обходим все ребра графа и добавляем циклы, не входящие в остовное дерево
        foreach (var edge in graph.Edges)
        {
            if (!treeEdges.Contains((edge.Source.Id, edge.Target.Id)))
            {
                // Находим путь в остовном дереве от Source к Target
                List<Edge> cycle = FindPathInTree(minimumSpanningTree, edge.Source, edge.Target);
                cycle.Add(edge);
                fundamentalCycles.Add(cycle);
            }
        }

        return fundamentalCycles;
    }

    private static List<Edge> FindPathInTree(List<Edge> minimumSpanningTree, Node source, Node target)
    {
        List<Edge> path = new List<Edge>();
        Dictionary<int, int> parent = new Dictionary<int, int>();
        Queue<int> queue = new Queue<int>();

        // Используем BFS для нахождения пути в остовном дереве
        parent[source.Id] = -1;
        queue.Enqueue(source.Id);

        while (queue.Count > 0)
        {
            int currentNodeId = queue.Dequeue();
            if (currentNodeId == target.Id)
                break;

            foreach (var edge in minimumSpanningTree)
            {
                if (edge.Source.Id == currentNodeId)
                {
                    int neighborId = edge.Target.Id;
                    if (!parent.ContainsKey(neighborId))
                    {
                        parent[neighborId] = currentNodeId;
                        queue.Enqueue(neighborId);
                    }
                }
                else if (edge.Target.Id == currentNodeId)
                {
                    int neighborId = edge.Source.Id;
                    if (!parent.ContainsKey(neighborId))
                    {
                        parent[neighborId] = currentNodeId;
                        queue.Enqueue(neighborId);
                    }
                }
            }
        }

        // Восстанавливаем путь от Target к Source
        int node = target.Id;
        while (node != source.Id)
        {
            int parentId = parent[node];
            path.Insert(0, minimumSpanningTree.Find(e => (e.Source.Id == parentId && e.Target.Id == node) || (e.Source.Id == node && e.Target.Id == parentId)));
            node = parentId;
        }

        return path;
    }

    public static void PrintFindFundamentalCycles(WeightedGraph graph)
    {
        List<List<Edge>> fundamentalCycles = FindFundamentalCycles(graph);

        Console.WriteLine("Фундаментальное множество циклов:");
        Console.WriteLine("-------------------------------");

        int cycleIndex = 1;
        foreach (var cycle in fundamentalCycles)
        {
            Console.WriteLine($"Цикл {cycleIndex}:");
            foreach (var edge in cycle)
            {
                Console.WriteLine($"{edge.Source.Id} -> {edge.Target.Id} (вес: {edge.Weight})");
            }
            Console.WriteLine();
            cycleIndex++;
        }

        Console.WriteLine("-------------------------------");
    }


    
    public static Dictionary<Node, int> FindShortestPaths(WeightedGraph graph, Node source)
    {
        var distances = new Dictionary<Node, int>();
        var queue = new PriorityQueue<Node, int>();

        // Инициализируем расстояния
        foreach (var node in graph.Nodes)
        {
            distances[node] = int.MaxValue;
        }
        distances[source] = 0;

        // Добавляем исходную вершину в очередь
        queue.Enqueue(source, 0);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            // Обновляем расстояния для соседних вершин
            foreach (var edge in graph.Edges.Where(e => e.Source == current))
            {
                var neighbor = edge.Target;
                var distance = distances[current] + edge.Weight;

                if (distance < distances[neighbor])
                {
                    distances[neighbor] = distance;
                    queue.Enqueue(neighbor, distance);
                }
            }
        }

        return distances;
    }


    public static void PrintFindShortestPaths(WeightedGraph graph, int sourceNodeId)
    {
        // Найти узел с заданным id
        Node sourceNode = graph.Nodes.FirstOrDefault(n => n.Id == sourceNodeId);
        if (sourceNode == null)
        {
            Console.WriteLine($"Узел с id {sourceNodeId} не найден в графе.");
            return;
        }

        // Найти кратчайшие пути от исходного узла до всех остальных
        var shortestPaths = FindShortestPaths(graph, sourceNode);

        // Вывести информацию о кратчайших путях
        Console.WriteLine($"Кратчайшие пути от узла {sourceNodeId}:");
        foreach (var kvp in shortestPaths)
        {
            Console.WriteLine($"Узел {kvp.Key.Id}: расстояние {kvp.Value}");
        }
    }


}

public class Edge
{
    public Node Source { get; set; }
    public Node Target { get; set; }
    public int Weight { get; set; }
}