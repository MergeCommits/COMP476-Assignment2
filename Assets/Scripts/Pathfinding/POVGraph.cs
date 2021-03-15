using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class POVGraph : MonoBehaviour {
    public GameObject nodePrefab;

    private Node[] nodes;

    private Cluster cluster1 = new Cluster();
    private Cluster cluster2 = new Cluster();
    private Cluster cluster3 = new Cluster();
    private Cluster hallway1 = new Cluster();
    private Cluster hallway2 = new Cluster();
    private Cluster hallway3 = new Cluster();
    private Cluster hallway4 = new Cluster();

    private bool ranChecks;

    private void Start() {
        GameObject collectionOfNodes = GameObject.Find("PoV Nodes");
        nodes = collectionOfNodes.GetComponentsInChildren<Node>();
    }

    private void Update() {
        if (ranChecks) { return; }

        foreach (Node node in nodes) {
            switch (node.clusterType) {
                case Cluster.ClusterType.Cluster1:
                    node.cluster = cluster1;
                    cluster1.elements.Add(node);
                    break;
                case Cluster.ClusterType.Cluster2:
                    node.cluster = cluster2;
                    cluster2.elements.Add(node);
                    break;
                case Cluster.ClusterType.Cluster3:
                    node.cluster = cluster3;
                    cluster3.elements.Add(node);
                    break;
                case Cluster.ClusterType.Hallway1:
                    node.cluster = hallway1;
                    hallway1.elements.Add(node);
                    break;
                case Cluster.ClusterType.Hallway2:
                    node.cluster = hallway2;
                    hallway2.elements.Add(node);
                    break;
                case Cluster.ClusterType.Hallway3:
                    node.cluster = hallway3;
                    hallway3.elements.Add(node);
                    break;
                case Cluster.ClusterType.Hallway4:
                    node.cluster = hallway4;
                    hallway4.elements.Add(node);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            FindAllConnectedNeighbors(node);
        }

        GenerateGridAndPath(new ClusterHeuristic());
            
        ranChecks = true;
    }

    private void GenerateGridAndPath(AStar pathfindingAlgorithm) {
        pathfindingAlgorithm.ComputePath(nodes[0], nodes[8]);
        List<Node> path = pathfindingAlgorithm.GetFoundPath();
        for (int i = 0; i < path.Count; i++) {
            if (i == (path.Count - 1)) { break; }
            Debug.DrawLine(path[i].coord.toXZ(0.5f), path[i + 1].coord.toXZ(0.5f), Color.red, 100f);
        }
    }

    private void FindAllConnectedNeighbors(Node node) {
        Node[] possibleConnections = nodes.Where(n => n != node).ToArray();

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
