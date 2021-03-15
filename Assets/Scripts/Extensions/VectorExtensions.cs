using UnityEngine;

public static class VectorExtensions {
    public static Vector2 XZ(this Vector3 src) {
        return new Vector2(src.x, src.z);
    }
    
    public static Vector3 toXZ(this Vector2 src, float y  = 0f) {
        return new Vector3(src.x, y, src.y);
    }
}
