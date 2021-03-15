using System.Collections.Generic;
using UnityEngine;

public class TileGridGraph : MonoBehaviour {
    public GameObject nodePrefab;

    private readonly Vector2 startPoint = new Vector2(-9.41f, -9.17f);
    private readonly Vector2 endPoint = new Vector2(35f, 37.22f);

    private const int NODE_COUNT_X = 10;
    private const int NODE_COUNT_Y = 20;
    private float TOTAL_NODE_COUNT { get { return NODE_COUNT_Y * NODE_COUNT_X; } }

    private readonly Node[,] nodes = new Node[NODE_COUNT_X, NODE_COUNT_Y];

    private Node GetNodeWithBoundsCheck(int x, int y) {
        if (x < 0 || x >= NODE_COUNT_X) {
            return null;
        }

        if (y < 0 || y >= NODE_COUNT_Y) {
            return null;
        }

        return nodes[x, y];
    }

    private bool ranChecks;

    private void Start() {
        float xLength = (endPoint.x - startPoint.x) / NODE_COUNT_X;
        float yLength = (endPoint.y - startPoint.y) / NODE_COUNT_Y;
        
        for (int i = 0; i < TOTAL_NODE_COUNT; i++) {
            int x = i / NODE_COUNT_Y;
            int y = i % NODE_COUNT_Y;

            float xPosition = startPoint.x + (xLength * x);
            float yPosition = startPoint.y + (yLength * y);

            Vector3 position = new Vector3(xPosition, 0.5f, yPosition);
            GameObject node = Instantiate(nodePrefab, position, Quaternion.identity);
            nodes[x, y] = node.GetComponent<Node>();
            nodes[x, y].x = x;
            nodes[x, y].y = y;
        }
    }

    private void Update() {
        if (ranChecks) { return; }

        for (int x = 0; x < NODE_COUNT_X; x++) {
            for (int y = 0; y < NODE_COUNT_Y; y++) {
                FindAllConnectedNeighbors(x, y);
            }
        }

        AStar aStar = new AStar();
        aStar.ComputePath(nodes[0,0], nodes[9,19]);
        List<Node> path = aStar.GetFoundPath();
        for (int i = 0; i < path.Count; i++) {
            if (i == (path.Count - 1)) { break; }
            Debug.DrawLine(path[i].coord.toXZ(0.5f), path[i + 1].coord.toXZ(0.5f), Color.red, 100f);
        }
            
        ranChecks = true;
    }

    private void FindAllConnectedNeighbors(int x, int y) {
        Node[] possibleConnections = {
            GetNodeWithBoundsCheck(x - 1, y - 1), GetNodeWithBoundsCheck(x, y - 1),
            GetNodeWithBoundsCheck(x + 1, y - 1), GetNodeWithBoundsCheck(x - 1, y), GetNodeWithBoundsCheck(x + 1, y),
            GetNodeWithBoundsCheck(x - 1, y + 1), GetNodeWithBoundsCheck(x, y + 1), GetNodeWithBoundsCheck(x + 1, y + 1)
        };

        Node node = GetNodeWithBoundsCheck(x, y);
        foreach (Node connection in possibleConnections) {
            if (connection == null) {
                continue;
            }

            if (!Physics.Linecast(node.coord.toXZ(0.5f), connection.coord.toXZ(0.5f))) {
                Debug.DrawLine(node.coord.toXZ(0.5f), connection.coord.toXZ(0.5f), Color.magenta, 100f);
                node.neighbors.Add(connection);
            }
        }
    }
}
