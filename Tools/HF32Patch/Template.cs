namespace UnityEngine
{
    public struct Vector3
    {
        public float x; public float y; public float z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    }
    public class Object
    {
        public static implicit operator bool(Object? o) => o is not null;
    }
    public class Component : Object { }
    public class MonoBehaviour : Component { }
}

namespace Template
{
    public class Zombie : UnityEngine.MonoBehaviour
    {
        public float fX;
        public float fY;
        public bool IsDisabled() => false;
        public bool CanAttacked() => false;
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
        public Board board = null!;
        public float fX;
        public float fY;

        public void Aim(UnityEngine.Vector3 distant) { }

        private void Update_Tracking()
        {
            Zombie target = null!;
            float minX = 2147483648f;

            foreach (Zombie zombie in board.zombieManager.zombieList)
            {
                if (zombie.IsDisabled())
                    continue;
                if (zombie.CanAttacked() && minX > zombie.fX)
                {
                    target = zombie;
                    minX = zombie.fX;
                }
            }

            if (target)
                Aim(new UnityEngine.Vector3(target.fX - fX, target.fY - fY, 0f));
        }
    }
}
