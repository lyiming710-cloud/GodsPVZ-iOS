using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public class Object
    {
        public static T Instantiate<T>(T original) where T : Object => null!;
        public static implicit operator bool(Object? value) => value is not null;
    }

    public class Component : Object
    {
        public Transform transform => null!;
    }

    public class Behaviour : Component { }
    public class MonoBehaviour : Behaviour { }

    public class GameObject : Object
    {
        public Transform transform => null!;
    }

    public class Transform : Component
    {
        public Vector3 position { get; set; }
        public void SetParent(Transform parent, bool worldPositionStays) { }
    }

    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
    }

    public class AudioClip : Object { }

    public class AudioSource : Behaviour
    {
        public void Pause() { }
    }

    public class Camera : Behaviour
    {
        public static Camera main => null!;
    }
}

namespace Template
{
    public class Zombie : MonoBehaviour
    {
        public void BGMPasue() { }
    }

    public class ZombieManager : MonoBehaviour
    {
        public List<Zombie> zombieList = null!;

        public void BGMPasue()
        {
            foreach (Zombie zombie in zombieList)
            {
                zombie.BGMPasue();
            }
        }
    }

    public static class ResourceManager
    {
        public static Window_Q prefab_Window_Q = null!;
    }

    public class Window_Q : MonoBehaviour
    {
        public int Q;
        public bool onBoard;
        public Board board = null!;

        public static Window_Q PopupNewWindow(int q, Transform parent, Board board)
        {
            Window_Q window = UnityEngine.Object.Instantiate(ResourceManager.prefab_Window_Q);
            window.Q = q;
            window.onBoard = true;
            window.board = board;
            window.transform.SetParent(parent, false);
            return window;
        }
    }

    public static class GlobalStaticVars
    {
        public static float AudioVolume() => 0f;

        public static AudioSource CreateAudioAtPoint(AudioClip clip, Vector3 position, float volume)
        {
            return CreateAudioAtPoint(clip, position, volume, 1f);
        }

        public static AudioSource CreateAudioAtPoint(AudioClip clip, Vector3 position, float volume, float pitch) => null!;
    }

    public class Board : MonoBehaviour
    {
        public bool isFailed;
        public AudioClip[] audioList1 = null!;
        public AudioSource[] audioSource = null!;
        public ZombieManager zombieManager = null!;
        public GameObject WindowsUI = null!;

        public void GamePause(bool BGMPause) { }

        public void GameFail()
        {
            isFailed = true;
            GamePause(true);
            AudioClip clip = audioList1[2];
            Vector3 cameraPosition = Camera.main.transform.position;
            float volume = GlobalStaticVars.AudioVolume() * 0.8f;
            GlobalStaticVars.CreateAudioAtPoint(clip, cameraPosition, volume);
            audioSource[0].Pause();
            zombieManager.BGMPasue();
            Window_Q.PopupNewWindow(3, WindowsUI.transform, this);
        }
    }
}
