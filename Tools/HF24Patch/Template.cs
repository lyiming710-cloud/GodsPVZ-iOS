namespace UnityEngine
{
    public class Object
    {
        public static bool operator ==(Object? a, Object? b) => ReferenceEquals(a, b);
        public static bool operator !=(Object? a, Object? b) => !ReferenceEquals(a, b);
        public static implicit operator bool(Object? a) => a is not null;
        public override bool Equals(object? obj) => ReferenceEquals(this, obj);
        public override int GetHashCode() => base.GetHashCode();
    }
    public class Component : Object
    {
        public GameObject gameObject { get; } = null!;
        public Transform transform { get; } = null!;
    }
    public class MonoBehaviour : Component { }
    public class GameObject : Object
    {
        public Transform transform { get; } = null!;
        public T AddComponent<T>() where T : Component, new() => new T();
    }
    public class Transform : Component
    {
        public Vector3 position { get; set; }
        public Vector3 localPosition { get; set; }
        public Vector3 localScale { get; set; }
    }
    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    }
    public static class Time { public static float deltaTime => 0f; }
    public static class Debug { public static void Log(object message) { } }
}

namespace UnityEngine.Rendering
{
    public class SortingGroup : UnityEngine.Component
    {
        public string sortingLayerName { get; set; } = string.Empty;
        public int sortingOrder { get; set; }
    }
}

namespace Template
{
    public enum ProjectileType { LightSaber = 21 }
    public class Board : UnityEngine.MonoBehaviour { public bool gameStart; public bool gamePause; }
    public class Plant : UnityEngine.MonoBehaviour { }

    public class Projectile : UnityEngine.MonoBehaviour
    {
        public ProjectileType projectileType;
        public int ID;
        public float livingTime;
        public float fX, fY, fZ, fZ_shadow, fW, fD;
        public UnityEngine.Vector3 previousPosition;
        public int movementTracks;
        private UnityEngine.Vector3 speed;
        private UnityEngine.Vector3 acceleration;
        private float zSpeed;
        private float zAcceleration;
        public UnityEngine.Vector3 targetPosition;
        public UnityEngine.GameObject projectileSprite = null!;
        public UnityEngine.GameObject shadow = null!;
        private UnityEngine.GameObject track = null!;
        public Board board = null!;
        public UnityEngine.Rendering.SortingGroup sortingGroup = null!;
        public Plant origin_Plant = null!;

        private void Update_Time() { }
        private void Update_Tracking() { }
        private void Update_MoveTrack7() { }
        private bool TestOutofMap() => false;
        private void SetEulerAngles(float angular, float zAngular) { }
        public void Moving_SetNewPosition() { }
        public void DestroyProjectile() { }
        private void CollisionDetect() { }
        private void Rotating() { }

        private void Update()
        {
            if (sortingGroup == null)
                sortingGroup = gameObject.AddComponent<UnityEngine.Rendering.SortingGroup>();

            sortingGroup.sortingLayerName = "Entity";
            sortingGroup.sortingOrder = -10 - (int)(fY * 10f);

            if (board == null)
                return;

            if (board.gameStart && !board.gamePause)
            {
                Update_Time();
                switch (movementTracks)
                {
                    case 3:
                        Update_Tracking();
                        break;
                    case 4:
                        if (livingTime > 1.5f)
                        {
                            zSpeed = 0f;
                            speed.x = 1500f;
                            Update_Tracking();
                            movementTracks = 0;
                        }
                        else
                        {
                            float a = (float)Math.Log(10.0);
                            UnityEngine.Debug.Log("a=" + a.ToString());
                            float d = 95f - fZ;
                            UnityEngine.Debug.Log("d=" + d.ToString());
                            zSpeed = d * 6f / (livingTime * 6f * a + a);
                            Update_Tracking();
                        }
                        break;
                    case 6:
                        zSpeed = (fZ - 75f) * -5.2f;
                        break;
                    case 7:
                        Update_MoveTrack7();
                        break;
                }

                if (movementTracks == 5 && (fZ >= 1200f || TestOutofMap()))
                {
                    SetEulerAngles(0f, -90f);
                    speed = new UnityEngine.Vector3(0f, 0f, 0f);
                    zSpeed = -3000f;
                    zAcceleration = 0f;
                    fX = targetPosition.x;
                    fY = targetPosition.y;
                    fZ = 1190f;
                }

                float dt = UnityEngine.Time.deltaTime;
                fX += dt * speed.x;
                dt = UnityEngine.Time.deltaTime;
                fY += dt * speed.y;
                dt = UnityEngine.Time.deltaTime;
                speed.x += dt * acceleration.x;
                speed.y += dt * acceleration.y;
                speed.z += dt * acceleration.z;
                dt = UnityEngine.Time.deltaTime;
                fZ += dt * zSpeed;
                dt = UnityEngine.Time.deltaTime;
                zSpeed += dt * zAcceleration;

                Moving_SetNewPosition();

                shadow.transform.localScale = new UnityEngine.Vector3(fW / 40f * 1.425f, fD / 40f * 1.425f, 0f);
                if (shadow)
                {
                    UnityEngine.Vector3 p = shadow.transform.localPosition;
                    p.y = fZ_shadow;
                    shadow.transform.localPosition = p;
                }

                if (TestOutofMap())
                    DestroyProjectile();
                CollisionDetect();
            }

            if (track && projectileSprite)
                track.transform.position = projectileSprite.transform.position;

            Rotating();
            if (projectileType == ProjectileType.LightSaber && origin_Plant == null)
                DestroyProjectile();

            previousPosition.x = fX;
            previousPosition.y = fY;
            previousPosition.z = shadow.transform.position.y - fY;
        }
    }
}
