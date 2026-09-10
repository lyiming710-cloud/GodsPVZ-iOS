using System;
using UnityEngine;

namespace UnityEngine
{
    public class Object { }
    public class Component : Object { public Transform transform => null!; }
    public class Behaviour : Component { }
    public class MonoBehaviour : Behaviour { }
    public class GameObject : Object { public Transform transform => null!; }
    public class Transform : Component { public Vector3 position { get; set; } }
    public class Camera : Behaviour { public static Camera main => null!; }
    public struct Vector2 { public float x, y; }
    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    }
    public static class Random { public static Vector2 insideUnitCircle => default; }
    public static class Time
    {
        public static float fixedDeltaTime => 0f;
        public static float fixedTime => 0f;
    }
}

namespace Template
{
    public class Board : MonoBehaviour
    {
        public Vector3 dithering;
        public float amplitude;
        public float shakeTime;

        private static Vector3 RandomOffset(float amplitude)
        {
            Vector2 v = UnityEngine.Random.insideUnitCircle;
            float mag = (float)Math.Sqrt((double)(v.x * v.x + v.y * v.y));
            float x;
            float y;
            if (mag > 0.00001f)
            {
                x = v.x / mag;
                y = v.y / mag;
            }
            else
            {
                x = 0f;
                y = 0f;
            }
            return new Vector3(x * amplitude, y * amplitude, 0f);
        }

        private void FixedUpdate()
        {
            if (shakeTime > 0f)
                FixedUpdate_Shake();
        }

        private void FixedUpdate_Shake()
        {
            shakeTime -= Time.fixedDeltaTime;

            Transform transform = Camera.main.transform;
            Vector3 position = transform.position;
            transform.position = new Vector3(
                position.x - dithering.x,
                position.y - dithering.y,
                position.z - dithering.z);

            if (!(0f >= shakeTime))
            {
                dithering = RandomOffset(amplitude);
                Transform transform2 = Camera.main.transform;
                Vector3 position2 = transform2.position;
                transform2.position = new Vector3(
                    position2.x + dithering.x,
                    position2.y + dithering.y,
                    position2.z + dithering.z);
                amplitude = (1f - Time.fixedDeltaTime / Time.fixedTime * 0.05f) * amplitude;
            }
            else
            {
                amplitude = 0f;
                dithering = new Vector3(0f, 0f, 0f);
            }
        }
    }

    public class Plant : MonoBehaviour
    {
        public GameObject animationGroup = null!;
        public Vector3 dithering_anim;

        private static Vector3 RandomOffset(float amplitude)
        {
            Vector2 v = UnityEngine.Random.insideUnitCircle;
            float mag = (float)Math.Sqrt((double)(v.x * v.x + v.y * v.y));
            float x;
            float y;
            if (mag > 0.00001f)
            {
                x = v.x / mag;
                y = v.y / mag;
            }
            else
            {
                x = 0f;
                y = 0f;
            }
            return new Vector3(x * amplitude, y * amplitude, 0f);
        }

        private void Dithering_Animation(float amplitude)
        {
            Vector3 next = new Vector3(0f, 0f, 0f);
            if (amplitude != 0f)
                next = RandomOffset(amplitude);

            Transform transform = animationGroup.transform;
            Vector3 position = transform.position;
            transform.position = new Vector3(
                position.x - dithering_anim.x,
                position.y - dithering_anim.y,
                position.z - dithering_anim.z);

            Transform transform2 = animationGroup.transform;
            Vector3 position2 = transform2.position;
            transform2.position = new Vector3(
                position2.x + next.x,
                position2.y + next.y,
                position2.z + next.z);

            dithering_anim = next;
        }
    }
}
