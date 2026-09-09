using System.Collections.Generic;

namespace UnityEngine
{
    public struct Vector3
    {
        public float x; public float y; public float z;
        public Vector3(float x, float y, float z) { this.x=x; this.y=y; this.z=z; }
    }

    public class Object
    {
        public static implicit operator bool(Object? o) => o is not null;
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
        public Transform transform => null!;
        public T GetComponent<T>() where T : Component => null!;
    }

    public class Sprite : Object { }

    public class SpriteRenderer : Component
    {
        public Sprite sprite { get; set; } = null!;
    }

    public class AudioClip : Object { }

    public class AudioSource : Component { }

    public class Camera : MonoBehaviour
    {
        public static Camera main => null!;
    }
}

enum ParticleState
{
    RoadblockBroken = 26
}

static class ResourceManager
{
    public static List<UnityEngine.Sprite> deviceSprites = null!;
    public static List<UnityEngine.AudioClip> particleClips = null!;
}

static class GlobalStaticVars
{
    public static UnityEngine.GameObject GetAnimationSprite_Name(List<UnityEngine.GameObject> list, string name) => null!;
    public static float AudioVolume() => 0f;
    public static UnityEngine.AudioSource CreateAudioAtPoint(UnityEngine.AudioClip clip, UnityEngine.Vector3 position, float volume, float pitch) => null!;
}

class ParticlesManager : UnityEngine.MonoBehaviour
{
    public static ParticlesManager instance => null!;
    public UnityEngine.GameObject CreatNewParticle(ParticleState state) => null!;
}

namespace Template
{
    public class Device : UnityEngine.MonoBehaviour
    {
        public int ID;
        public float healthPoint;
        public float maxHealthPoint;
        public int brokenLevel;
        public List<UnityEngine.GameObject> animationSprites = null!;

        private void Broken() { }

        private void InjuryStatusUpdate()
        {
            // Original PC native uses COMISS 0,healthPoint + JAE. This means the
            // body is entered for ordered positive values and for unordered/NaN;
            // Broken() is taken only for ordered healthPoint <= 0.
            if (!(healthPoint <= 0f))
            {
                if (ID != 4)
                    return;

                float damagedFraction = 1f - healthPoint / maxHealthPoint;
                while (damagedFraction > ((float)brokenLevel + 1f) / 3f)
                {
                    brokenLevel++;

                    UnityEngine.GameObject roadblock = GlobalStaticVars.GetAnimationSprite_Name(animationSprites, "Roadblock");
                    UnityEngine.SpriteRenderer renderer = roadblock.GetComponent<UnityEngine.SpriteRenderer>();
                    if (renderer)
                        renderer.sprite = ResourceManager.deviceSprites[brokenLevel];

                    UnityEngine.GameObject particle = ParticlesManager.instance.CreatNewParticle(ParticleState.RoadblockBroken);
                    if (particle)
                        particle.transform.position = transform.position;

                    UnityEngine.AudioClip clip = ResourceManager.particleClips[12];
                    UnityEngine.Vector3 cameraPosition = UnityEngine.Camera.main.transform.position;
                    GlobalStaticVars.CreateAudioAtPoint(clip, cameraPosition, GlobalStaticVars.AudioVolume() * 1.6f, 0.7f);
                }
            }
            else
            {
                Broken();
            }
        }
    }
}
