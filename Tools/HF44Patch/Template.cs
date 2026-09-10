using System;
using UnityEngine;

namespace UnityEngine
{
    public class Object
    {
        public static implicit operator bool(Object? value) => !ReferenceEquals(value, null);
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
    public class Animator : Behaviour
    {
        public float GetFloat(int id) => 0f;
        public void SetFloat(int id, float value) { }
    }
    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    }
}

namespace Template
{
    public enum OccupyState
    {
        Null = -1,
        Bottom = 0,
        Cover = 1,
        Normal = 2,
        Floating = 3,
        Ladder = 4,
    }

    public class BoardConfig
    {
        public int GetGridX(float fX, float fY) => 0;
        public int GetGridY(float fX, float fY) => 0;
        public Vector3 GetGridCenterPosition(int gridX, int gridY) => default;
    }

    public class Board : MonoBehaviour
    {
        public BoardConfig boardConfig = null!;
        public Grid? GetGrid(int gridX, int gridY) => null;
    }

    public class Grid
    {
        public int GetPassablePoint() => 0;
        public Device? FindDevice_Occupy(OccupyState occupyState) => null;
    }

    public class Device : MonoBehaviour
    {
        public void DC_LadderClimb(Zombie zombie) { }
    }

    public class Zombie : MonoBehaviour
    {
        public Board board = null!;
        public int gridX;
        public int gridY;
        public int passablePoint;
        public int ID;
        public float fX;
        public float fY;
        public float updateRate;
        public Animator animator = null!;
        private static int Ani_SpeedHash;
        private static int Ani_AttackSpeedHash;
        private static int Ani_MoveSpeedHash;

        public float GetMS() => 0f;
        public Vector3 GetMoveDirection() => default;
        public void ResetIdleSpeed() { }
        public void ResetAttackSpeed() { }
        private void ZC_SnowbeastImpact(int gridX, int gridY) { }
        private void ZC_SnowbeastHitWall() { }

        public void ResetMoveSpeed()
        {
            float moveSpeed = GetMS();
            if (ID == 17 && GetMoveDirection().x < 0f)
                moveSpeed *= -1f;
            animator.SetFloat(Ani_MoveSpeedHash, moveSpeed);
        }

        private void ResetUpdateRate(float newUpdateRate)
        {
            if (animator)
            {
                if (updateRate == 0f)
                {
                    updateRate = newUpdateRate;
                    ResetIdleSpeed();
                    ResetAttackSpeed();
                    ResetMoveSpeed();
                }
                else
                {
                    float oldIdle = animator.GetFloat(Ani_SpeedHash);
                    animator.SetFloat(Ani_SpeedHash, newUpdateRate / updateRate * oldIdle);
                    float oldAttack = animator.GetFloat(Ani_AttackSpeedHash);
                    animator.SetFloat(Ani_AttackSpeedHash, newUpdateRate / updateRate * oldAttack);
                    float oldMove = animator.GetFloat(Ani_MoveSpeedHash);
                    animator.SetFloat(Ani_MoveSpeedHash, newUpdateRate / updateRate * oldMove);
                }
            }
            updateRate = newUpdateRate;
        }

        private void TestPosition(float nextX, float nextY)
        {
            int nextGridX = board.boardConfig.GetGridX(nextX, nextY);
            int nextGridY = board.boardConfig.GetGridY(nextX, nextY);
            if (nextGridX == gridX && nextGridY == gridY)
                return;

            float deltaX = nextGridX - gridX;
            float deltaY = nextGridY - gridY;
            int signX = Math.Sign(deltaX);
            int signY = Math.Sign(deltaY);
            Grid? grid = board.GetGrid(nextGridX, nextGridY);
            if (grid != null)
            {
                int neededPassablePoint = grid.GetPassablePoint();
                if (passablePoint >= neededPassablePoint)
                {
                    gridX = nextGridX;
                    gridY = nextGridY;
                    if (ID == 13)
                        ZC_SnowbeastImpact(nextGridX, nextGridY);
                    if (signX < 0)
                    {
                        Device? ladder = grid.FindDevice_Occupy(OccupyState.Ladder);
                        if (ladder)
                            ladder.DC_LadderClimb(this);
                    }
                    return;
                }

                Vector3 currentCenter = board.boardConfig.GetGridCenterPosition(gridX, gridY);
                Vector3 nextCenter = board.boardConfig.GetGridCenterPosition(nextGridX, nextGridY);
                if (deltaX != 0f)
                    fX = (nextCenter.x + currentCenter.x) * 0.5f - signX * 3f;
                if (deltaY != 0f)
                    fY = (nextCenter.y + currentCenter.y) * 0.5f + signY * 3f;

                Transform t = transform;
                Vector3 position = t.position;
                position.x = nextX;
                position.y = nextY;
                t.position = position;
                if (ID == 13)
                    ZC_SnowbeastHitWall();
            }
            else
            {
                gridX = nextGridX;
                gridY = nextGridY;
            }
        }
    }
}
