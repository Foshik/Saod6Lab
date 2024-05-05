namespace Saod6Lab.GraphInput;

public class Node
{
    public int Id { get; set; }
    public List<Node> Neighbors { get; set; } = new List<Node>();
}
