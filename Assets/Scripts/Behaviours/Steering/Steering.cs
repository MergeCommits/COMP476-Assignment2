using UnityEngine;

public class Steering {
    protected const float TIME_TO_TARGET = 1f;
    protected const float SATISFACTION_RADIUS = 0.5f;
    protected const float SLOW_RADIUS = 4f;

    #region Acceleration

    public SteeringOutput PerformArrive(SteeringInput input) {
        Vector2 direction = input.targetPosition - input.position;
        float distance = direction.magnitude;

        if (distance < SATISFACTION_RADIUS) {
            return new SteeringOutput();
        }

        float targetSpeed = distance > SLOW_RADIUS
            ? input.maxVelocity
            : input.maxVelocity * distance / SLOW_RADIUS;

        Vector2 targetVelocity = direction;
        targetVelocity = targetVelocity.normalized;
        targetVelocity *= targetSpeed;

        Vector2 acceleration = (targetVelocity - input.velocity) / TIME_TO_TARGET;
        return new SteeringOutput {acceleration = acceleration, angularAcceleration = 0f};
    }

    #endregion Acceleration

    #region Angular Acceleration

    private float MapToRange(float rotation) {
        if (rotation > Mathf.PI) {
            return rotation - (2 * Mathf.PI);
        }

        if (rotation < -Mathf.PI) {
            return rotation + (2 * Mathf.PI);
        }

        return rotation;
    }

    private const float SATISFACTION_ORIENTATION = 5f * Mathf.Deg2Rad;
    public float PerformAlign(SteeringInput input) {
        float rotation = input.targetOrientation - input.orientation;
        rotation = MapToRange(rotation);
        float rotationSize = Mathf.Abs(rotation);

        if (rotationSize < SATISFACTION_ORIENTATION) {
            return 0f;
        }

        float targetRotation;
        if (rotationSize > SLOW_RADIUS) {
            targetRotation = input.maxRotation;
        } else {
            targetRotation = input.maxRotation * rotationSize / SLOW_RADIUS;
        }

        targetRotation *= rotation / rotationSize;

        float angularAcceleration = targetRotation - input.rotation;
        angularAcceleration /= TIME_TO_TARGET;

        float absAngular = Mathf.Abs(angularAcceleration);
        if (absAngular > input.maxAngularAcceleration) {
            angularAcceleration /= absAngular;
            angularAcceleration *= input.maxAngularAcceleration;
        }

        return angularAcceleration;
    }

    public float PerformLookWhereYouGoing(SteeringInput input) {
        if (Mathf.Approximately(input.velocity.sqrMagnitude, 0f)) {
            return 0f;
        }

        input.targetOrientation = Mathf.Atan2(input.velocity.y, input.velocity.x);
        return PerformAlign(input);
    }

    #endregion Angular Acceleration
    
    private SteeringInput BuildInput(Follower follower) {
        Vector2 targetPosition = follower.hasFollowerScript
            ? follower.followerTarget.position
            : Vector2.zero;
        if (!follower.hasFollowerScript && follower.target != null) {
            targetPosition = follower.target.transform.position.XZ();
        }
        
        Vector2 targetVelocity = follower.hasFollowerScript
            ? follower.followerTarget.velocity
            : Vector2.zero;
        
        float targetOrientation = follower.hasFollowerScript
            ? follower.followerTarget.orientation
            : 0f;
        
            
        SteeringInput followerInput = new SteeringInput {
            position = follower.position,
            velocity = follower.velocity,
            orientation = follower.orientation,
            rotation = follower.rotation,
            maxVelocity = follower.maxVelocity,
            maxRotation = follower.maxRotation,
            maxAcceleration = follower.maxAcceleration,
            maxAngularAcceleration = follower.maxAngularAcceleration,
            targetPosition = targetPosition,
            targetVelocity = targetVelocity,
            targetOrientation = targetOrientation
        };

        return followerInput;
    }

    private void ApplyOutput(Follower follower, SteeringOutput steeringOutput) {
        follower.position += (follower.velocity * Time.deltaTime) + (follower.acceleration * (0.5f * Time.deltaTime * Time.deltaTime));
        follower.orientation += (follower.rotation * Time.deltaTime) +
                                (follower.angularAcceleration * (0.5f * Time.deltaTime * Time.deltaTime));

        Transform transform1 = follower.transform;
        transform1.position = new Vector3(follower.position.x, transform1.position.y, follower.position.y);
        transform1.eulerAngles = new Vector3(0f, -follower.orientation * Mathf.Rad2Deg, 0f);

        follower.velocity += follower.acceleration * Time.deltaTime;
        if (follower.velocity.magnitude > follower.maxVelocity) {
            follower.velocity = follower.velocity.normalized;
            follower.velocity *= follower.maxVelocity;
        }
        
        follower.acceleration = steeringOutput.acceleration;
        if (follower.acceleration.magnitude > follower.maxAcceleration) {
            follower.acceleration = follower.acceleration.normalized;
            follower.acceleration *= follower.maxAcceleration;
        }
        
        follower.rotation += follower.angularAcceleration * Time.deltaTime;
        follower.rotation = Mathf.Clamp(follower.rotation, -follower.maxRotation, follower.maxRotation);

        follower.angularAcceleration = Mathf.Clamp(steeringOutput.angularAcceleration, -follower.maxAngularAcceleration, follower.maxAngularAcceleration);
    }

    public void UpdateGoal(Follower follower) {
        SteeringInput input = BuildInput(follower);

        SteeringOutput output = PerformArrive(input);
        output.angularAcceleration = PerformLookWhereYouGoing(input);
        
        ApplyOutput(follower, output);
    }
}
