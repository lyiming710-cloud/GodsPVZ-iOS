using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public class Object
    {
        public static implicit operator bool(Object? value) => !ReferenceEquals(value, null);
    }

    public class Component : Object
    {
        public Transform transform = null!;
    }

    public class Behaviour : Component { }
    public class MonoBehaviour : Behaviour { }

    public class Transform : Component
    {
        public Vector3 position { get; set; }
    }

    public class Camera : Behaviour
    {
        public static Camera main => null!;
    }

    public class AudioClip : Object { }
    public class AudioSource : Behaviour { }

    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
    }
}

namespace Template
{
    public static class ResourceManager
    {
        public static List<AudioClip> boardClips = null!;
    }

    public static class GlobalStaticVars
    {
        public static float AudioVolume() => 0f;
        public static AudioSource CreateAudioAtPoint(AudioClip clip, Vector3 position, float volume, float pitch) => null!;
    }

    public class Board : MonoBehaviour
    {
        public ZombieManager zombieManager = null!;
    }

    public class ZombieManager : MonoBehaviour
    {
        public List<Zombie> zombieList = null!;
    }

    public class Zombie : MonoBehaviour
    {
        public int wave;
        public float healthPoint;
        public float armor1Point;
        public float armor2Point;
        public bool isDying;
    }

    public class EnemyManager : MonoBehaviour
    {
        public int theWave;
        private float waveHealth;
        public float waveHealthRemainder;
        public Board board = null!;

        private void PlayBoardAudio(int ID)
        {
            AudioClip audioClip = ResourceManager.boardClips[ID];
            if (audioClip)
            {
                Vector3 position = Camera.main.transform.position;
                float volume = GlobalStaticVars.AudioVolume();
                GlobalStaticVars.CreateAudioAtPoint(audioClip, position, volume, 1f);
            }
        }

        private bool TextWaveHealth()
        {
            waveHealthRemainder = 0f;

            Board b = board;
            if ((object)b == null)
                throw new NullReferenceException();

            ZombieManager zm = b.zombieManager;
            if ((object)zm == null)
                throw new NullReferenceException();

            List<Zombie> list = zm.zombieList;
            if (list == null)
                throw new NullReferenceException();

            foreach (Zombie z in list)
            {
                if ((object)z == null)
                    throw new NullReferenceException();

                if (z.wave == theWave - 1 && !z.isDying)
                {
                    waveHealthRemainder += z.healthPoint;
                    waveHealthRemainder += z.armor1Point;
                    waveHealthRemainder += z.armor2Point * 0.2f;
                }
            }

            if (theWave % 10 != 9)
                return waveHealth * 0.5 >= waveHealthRemainder;

            return 0f >= waveHealthRemainder;
        }
    }
}
