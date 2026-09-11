using System;
using UnityEngine;

namespace UnityEngine
{
    public class Object
    {
        public static bool operator ==(Object? a, Object? b) => ReferenceEquals(a, b);
        public static bool operator !=(Object? a, Object? b) => !ReferenceEquals(a, b);
        public override bool Equals(object? obj) => ReferenceEquals(this, obj);
        public override int GetHashCode() => base.GetHashCode();
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
    }
}

namespace Template
{
    public class BoardConfig
    {
        public Vector3 GetZombiePosition(int gridX, int gridY) => default;
    }

    public class Board : MonoBehaviour
    {
        public BoardConfig boardConfig = null!;
    }

    public class Plant : MonoBehaviour { }

    public class Zombie : MonoBehaviour
    {
        public static float deadzone_distance;
        public Board board = null!;
    }

    public class EnemyPath
    {
        public int gridX;
        public int gridY;
        public bool original;
        public Zombie zombie = null!;
        public Plant plant = null!;

        public bool ArrivalTest(Zombie father)
        {
            Board board = father.board;
            Vector3 target = board.boardConfig.GetZombiePosition(gridX, gridY);
            Vector3 current = father.transform.position;

            float dx = Math.Abs(target.x - current.x);
            if (Zombie.deadzone_distance > dx)
            {
                float dy = Math.Abs(target.y - current.y);
                if (Zombie.deadzone_distance > dy)
                    return true;
            }

            if (original && zombie == null && plant == null)
                return true;

            return false;
        }
    }
}
