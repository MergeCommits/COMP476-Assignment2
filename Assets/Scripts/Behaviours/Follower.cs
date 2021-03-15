using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[SelectionBase]
public class Follower : MonoBehaviour {
    public bool pov = true;
    public POVGraph povGraph;
    public TileGridGraph tileGridGraph;
    
    public Node start;
    public Node end;
    
    public GameObject target;

    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;
    public float orientation;
    public float rotation;
    public float angularAcceleration;
    [NonSerialized] public float maxVelocity = 4f;
    [NonSerialized] public float maxRotation = 4f;
    [NonSerialized] public float maxAcceleration = 35f;
    [NonSerialized] public float maxAngularAcceleration = 20f;

    private Steering currentBehavior = new Steering();
    public Follower followerTarget { private set; get; }
    public bool hasFollowerScript { private set; get; } = false;

    public enum State {
        ToGoal,
        Frozen
    }

    [NonSerialized]
    public State currentState = State.ToGoal;

    private List<Node> path = new List<Node>();

    private bool computedPath = false;

    private void Start() {
        Transform transform1 = transform;

        position = transform1.position.XZ();
        orientation = transform1.rotation.eulerAngles.y * Mathf.Deg2Rad;
        if (target != null) {
            followerTarget = target.gameObject.GetComponent<Follower>();
            hasFollowerScript = followerTarget != null;
        }
    }

    void FixedUpdate() {
        switch (currentState) {
            case State.ToGoal:
                if (!computedPath) {
                    if (pov) {
                        if (!povGraph.ranChecks) {
                            break;
                        }
                        path = povGraph.GenerateGridAndPath(new ClusterHeuristic(), start, end);
                        target = path[0].gameObject;
                    } else {
                        if (!tileGridGraph.ranChecks) {
                            break;
                        }
                        path = tileGridGraph.GenerateGridAndPath(new ClusterHeuristic(), tileGridGraph.nodes[0,0], tileGridGraph.nodes[7,19]);
                        target = path[0].gameObject;
                    }

                    computedPath = true;
                }
                
                if (Vector2.Distance(position, target.transform.position.XZ()) < 0.8f) {
                    if (target == path.Last().gameObject) {
                        currentState = State.Frozen;
                        break;
                    }
                    
                    target = path.SkipWhile(p => p.gameObject != target)
                        .ElementAt(1).gameObject;
                }
                
                currentBehavior.UpdateGoal(this);
                break;
            case State.Frozen:
                velocity = Vector2.zero;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
