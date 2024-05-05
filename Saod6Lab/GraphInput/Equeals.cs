namespace Saod6Lab.GraphInput;

public class Equeals
{
    private bool AreEdgesEqual(Edge edge1, Edge edge2)
    {
        return (edge1.Source == edge2.Source && edge1.Target == edge2.Target) ||
               (edge1.Source == edge2.Target && edge1.Target == edge2.Source);
    }
    public bool AreListsEqual(List<Edge> list1, List<Edge> list2)
    {
        if (list1.Count != list2.Count)
            return false;

        foreach (var edge1 in list1)
        {
            bool found = false;
            foreach (var edge2 in list2)
            {
                if (AreEdgesEqual(edge1, edge2))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
                return false;
        }

        return true;
    }

}