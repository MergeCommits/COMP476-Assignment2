public class Dijkstra : AStar {
    protected override float Heuristic(Node a, Node b) {
        return 0f;
    }
}
