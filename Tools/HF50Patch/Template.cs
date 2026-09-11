using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public class Object
    {
        public static implicit operator bool(Object? a) => a is not null;
        public static bool operator ==(Object? a, Object? b) => ReferenceEquals(a, b);
        public static bool operator !=(Object? a, Object? b) => !ReferenceEquals(a, b);
        public static T Instantiate<T>(T original) where T : Object => original;
        public override bool Equals(object? obj) => ReferenceEquals(this, obj);
        public override int GetHashCode() => base.GetHashCode();
    }
    public class Component : Object { public Transform transform => null!; }
    public class Behaviour : Component { }
    public class MonoBehaviour : Behaviour { }
    public class GameObject : Object { public Transform transform => null!; }
    public class Transform : Component
    {
        public Vector3 position { get; set; }
        public Vector3 localScale { get; set; }
        public void SetParent(Transform parent, bool worldPositionStays) { }
    }
    public class Animator : Behaviour
    {
        public void SetInteger(string name, int value) { }
        public void SetBool(string name, bool value) { }
        public void SetBool(int id, bool value) { }
    }
    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x=x; this.y=y; this.z=z; }
        public static Vector3 zero => default;
    }
    public static class Random { public static int Range(int minInclusive, int maxExclusive) => 0; }
}

namespace Template
{
    using UnityEngine;

    public enum OccupyState { Null = -1, Bottom = 0, Cover = 1, Normal = 2, Floating = 3, Ladder = 4 }

    public class Device : MonoBehaviour { }
    public class Grid
    {
        public Device device_bottom = null!;
        public Device device_common = null!;
        public Device device_sheath = null!;
        public Device device_ladder = null!;
        public Device device_top = null!;

        public Device FindDevice_Occupy(OccupyState occupyState)
        {
            return occupyState switch
            {
                OccupyState.Bottom => device_bottom,
                OccupyState.Cover => device_sheath,
                OccupyState.Normal => device_common,
                OccupyState.Floating => device_top,
                OccupyState.Ladder => device_ladder,
                _ => null!
            };
        }
    }

    public static class GlobalStaticVars
    {
        public static GameObject GetAnimationSprite_Name(List<GameObject> animationSprites, string spriteName) => null!;
        public static Vector3 GetAnimationSpritePosition(List<GameObject> animationSprites, string childName)
        {
            GameObject obj = GetAnimationSprite_Name(animationSprites, childName);
            if (obj != null)
                return obj.transform.position;
            return Vector3.zero;
        }
    }

    public class Project : MonoBehaviour
    {
        public int movementTracks;
        public Board board = null!;
    }
    public class SunManager : MonoBehaviour { public List<Project> suns = null!; }
    public class EnemyManager : MonoBehaviour { public bool finish; }
    public class ZombieManager : MonoBehaviour { public List<Zombie> zombieList = null!; }
    public class Board : MonoBehaviour
    {
        public SunManager sunManager = null!;
        public EnemyManager enemyManager = null!;
        public ZombieManager zombieManager = null!;

        public bool TestWinTargetZombie()
        {
            if (enemyManager && enemyManager.finish)
            {
                foreach (Zombie zombie in zombieManager.zombieList)
                {
                    if (zombie.IsWinTarget())
                        return false;
                }
                return true;
            }
            return false;
        }
    }
    public class ProjectManager : MonoBehaviour
    {
        public Board board = null!;
        public List<Project> projectPrefabs = null!;
        public List<Project> projects = null!;

        public Project CreateProject(int movementTracks, int ID, Vector3 position)
        {
            Project prefab = projectPrefabs[ID];
            if (!prefab)
                return null!;

            Project project = UnityEngine.Object.Instantiate(projectPrefabs[ID]);
            project.movementTracks = movementTracks;
            project.transform.position = position;
            project.board = board;

            if (ID != 0)
            {
                projects.Add(project);
                project.transform.SetParent(transform, false);
            }
            else
            {
                board.sunManager.suns.Add(project);
                project.transform.SetParent(board.sunManager.transform, false);
            }
            return project;
        }
    }

    public class Zombie : MonoBehaviour
    {
        public int ID;
        public int brokenLevel;
        public bool isStant;
        public float waitingTime;
        public Vector3 rSpeed;
        private Animator animator = null!;
        private static readonly int Ani_StantHash;

        public bool IsPlantZombie() => false;
        public bool IsWinTarget() => false;
        public void ResetIdleSpeed() { }

        private void TranToStant(float waitingTime)
        {
            this.waitingTime = waitingTime;
            rSpeed = Vector3.zero;
            isStant = true;

            if (ID != 13)
            {
                Transform target = transform;
                Vector3 scaleX = transform.localScale;
                Vector3 scaleY = transform.localScale;
                Vector3 scaleZ = transform.localScale;
                target.localScale = new Vector3(Math.Abs(scaleX.x), scaleY.y, scaleZ.z);
            }

            if (ID == 0 || ID == 2 || ID == 4 || ID == 5 || ID == 14 || ID == 15 || IsPlantZombie())
            {
                animator.SetInteger("Group", UnityEngine.Random.Range(0, 2));
            }

            animator.SetBool(Ani_StantHash, true);
            ResetIdleSpeed();
            if (ID == 17 && waitingTime > 0f && brokenLevel < 2)
                animator.SetBool("Ready", true);
        }
    }
}
