namespace UnityEngine
{
    public class Object
    {
        public static bool operator ==(Object? a, Object? b) => ReferenceEquals(a, b);
        public static bool operator !=(Object? a, Object? b) => !ReferenceEquals(a, b);
        public override bool Equals(object? obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
    }
    public class Component : Object { }
    public class MonoBehaviour : Component { }
}

namespace Template
{
    public enum Camp { Plant, Zombie }

    public class Damage
    {
        public bool AreaDamage() => false;
    }

    public class Plant : UnityEngine.MonoBehaviour
    {
        public float fX;
        public float fY;
        public float fW;
        public float fD;
        public float fH;
        public Camp camp;
        public void TakeDamage(Damage damage, Projectile projectile) { }
    }

    public class PlantManager : UnityEngine.MonoBehaviour
    {
        public List<Plant> plants = null!;
    }

    public class Board : UnityEngine.MonoBehaviour
    {
        public PlantManager plantManager = null!;
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
        public Board board = null!;
        public Damage damage = null!;

        private void Collision_AudioParticle() { }
        public void DestroyProjectile() { }

        private bool CollisionDetect_Plant(bool sameCamp)
        {
            Plant target = null!;
            foreach (Plant plant in board.plantManager.plants)
            {
                if (!((fW + plant.fW) * 0.5f > MathF.Abs(plant.fX - fX)))
                    continue;
                if (!((fD + plant.fD) * 0.5f > MathF.Abs(plant.fY - fY)))
                    continue;
                if ((plant.camp == camp) != sameCamp)
                    continue;
                if (!(plant.fH > fZ - fH * 0.5f))
                    continue;
                if (!(fZ + fH * 0.5f > 0f))
                    continue;
                target = plant;
                break;
            }

            if (target != null)
            {
                if (ID == 10 || ID == 11 || ID == 15 || (ID >= 26 && ID <= 32))
                    damage.AreaDamage();
                else
                    target.TakeDamage(damage, this);

                Collision_AudioParticle();
                if (ID != 19 && ID != 23)
                    DestroyProjectile();
                return true;
            }

            return false;
        }
    }
}
