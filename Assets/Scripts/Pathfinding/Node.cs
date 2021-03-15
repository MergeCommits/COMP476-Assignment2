using System;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public Cluster.ClusterType clusterType;
    public Cluster cluster;

    public Vector2 coord { get; private set; }

    public int x;
    public int y;

    public List<Node> neighbors {
        get;
        private set;
    }

    private void Awake() {
        clusterType = Cluster.ClusterType.Hallway1;
        coord = transform.position.XZ();
        neighbors = new List<Node>();
    }

    private void OnCollisionEnter(Collision other) {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        switch (other.name) {
            case "Cluster1":
                clusterType = Cluster.ClusterType.Cluster1;
                break;
            case "Cluster2":
                clusterType = Cluster.ClusterType.Cluster2;
                break;
            case "Cluster3":
                clusterType = Cluster.ClusterType.Cluster3;
                break;
            case "Hallway1":
                clusterType = Cluster.ClusterType.Hallway1;
                break;
            case "Hallway2":
                clusterType = Cluster.ClusterType.Hallway2;
                break;
            case "Hallway3":
                clusterType = Cluster.ClusterType.Hallway3;
                break;
            case "Hallway4":
                clusterType = Cluster.ClusterType.Hallway4;
                break;
        }
    }

    public float Cost(Node other) {
        if (Math.Abs(x - other.x) == 1 && Math.Abs(y - other.y) == 1) {
            return Mathf.Sqrt(2);
        }

        return 1;
    }
}
