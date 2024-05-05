// See https://aka.ms/new-console-template for more information
using Saod6Lab.GraphInput;
using System;
using System.Runtime.Intrinsics.Arm;
using System.Threading;

for (;;)
{
    Console.Write("Введите количество узлов: ");
    int numberOfNodes = int.Parse(Console.ReadLine());

    Console.Write("Введите количество ребер: ");
    int numberOfEdges = int.Parse(Console.ReadLine());

    if (numberOfEdges < (numberOfNodes - 1))
    {
        Console.WriteLine("Количетсов ребер должно быть больше, минимум n-1");
    }
    else if (numberOfEdges > (numberOfNodes * (numberOfNodes - 1) / 2))
    {
        Console.WriteLine("Количетсво ребер должно быть меньше чем n *(n-1)/2");
    }
    else
    {
        Graph graph = Graph.GenerateRandomGraph(numberOfNodes, numberOfEdges);
       
        WeightedGraph weightedGraph = new WeightedGraph(graph);
        Equeals equeals = new();
        var s1 = WeightedGraph.AlgorithmKruskal(weightedGraph);
        var s2 = WeightedGraph.AlgorithmDijkstraPrim(weightedGraph);
       
        Console.WriteLine("Созданный граф:");
        Graph.PrintGraphAsText(graph);
        Graph.PrintIncidenceMatrix(graph);
        Graph.PrintEdgeList(graph);
        WeightedGraph.PrintEdges(weightedGraph);
        WeightedGraph.PrintAlgorithmDijkstraPrim(weightedGraph);
        WeightedGraph.PrintAlgorithmDijkstraPrim(weightedGraph);
        WeightedGraph.PrintFindFundamentalCycles(weightedGraph);
        WeightedGraph.PrintFindShortestPaths(weightedGraph, 1);
        // Создание и запуск потока, который отслеживает нажатие 'q'
        Thread thread = new Thread(() =>
        {
            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    return;
                }
            }
        });
        thread.Start();
    }
}