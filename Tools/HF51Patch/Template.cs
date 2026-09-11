using UnityEngine;

namespace UnityEngine
{
    public class Object
    {
        public static implicit operator bool(Object? value) => value is not null;
    }

    public class Component : Object
    {
        public Transform transform => null!;
    }

    public class Behaviour : Component { }
    public class MonoBehaviour : Behaviour { }

    public class Transform : Component
    {
        public Vector3 position { get; set; }
    }

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

    public static class Random
    {
        public static float Range(float minInclusive, float maxInclusive) => minInclusive;
    }
}

namespace Template
{
    public class Plant : MonoBehaviour
    {
        public float fX;
        public float fY;
        public float fZ;
    }

    public class Zombie : MonoBehaviour { }

    public class Project : MonoBehaviour
    {
        public int movementTracks;
        private Vector3 endPosition;

        public void SetEndPosition<T>(T host)
        {
            if (host is null)
                return;

            if (host is Plant plant && plant)
            {
                if (movementTracks == 1)
                {
                    endPosition = plant.transform.position;
                    endPosition.y += UnityEngine.Random.Range(0f, 15f);
                }
                else if (movementTracks == 2)
                {
                    endPosition = new Vector3(plant.fX, plant.fY + plant.fZ, 0f);
                    endPosition.y += UnityEngine.Random.Range(0f, 10f);
                }
            }

            if (host is Zombie zombie && movementTracks == 2 && zombie)
            {
                endPosition = zombie.transform.position;
                endPosition.y += UnityEngine.Random.Range(0f, 15f);
            }
        }
    }
}
