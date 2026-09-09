namespace UnityEngine
{
    public struct Vector3
    {
        public float x; public float y; public float z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public float magnitude => 0f;
        public void Normalize() { }
        public static Vector3 operator *(Vector3 a, float d) => default;
    }

    public class Object
    {
        public static implicit operator bool(Object? o) => o is not null;
    }

    public class Transform : Object
    {
        public Vector3 localEulerAngles { get; set; }
    }

    public class GameObject : Object
    {
        public Transform transform => null!;
    }

    public class Component : Object { }
    public class MonoBehaviour : Component { }

    public static class Mathf
    {
        public static float Atan2(float y, float x) => 0f;
        public static float Abs(float f) => 0f;
        public static bool Approximately(float a, float b) => false;
        public static float Lerp(float a, float b, float t) => 0f;
    }
}

namespace Template
{
    public class Projectile : UnityEngine.MonoBehaviour
    {
        private UnityEngine.Vector3 speed;
        private float angular;
        private float zAngular;
        public UnityEngine.GameObject projectileSprite = null!;
        public UnityEngine.GameObject projectileAnimation = null!;
        public UnityEngine.GameObject shadow = null!;
        private UnityEngine.GameObject track = null!;

        public void Aim(UnityEngine.Vector3 distant)
        {
            float angle = UnityEngine.Mathf.Atan2(distant.y, distant.x) * 57.29578f;
            if (speed.x < 0f)
                angle = 180f - angle;

            distant.Normalize();
            speed = distant * speed.magnitude;
            SetEulerAngles(angle, 0f);
        }

        private void SetEulerAngles(float angular, float zAngular)
        {
            this.angular = angular;
            this.zAngular = zAngular;

            if (angular != 0f)
            {
                if (zAngular == 0f || UnityEngine.Mathf.Approximately(angular, zAngular))
                {
                    zAngular = angular;
                }
                else
                {
                    float selected = UnityEngine.Mathf.Abs(zAngular) > UnityEngine.Mathf.Abs(angular) ? angular : zAngular;
                    zAngular = UnityEngine.Mathf.Lerp(angular, zAngular, selected / (angular + zAngular));
                }
            }

            if (projectileSprite)
                projectileSprite.transform.localEulerAngles = new UnityEngine.Vector3(projectileSprite.transform.localEulerAngles.x, projectileSprite.transform.localEulerAngles.y, zAngular);

            if (projectileAnimation)
                projectileAnimation.transform.localEulerAngles = new UnityEngine.Vector3(projectileAnimation.transform.localEulerAngles.x, projectileAnimation.transform.localEulerAngles.y, zAngular);

            if (shadow)
                shadow.transform.localEulerAngles = new UnityEngine.Vector3(shadow.transform.localEulerAngles.x, shadow.transform.localEulerAngles.y, angular);

            if (track)
                track.transform.localEulerAngles = new UnityEngine.Vector3(projectileSprite.transform.localEulerAngles.x, projectileSprite.transform.localEulerAngles.y, zAngular);
        }
    }
}
