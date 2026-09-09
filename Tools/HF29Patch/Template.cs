namespace UnityEngine
{
    public struct Vector3 { public float x; public float y; public float z; }
    public class Object
    {
        public static bool operator ==(Object? a, Object? b) => ReferenceEquals(a, b);
        public static bool operator !=(Object? a, Object? b) => !ReferenceEquals(a, b);
        public override bool Equals(object? obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
    }
    public class GameObject : Object { public bool activeSelf { get; } }
    public class Component : Object { public GameObject gameObject { get; } = null!; }
    public class MonoBehaviour : Component { }
}

namespace Template
{
    public enum Camp { Plant, Zombie }

    public class Zombie : UnityEngine.MonoBehaviour
    {
        public float fX;
        public float fY;
        public float fZ;
        public float fW;
        public float fD;
        public float fH;
        public Camp camp;
        public UnityEngine.Vector3 previousPosition;
        public bool CanAttacked() => false;
        public UnityEngine.Vector3 GetPredictedPosition(float time) => default;
    }

    public class ZombieManager : UnityEngine.MonoBehaviour
    {
        public List<Zombie> zombieList = null!;
    }

    public class Board : UnityEngine.MonoBehaviour
    {
        public ZombieManager zombieManager = null!;
    }

    public class Projectile : UnityEngine.MonoBehaviour
    {
        public int ID;
        public Camp camp;
        public float fX;
        public float fY;
        public float fZ;
        public float fW;
        public float fD;
        public float fH;
        public UnityEngine.Vector3 previousPosition;
        public Board board = null!;

        private void Collision_Zombie(Zombie zombie) { }

        private bool CollisionDetect_Zombie(bool sameCamp)
        {
            if (ID != 19 && ID != 23)
            {
                Zombie target = null!;
                foreach (Zombie zombie in board.zombieManager.zombieList)
                {
                    if (!zombie.CanAttacked())
                        continue;
                    if ((zombie.camp == camp) != sameCamp)
                        continue;
                    if (!((fW + zombie.fW) * 0.5f > MathF.Abs(zombie.fX - fX)))
                        continue;
                    if (!((fD + zombie.fD) * 0.5f > MathF.Abs(zombie.fY - fY)))
                        continue;
                    if (!(zombie.fZ + zombie.fH > fZ - fH * 0.5f))
                        continue;
                    if (!(fZ + fH * 0.5f > zombie.fZ))
                        continue;
                    target = zombie;
                    break;
                }

                if (target != null)
                {
                    Collision_Zombie(target);
                    return true;
                }
                return false;
            }

            bool hit = false;
            for (int i = 0; i < board.zombieManager.zombieList.Count; i++)
            {
                Zombie zombie = board.zombieManager.zombieList[i];
                if (!zombie.CanAttacked())
                    continue;
                if ((zombie.camp == camp) != sameCamp)
                    continue;

                UnityEngine.Vector3 predicted = zombie.GetPredictedPosition(0f);
                float halfW = (zombie.fW + fW) * 0.5f;
                float halfD = (zombie.fD + fD) * 0.5f;

                // PC COMISS/JAE form: unordered does not reject the current-frame candidate.
                if (MathF.Abs(predicted.x - fX) >= halfW)
                    continue;
                if (MathF.Abs(predicted.y - fY) >= halfD)
                    continue;
                if (fZ - fH * 0.5f >= zombie.fZ + zombie.fH)
                    continue;
                if (zombie.fZ >= fZ + fH * 0.5f)
                    continue;

                // Previous-frame overlap uses the opposite COMISS branch form: unordered is not a persistent overlap.
                bool previousOverlap =
                    halfW > MathF.Abs(zombie.previousPosition.x - previousPosition.x) &&
                    halfD > MathF.Abs(zombie.previousPosition.y - previousPosition.y) &&
                    zombie.fZ + zombie.fH > fZ - fH * 0.5f &&
                    fZ + fH * 0.5f > zombie.fZ;

                if (previousOverlap)
                    continue;

                Collision_Zombie(zombie);
                hit = true;
                if (!gameObject.activeSelf)
                    break;
            }
            return hit;
        }
    }
}
