namespace UnityEngine
{
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public class Object { }

    public class Transform : Object
    {
        public Vector3 position { get; set; }
    }

    public class Component : Object
    {
        public Transform transform => null!;
    }

    public class MonoBehaviour : Component { }

    public class GameObject : Object
    {
        public Transform transform => null!;
    }
}

namespace Template
{
    public class Plant : UnityEngine.MonoBehaviour { }
    public class Zombie : UnityEngine.MonoBehaviour { }

    public class Projectile : UnityEngine.MonoBehaviour
    {
        public float fX;
        public float fY;
        public float fZ;
        public UnityEngine.Vector3 previousPosition;
        public int movementTracks;
        private UnityEngine.Vector3 speed;
        private float zSpeed;
        private float zAcceleration;
        private float angularSpeed;
        private float angularAcceleration;
        public UnityEngine.GameObject shadow = null!;
        public Plant origin_Plant = null!;
        public Zombie origin_Zombie = null!;

        private void SetEulerAngles(float angular, float zAngular) { }
        private void Moving_SetNewPosition() { }

        public void Initial<T>(UnityEngine.Vector3 startPosition, UnityEngine.Vector3 speed, UnityEngine.Vector3 acceleration, float fZ, int movementTracks, T origin)
        {
            Initial(startPosition, speed, acceleration, fZ, 0f, movementTracks, 0f, 0f, 0f, origin);
        }

        public void Initial<T>(UnityEngine.Vector3 startPosition, UnityEngine.Vector3 speed, UnityEngine.Vector3 acceleration, float fZ, float zSpeed, int movementTracks, T origin)
        {
            Initial(startPosition, speed, acceleration, fZ, zSpeed, movementTracks, 0f, 0f, 0f, origin);
        }

        public void Initial<T>(UnityEngine.Vector3 startPosition, UnityEngine.Vector3 speed, UnityEngine.Vector3 acceleration, float fZ, float zSpeed, int movementTracks, float zAngular, float angularSpeed, float angularAcceleration, T origin)
        {
            fX = startPosition.x;
            fY = startPosition.y - fZ;
            this.fZ = fZ;
            this.speed = speed;
            this.zSpeed = zSpeed;
            this.movementTracks = movementTracks;
            SetEulerAngles(0f, zAngular);
            this.angularSpeed = angularSpeed;
            this.angularAcceleration = angularAcceleration;
            zAcceleration = movementTracks == 1 ? -2025f : 0f;
            Moving_SetNewPosition();
            previousPosition = new UnityEngine.Vector3(-1000000f, fY, shadow.transform.position.y - fY);

            if (origin is Plant plant)
                origin_Plant = plant;
            if (origin is Zombie zombie)
                origin_Zombie = zombie;
        }
    }
}
