namespace UnityEngine
{
    public struct Vector3
    {
        public float x; public float y; public float z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    }
    public enum Space { World, Self }
    public class Object
    {
        public static implicit operator bool(Object? o) => o is not null;
    }
    public class Transform : Object
    {
        public Vector3 localEulerAngles { get; set; }
        public void Rotate(Vector3 axis, float angle, Space relativeTo) { }
    }
    public class Component : Object { public Transform transform { get; } = null!; }
    public class GameObject : Object { public Transform transform { get; } = null!; }
    public class MonoBehaviour : Component { }
    public static class Time { public static float deltaTime => 0f; }
}

namespace Template
{
    public class Projectile : UnityEngine.MonoBehaviour
    {
        public float speed;
        public float angularSpeed;
        public float angularAcceleration;
        public UnityEngine.GameObject projectileSprite = null!;
        public UnityEngine.GameObject projectileAnimation = null!;

        private void Rotating()
        {
            bool reverse = speed < 0f || float.IsNaN(speed);
            transform.localEulerAngles = new UnityEngine.Vector3(
                transform.localEulerAngles.x,
                reverse ? 180f : 0f,
                transform.localEulerAngles.z);

            if (angularAcceleration != 0f)
                angularSpeed += UnityEngine.Time.deltaTime * angularAcceleration;

            if (angularSpeed != 0f)
            {
                if (projectileSprite)
                    projectileSprite.transform.Rotate(new UnityEngine.Vector3(0f, 0f, 1f), UnityEngine.Time.deltaTime * angularSpeed, UnityEngine.Space.World);
                if (projectileAnimation)
                    projectileAnimation.transform.Rotate(new UnityEngine.Vector3(0f, 0f, 1f), UnityEngine.Time.deltaTime * angularSpeed, UnityEngine.Space.World);
            }
        }
    }
}
