using System;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public Vector2 coord { get; private set; }
    
    public int x;
    public int y;
    
    public List<Node> neighbors {
        get;
    } = new List<Node>();

    private void Start() {
        coord = transform.position.XZ();
    }

    private void OnCollisionEnter(Collision other) {
        Destroy(gameObject);
    }

    public float Cost(Node other) {
        if (Math.Abs(x - other.x) == 1 && Math.Abs(y - other.y) == 1) {
            return Mathf.Sqrt(2);
        }

        return 1;
    }
}
