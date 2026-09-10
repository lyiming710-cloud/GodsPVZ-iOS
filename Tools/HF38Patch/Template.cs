namespace UnityEngine
{
    public class Object
    {
        public static bool operator ==(Object? a, Object? b) => ReferenceEquals(a, b);
        public static bool operator !=(Object? a, Object? b) => !ReferenceEquals(a, b);
        public override bool Equals(object? obj) => ReferenceEquals(this, obj);
        public override int GetHashCode() => base.GetHashCode();
    }

    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 zero => default;
    }

    public class Transform : Object
    {
        public Vector3 position { get; set; }
    }

    public class Component : Object
    {
        public Transform transform => null!;
    }

    public class MonoBehaviour : Component { }
}

namespace Template
{
    public enum PlantType
    {
        Null = -1,
        Peasniper = 0,
        Sunflower = 1,
        CherryBlaster = 2,
        StarfruitSwordImmortal = 49,
    }

    public enum Camp { }

    public class StatsIncreased
    {
        public float value;
    }

    public class BuffManager
    {
        public StatsIncreased FindStatsIncreased(string name) => null!;
    }

    public class Skill
    {
        public int ID;
    }

    public class Zombie : UnityEngine.MonoBehaviour
    {
        public bool ashes;
        public float fX;
        public float fY;
    }

    public class Damage { }

    public class Projectile : UnityEngine.MonoBehaviour
    {
        public float maxLivingTime;

        public void Initial<T>(UnityEngine.Vector3 startPosition, UnityEngine.Vector3 speed,
            UnityEngine.Vector3 acceleration, float fZ, int movementTracks, T origin) { }

        public void SetDamage(Damage damage, Camp camp) { }
    }

    public class ProjectileManager
    {
        public Projectile CrateNewProjectile(int id) => null!;
    }

    public class Board
    {
        public ProjectileManager projectileManager = null!;
    }

    public class Plant : UnityEngine.MonoBehaviour
    {
        public PlantType plantType;
        public int order;
        public float attackIntervalCountdown;
        public Board board = null!;
        public Camp camp;
        public BuffManager buffManager = null!;
        public Skill skill = null!;
        public bool skillOngoing;

        public Damage GetDamage(Projectile projectile, int projectileID, int specialType) => null!;

        public void KillEvent(Zombie zombie)
        {
            if (plantType == PlantType.Peasniper && order >= 1)
                attackIntervalCountdown *= 0.6f;

            if (plantType == PlantType.CherryBlaster && order >= 1 && zombie != null && zombie.ashes)
            {
                Projectile projectile = board.projectileManager.CrateNewProjectile(28);
                if (projectile != null)
                {
                    projectile.Initial<Plant>(
                        new UnityEngine.Vector3(zombie.fX, zombie.fY + 20f, 0f),
                        UnityEngine.Vector3.zero,
                        UnityEngine.Vector3.zero,
                        20f,
                        0,
                        this);
                    projectile.SetDamage(GetDamage(projectile, 28, 0), camp);
                }
            }

            if (plantType == PlantType.StarfruitSwordImmortal)
                PC_SSI_KillEvent(zombie);
        }

        private void PC_SSI_KillEvent(Zombie zombie)
        {
            if (order < 1)
                return;

            StatsIncreased increased = buffManager.FindStatsIncreased("SSI_Characteristic_Atk");
            if (increased != null)
            {
                float cap = 2f;
                if (skill.ID == 2 && skillOngoing)
                    cap = 3.5f;

                if (increased.value < cap)
                {
                    increased.value += 0.15f;
                    if (increased.value > cap)
                        increased.value = cap;
                }
            }

            if (order >= 3)
            {
                Projectile projectile = board.projectileManager.CrateNewProjectile(21);
                UnityEngine.Vector3 position = zombie.transform.position;
                position.y += 1200f;
                projectile.Initial<Plant>(position, UnityEngine.Vector3.zero, UnityEngine.Vector3.zero, 1200f, 6, this);
                projectile.SetDamage(null!, camp);
                projectile.maxLivingTime = 7f;

                if (skill.ID == 3 && skillOngoing)
                    projectile.maxLivingTime *= 3f;
            }
        }
    }
}
