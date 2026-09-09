namespace UnityEngine
{
    public struct Vector3
    {
        public float x; public float y; public float z;
        public Vector3(float x, float y, float z) { this.x=x; this.y=y; this.z=z; }
    }

    public struct Quaternion
    {
        public static Quaternion Euler(float x, float y, float z) => default;
        public static Vector3 operator *(Quaternion rotation, Vector3 point) => default;
    }

    public class Object
    {
        public static bool operator ==(Object? a, Object? b) => ReferenceEquals(a,b);
        public static bool operator !=(Object? a, Object? b) => !ReferenceEquals(a,b);
        public static implicit operator bool(Object? o) => o is not null;
        public static void Destroy(Object obj, float t) { }
        public override bool Equals(object? obj) => ReferenceEquals(this,obj);
        public override int GetHashCode() => base.GetHashCode();
    }

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
        public GameObject(string name) { }
        public Transform transform => null!;
        public T AddComponent<T>() where T : Component => null!;
    }

    public class AudioClip : Object
    {
        public float length => 0f;
    }

    public class AudioSource : Component
    {
        public AudioClip clip { get; set; } = null!;
        public float volume { get; set; }
        public float pitch { get; set; }
        public float spatialBlend { get; set; }
        public void Play() { }
    }

    public static class Time
    {
        public static float deltaTime => 0f;
        public static float timeScale => 0f;
    }
}

namespace Template
{
    public class Damage
    {
        public bool AreaDamage() => false;
    }

    public class Plant : UnityEngine.MonoBehaviour
    {
        public float LivingTime() => 0f;
        public Damage GetDamage(Projectile projectile, int id, int level) => null!;
    }

    public class Projectile : UnityEngine.MonoBehaviour
    {
        public int ID;
        public Damage damage = null!;
        public float livingTime;
        public float updateRate;
        public float fX;
        public float fY;
        public float fZ;
        public int index;
        public float maxLivingTime;
        public float pc_21_timing;
        public UnityEngine.Vector3 speed;
        public float zSpeed;
        public Plant origin_Plant = null!;

        private void Collision_AudioParticle() { }
        private void DestroyProjectile() { }
        private void PC_LightSaber_Explosion() { }

        private void Update_Time()
        {
            float step = UnityEngine.Time.deltaTime * updateRate;
            livingTime += step;

            if (ID == 27 && livingTime > 3f)
            {
                damage.AreaDamage();
                Collision_AudioParticle();
                DestroyProjectile();
            }

            if (ID == 28 && livingTime > 15f)
            {
                damage.AreaDamage();
                Collision_AudioParticle();
                DestroyProjectile();
            }

            if (ID == 21)
            {
                pc_21_timing += step;
                if (pc_21_timing >= 1f && origin_Plant != null)
                {
                    origin_Plant.GetDamage(this, 21, 0).AreaDamage();
                    pc_21_timing = 0f;
                }
                if (livingTime > maxLivingTime)
                    PC_LightSaber_Explosion();
            }
        }

        private void Update_MoveTrack7()
        {
            zSpeed = (fZ - 75f) * -5.2f;
            if (origin_Plant != null)
            {
                UnityEngine.Vector3 plantPosition = origin_Plant.transform.position;
                float angle = index * 51.43f + origin_Plant.LivingTime() * 120f;
                UnityEngine.Vector3 offset = UnityEngine.Quaternion.Euler(0f, 0f, angle) * new UnityEngine.Vector3(300f, 0f, 0f);
                speed = new UnityEngine.Vector3(
                    (plantPosition.x + offset.x - fX) * 2.2f,
                    (plantPosition.y + offset.y * 0.75f - fY) * 2.2f,
                    0f);
            }
        }
    }

    public class Zombie : UnityEngine.MonoBehaviour
    {
        public float fX;
        public float fY;
        public float fZ;
        public UnityEngine.Vector3 rSpeed;

        public UnityEngine.Vector3 GetPredictedPosition(float time)
        {
            UnityEngine.Vector3 position = new UnityEngine.Vector3(fX, fY, fZ);
            float t = time + UnityEngine.Time.deltaTime;
            position.x += t * rSpeed.x;
            position.y += t * rSpeed.y;
            return position;
        }
    }

    public static class GlobalStaticVars
    {
        public static UnityEngine.AudioSource CreateAudioAtPoint(UnityEngine.AudioClip clip, UnityEngine.Vector3 position, float volume, float pitch)
        {
            if (clip == null)
                return null!;

            UnityEngine.GameObject go = new UnityEngine.GameObject("One shot audio");
            if ((object)go == null)
                throw new System.NullReferenceException();

            UnityEngine.Transform transform = go.transform;
            if ((object)transform == null)
                throw new System.NullReferenceException();
            transform.position = position;

            UnityEngine.AudioSource source = go.AddComponent<UnityEngine.AudioSource>();
            if ((object)source == null)
                throw new System.NullReferenceException();
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.spatialBlend = 0f;
            source.Play();

            float scale = 0.01f > UnityEngine.Time.timeScale ? 0.01f : UnityEngine.Time.timeScale;
            UnityEngine.Object.Destroy(go, clip.length * scale);
            return source;
        }
    }
}
