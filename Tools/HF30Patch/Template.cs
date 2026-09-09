namespace UnityEngine
{
    public struct Vector3 { public float x; public float y; public float z; }
    public class Object
    {
        public static bool operator ==(Object? a, Object? b) => ReferenceEquals(a,b);
        public static bool operator !=(Object? a, Object? b) => !ReferenceEquals(a,b);
        public override bool Equals(object? obj) => base.Equals(obj);
        public override int GetHashCode() => base.GetHashCode();
    }
    public class GameObject : Object { public bool activeSelf { get; } }
    public class Transform : Object { public Vector3 position { get; } }
    public class Component : Object { public GameObject gameObject { get; } = null!; public Transform transform { get; } = null!; }
    public class MonoBehaviour : Component { }
}

namespace Template
{
    public enum Camp { Plant, Zombie }

    public class Damage { public bool AreaDamage() => false; }

    public class Device : UnityEngine.MonoBehaviour
    {
        public float fX; public float fY; public float fZ; public float fW; public float fD; public float fH;
        public Camp camp;
        public bool CanAttacked(Damage damage) => false;
        public void TakeDamage(Damage damage, Projectile projectile) { }
    }

    public class DeviceManager : UnityEngine.MonoBehaviour { public List<Device> deviceList = null!; }
    public class Board : UnityEngine.MonoBehaviour { public DeviceManager deviceManager = null!; }

    public class Projectile : UnityEngine.MonoBehaviour
    {
        public int ID; public Camp camp;
        public Damage damage = null!;
        public float fX; public float fY; public float fZ; public float fZ_shadow; public float fW; public float fD; public float fH;
        public UnityEngine.Vector3 previousPosition;
        public Board board = null!;

        private void Collision_Device(Device device) { }
        private void Collision_AudioParticle() { }
        public void DestroyProjectile() { }

        private bool CollisionDetect_Device(bool sameCamp)
        {
            if (ID != 19 && ID != 23)
            {
                Device target = null!;
                foreach (Device device in board.deviceManager.deviceList)
                {
                    if (!device.CanAttacked(damage)) continue;
                    if ((device.camp == camp) != sameCamp) continue;
                    if (!((fW + device.fW) * 0.5f > MathF.Abs(device.fX - fX))) continue;
                    if (!((fD + device.fD) * 0.5f > MathF.Abs(device.fY - fY))) continue;
                    if (!(device.fZ + device.fH > fZ - fH * 0.5f)) continue;
                    if (!(fZ + fH * 0.5f > device.fZ)) continue;
                    target = device;
                    break;
                }
                if (target != null)
                {
                    Collision_Device(target);
                    return true;
                }
                return false;
            }

            List<Device> hits = new List<Device>();
            bool hit = false;
            foreach (Device device in board.deviceManager.deviceList)
            {
                if (!device.CanAttacked(damage)) continue;
                if ((device.camp == camp) != sameCamp) continue;
                UnityEngine.Vector3 position = device.transform.position;
                float halfW = (fW + device.fW) * 0.5f;
                float halfD = (fD + device.fD) * 0.5f;
                if (MathF.Abs(position.x - fX) >= halfW) continue;
                if (MathF.Abs(position.y - fY) >= halfD) continue;
                if (fZ - fH * 0.5f >= device.fZ + device.fH) continue;
                if (device.fZ >= fZ + fH * 0.5f) continue;
                bool previousOverlap =
                    halfW > MathF.Abs(position.x - previousPosition.x) &&
                    halfD > MathF.Abs(position.y - previousPosition.y) &&
                    device.fZ + device.fH > fZ - fH * 0.5f &&
                    fZ + fH * 0.5f > device.fZ;
                if (previousOverlap) continue;
                hits.Add(device);
                hit = true;
            }

            foreach (Device device in hits)
            {
                if (ID == 10 || ID == 11 || ID == 15 || (ID >= 26 && ID <= 32)) damage.AreaDamage();
                else device.TakeDamage(damage, this);
                Collision_AudioParticle();
                if (ID != 19 && ID != 23) DestroyProjectile();
                if (!gameObject.activeSelf) break;
            }
            return hit;
        }
    }
}
