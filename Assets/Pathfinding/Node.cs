using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public Vector2 coord { get; private set; }
    
    public List<Node> neighbors {
        get;
    } = new List<Node>();

    private void Start() {
        coord = transform.position.XZ();
    }

    private void OnCollisionEnter(Collision other) {
        Destroy(gameObject);
    }
}
