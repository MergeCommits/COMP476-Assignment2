using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MovementInput {
    private readonly ButtonInput upKey;
    private readonly ButtonInput leftKey;
    private readonly ButtonInput downKey;
    private readonly ButtonInput rightKey;
    private readonly StickInput leftStick;

    public Vector2 axisDelta { get; private set; }

    public MovementInput(ButtonControl up, ButtonControl down, ButtonControl left, ButtonControl right,
        Gamepad gamepad = null) {
        upKey = new ButtonInput(up);
        downKey = new ButtonInput(down);
        leftKey = new ButtonInput(left);
        rightKey = new ButtonInput(right);

        if (gamepad != null) {
            leftStick = new StickInput(gamepad.leftStick);
        } else {
            leftStick = null;
        }

        axisDelta = Vector2.zero;
    }

    public void Update() {
        upKey.Update();
        downKey.Update();
        leftKey.Update();
        rightKey.Update();
    }

    public void FixedUpdate() {
        upKey.FixedUpdate();
        downKey.FixedUpdate();
        leftKey.FixedUpdate();
        rightKey.FixedUpdate();
        UpdateAxis();
    }

    private void UpdateAxis() {
        float xAxis = 0;
        float yAxis = 0;

        if (leftKey.isDown) {
            xAxis--;
        }

        if (rightKey.isDown) {
            xAxis++;
        }

        if (upKey.isDown) {
            yAxis++;
        }

        if (downKey.isDown) {
            yAxis--;
        }

        if (leftStick != null) {
            leftStick.FixedUpdate();
            Vector2 stickPos = leftStick.stickPosition;

            xAxis = Mathf.Clamp(stickPos.x + xAxis, -1f, 1f);
            yAxis = Mathf.Clamp(stickPos.y + yAxis, -1f, 1f);
        }

        axisDelta = new Vector2(xAxis, yAxis);
    }
}
