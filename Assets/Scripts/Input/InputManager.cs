using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    private static InputManager singleton;
    
    private Gamepad gamepad;
    public static MovementInput movement;
    public static ButtonInput pauseKey;

    private void Awake() {
        gamepad = Gamepad.current;

        if (singleton == null) {
            singleton = this;
        }
    }

    private void OnEnable() {
        if (movement != null) { return; }

        movement = new MovementInput(Keyboard.current.wKey, Keyboard.current.sKey,
            Keyboard.current.aKey, Keyboard.current.dKey, gamepad);
        pauseKey = new ButtonInput(Keyboard.current.escapeKey, gamepad?.startButton);
    }

    private void Update() {
        movement.Update();
        pauseKey.Update();
    }

    private void FixedUpdate() {
        movement.FixedUpdate();
        pauseKey.FixedUpdate();
    }

    /// <summary>
    /// Forces a FixedUpdate run of the inputs. Used for running inputs while game is paused.
    /// </summary>
    public static void ForceFixedUpdate() {
        singleton.FixedUpdate();
    }
}
