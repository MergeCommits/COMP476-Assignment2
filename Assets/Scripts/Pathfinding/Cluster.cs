using System.Collections.Generic;
using UnityEngine;

public class Cluster {
    public enum ClusterType {
        Cluster1,
        Cluster2,
        Cluster3,
        Hallway1,
        Hallway2,
        Hallway3,
        Hallway4,
    }

    public List<Node> elements = new List<Node>();

    public Node GetRandomElement() {
        int index = Random.Range(0, elements.Count);
        return elements[index];
    }
}
