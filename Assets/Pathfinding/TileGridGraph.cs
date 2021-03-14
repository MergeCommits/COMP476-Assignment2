using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGridGraph : MonoBehaviour {
    public GameObject nodePrefab;

    private readonly Vector2 startPoint = new Vector2(-9.41f, -9.17f);
    private readonly Vector2 endPoint = new Vector2(32.55f, 37.22f);
    
    private const int NODE_COUNT_X = 10;
    private const int NODE_COUNT_Y = 20;
    private float TOTAL_NODE_COUNT { get { return NODE_COUNT_Y * NODE_COUNT_X;  } }

    private readonly List<Node> nodes = new List<Node>();
    
    private void Start() {
        float xLength = (endPoint.x - startPoint.x) / NODE_COUNT_X;
        float yLength = (endPoint.y - startPoint.y) / NODE_COUNT_Y;
        // Debug.Log("X: " + xLength);
        // Debug.Log("Y: " + yLength);
        for (int i = 0; i < TOTAL_NODE_COUNT; i++) {
            // ReSharper disable once PossibleLossOfFraction
            float xPosition = startPoint.x + (xLength * (i / NODE_COUNT_Y));
            float yPosition = startPoint.y + (yLength * (i % NODE_COUNT_Y));

            Vector3 position = new Vector3(xPosition, 0.5f, yPosition);
            GameObject node = Instantiate(nodePrefab, position, Quaternion.identity);
            nodes.Add(node.GetComponent<Node>());
        }
    }

    // Update is called once per frame
    void Update() {
    }
}
