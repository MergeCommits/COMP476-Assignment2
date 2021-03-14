using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour {
    void FixedUpdate() {
        float speed = 1.5f;
        Vector2 direction = new Vector2(InputManager.movement.axisDelta.x, InputManager.movement.axisDelta.y);
        direction = Vector2.ClampMagnitude(direction, 1.0f);

        Vector2 displacement = direction * (speed * Time.deltaTime);
        transform.Translate(displacement.x, 0f, displacement.y);
    }
}
