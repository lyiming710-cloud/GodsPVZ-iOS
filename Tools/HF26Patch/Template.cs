using System.Collections.Generic;

namespace UnityEngine
{
    public class Object
    {
        public static implicit operator bool(Object obj) => false;
    }
    public class Component : Object
    {
        public GameObject gameObject { get; } = null!;
        public Transform transform { get; } = null!;
    }
    public class GameObject : Object
    {
        public Transform transform { get; } = null!;
        public T AddComponent<T>() where T : Component => null!;
        public T GetComponent<T>() where T : Component => null!;
    }
    public class Transform : Component
    {
        public Vector3 position { get; set; }
        public Vector3 localScale { get; set; }
    }
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    }
    public class Animator : Component
    {
        public void SetInteger(string name, int value) { }
    }
    public class Camera : Component
    {
        public static Camera main { get; } = null!;
    }
    public class AudioClip : Object { }
    public class AudioSource : Component { }
    public static class Random
    {
        public static int Range(int minInclusive, int maxExclusive) => 0;
        public static float Range(float minInclusive, float maxInclusive) => 0f;
    }
}

namespace UnityEngine.Rendering
{
    public class SortingGroup : UnityEngine.Component
    {
        public string sortingLayerName { get; set; } = string.Empty;
    }
}

namespace Template
{
    public enum ParticleState
    {
        None = 0,
        CherryBoom = 2,
        IcicleSlapt = 12,
        LightSaberDoom = 19,
        PeaSlapt = 21,
        PotatoBoom = 23,
        SnowPeaSlapt = 31,
        SPHBombing = 33
    }

    public class Board
    {
        public void Shake(float time, float amplitude) { }
    }

    public class ParticlesManager
    {
        public static ParticlesManager instance { get; } = null!;
        public UnityEngine.GameObject CreatNewParticle(ParticleState state) => null!;
    }

    public static class ResourceManager
    {
        public static List<UnityEngine.AudioClip> zombieClips = null!;
        public static List<UnityEngine.AudioClip> particleClips = null!;
    }

    public static class GlobalStaticVars
    {
        public class LawnApp { public float audioVolume; }
        public static LawnApp gLawnApp = null!;
        public static UnityEngine.AudioSource CreateAudioAtPoint(UnityEngine.AudioClip clip, UnityEngine.Vector3 position, float volume, float pitch) => null!;
    }

    public class Projectile
    {
        public int ID;
        public int index;
        public float fX;
        public float fY;
        public float fZ;
        public Board board = null!;

        private void Collision_AudioParticle()
        {
            float volume = GlobalStaticVars.gLawnApp.audioVolume;

            bool makeParticle = false;
            ParticleState state = ParticleState.None;
            float particleY = fY + fZ;
            float particleScale = 1f;

            switch (ID)
            {
                case 0:
                    makeParticle = true;
                    state = ParticleState.PeaSlapt;
                    break;
                case 1:
                    makeParticle = true;
                    state = ParticleState.SnowPeaSlapt;
                    break;
                case 9:
                    makeParticle = true;
                    state = ParticleState.CherryBoom;
                    particleY = fY + 40f;
                    particleScale = 1.1f;
                    board.Shake(0.15f, 8f);
                    break;
                case 10:
                case 11:
                    makeParticle = true;
                    state = ParticleState.CherryBoom;
                    board.Shake(0.1f, 5f);
                    break;
                case 15:
                    makeParticle = true;
                    state = ParticleState.PotatoBoom;
                    particleY = fY;
                    break;
                case 19:
                case 20:
                    makeParticle = true;
                    state = ParticleState.IcicleSlapt;
                    break;
                case 26:
                    makeParticle = true;
                    state = ParticleState.SPHBombing;
                    board.Shake(0.5f, 10f);
                    break;
                case 27:
                case 28:
                    makeParticle = true;
                    state = ParticleState.CherryBoom;
                    particleScale = 0.75f;
                    board.Shake(0.1f, 5f);
                    break;
                case 30:
                    makeParticle = true;
                    state = ParticleState.LightSaberDoom;
                    particleY = fY;
                    board.Shake(0.15f, 10f);
                    break;
                case 31:
                    makeParticle = true;
                    state = ParticleState.SPHBombing;
                    board.Shake(0.5f, 10f);
                    break;
                case 32:
                    makeParticle = true;
                    state = ParticleState.CherryBoom;
                    particleScale = 1.2f;
                    board.Shake(0.2f, 8f);
                    break;
            }

            if (makeParticle)
            {
                UnityEngine.GameObject particle = ParticlesManager.instance.CreatNewParticle(state);
                if (particle)
                {
                    particle.transform.position = new UnityEngine.Vector3(fX, particleY, 0f);
                    UnityEngine.Rendering.SortingGroup sortingGroup = particle.AddComponent<UnityEngine.Rendering.SortingGroup>();
                    sortingGroup.sortingLayerName = "Particles";
                    UnityEngine.Vector3 oldScale = particle.transform.localScale;
                    particle.transform.localScale = new UnityEngine.Vector3(oldScale.x * particleScale, oldScale.y * particleScale, oldScale.z * particleScale);
                    if (ID == 30)
                        particle.GetComponent<UnityEngine.Animator>().SetInteger("type", index);
                }
            }

            UnityEngine.AudioClip clip = null!;
            float pitch;
            bool play = false;
            switch (ID)
            {
                case 0:
                case 1:
                    pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    clip = ResourceManager.particleClips[UnityEngine.Random.Range(0, 3) + 7];
                    volume *= 0.8f;
                    play = true;
                    break;
                case 9:
                case 10:
                case 11:
                    pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    clip = ResourceManager.particleClips[1];
                    volume *= 0.8f;
                    play = true;
                    break;
                case 15:
                    pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    clip = ResourceManager.particleClips[0];
                    play = true;
                    break;
                case 19:
                    pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    clip = ResourceManager.particleClips[10];
                    play = true;
                    break;
                case 20:
                    pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    clip = ResourceManager.particleClips[11];
                    play = true;
                    break;
                case 23:
                    pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    clip = ResourceManager.particleClips[21];
                    play = true;
                    break;
                case 24:
                    pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    clip = ResourceManager.particleClips[11];
                    volume *= 0.5f;
                    play = true;
                    break;
                case 25:
                    pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    clip = ResourceManager.particleClips[11];
                    volume *= 0.75f;
                    play = true;
                    break;
                case 26:
                case 31:
                    pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    clip = ResourceManager.particleClips[16];
                    volume *= 1.5f;
                    play = true;
                    break;
                case 27:
                case 28:
                    pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    clip = ResourceManager.particleClips[1];
                    volume *= 0.6f;
                    play = true;
                    break;
                case 30:
                    pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    clip = ResourceManager.particleClips[22];
                    GlobalStaticVars.CreateAudioAtPoint(ResourceManager.zombieClips[10], UnityEngine.Camera.main.transform.position, volume, pitch);
                    play = true;
                    break;
                case 32:
                    pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    clip = ResourceManager.particleClips[1];
                    volume *= 1.2f;
                    play = true;
                    break;
                default:
                    return;
            }

            if (play && clip)
                GlobalStaticVars.CreateAudioAtPoint(clip, UnityEngine.Camera.main.transform.position, volume, pitch);
        }
    }
}
