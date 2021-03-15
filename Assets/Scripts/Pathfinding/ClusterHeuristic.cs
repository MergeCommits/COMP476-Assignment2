public class ClusterHeuristic : AStar {
    protected override float Heuristic(Node a, Node b) {
        if (a.cluster == b.cluster) { return 0f; }

        Node randomA = a.cluster.GetRandomElement();
        Node randomB = a.cluster.GetRandomElement();
        
        return base.Heuristic(randomA, randomB);
    }
}
