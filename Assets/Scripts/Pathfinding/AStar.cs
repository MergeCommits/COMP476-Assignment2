using System.Collections.Generic;
using UnityEngine;

public class AStar {
    public Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
    public Dictionary<Node, float> costSoFar = new Dictionary<Node, float>();

    private Node start;
    private Node goal;

    protected virtual float Heuristic(Node a, Node b) {
        return Mathf.Abs(a.coord.x - b.coord.x) + Mathf.Abs(a.coord.y - b.coord.y);
    }

    public void ComputePath(Node start, Node goal) {
        this.start = start;
        this.goal = goal;

        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(start, 0f);

        cameFrom.Add(start, start);
        costSoFar.Add(start, 0f);

        while (frontier.Count > 0f) {
            Node current = frontier.Dequeue();

            if (current.Equals(goal)) { break; }

            
            foreach (Node neighbor in current.neighbors) {
                Debug.DrawLine(current.coord.toXZ(0.5f), neighbor.coord.toXZ(0.5f), Color.yellow, 100f);
                float newCost = costSoFar[current] + current.Cost(neighbor);

                // Add new cost if node hasn't been traversed or is smaller than a previous trip to that node.
                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor]) {
                    if (costSoFar.ContainsKey(neighbor)) {
                        costSoFar.Remove(neighbor);
                        cameFrom.Remove(neighbor);
                    }

                    costSoFar.Add(neighbor, newCost);
                    cameFrom.Add(neighbor, current);
                    float priority = newCost + Heuristic(neighbor, goal);
                    frontier.Enqueue(neighbor, priority);
                }
            }
        }
    }

    public List<Node> GetFoundPath() {
        List<Node> path = new List<Node>();
        Node current = goal;

        while (!current.Equals(start)) {
            if (!cameFrom.ContainsKey(current)) {
                return new List<Node>();
            }

            path.Add(current);
            current = cameFrom[current];
        }

        path.Add(start);
        path.Reverse();
        return path;
    }
}
