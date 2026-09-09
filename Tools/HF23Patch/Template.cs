namespace UnityEngine
{
    public class Object { }
    public class Component : Object { public Transform transform { get; } = null!; }
    public class MonoBehaviour : Component { }
    public class GameObject : Object { public Transform transform { get; } = null!; }
    public class Transform : Component { public Vector3 position { get; set; } }
    public struct Vector3 { public float x; public float y; public float z; }
    public class AudioClip : Object { }
    public class AudioSource : Component { }
    public class Camera : Component { public static Camera main { get; } = null!; }
    public static class Random { public static int Range(int minInclusive, int maxExclusive) => 0; }
}

namespace Template
{
    public enum Armor1Type { none, cone, bucket, brick, iceCube, heavyHelmet, saboteursArmor }
    public enum Armor2Type { none, screendoor, newspaper, heavyShield, ladder }
    public enum DamageAttribute { normal, throughout, artillery, ashes, real }
    public enum ElementType { fire_ice }

    public class Projectile { }
    public class Element { public ElementType type; public float point; }
    public class ElementManager { public void ToEffect(Element element) { } }
    public class Buff { }
    public class StatsIncreased : Buff { public float value; }
    public class BuffManager
    {
        public float GetIncrement(float value, string name) => 0f;
        public Buff FindBuff(string name) => null!;
    }

    public class Damage
    {
        public float damagePoint;
        public DamageAttribute damageAttribute;
        public bool acrossArmor1;
        public bool acrossArmor2;
        public bool noResidue;
        public float penetrationVigour;
        public List<Element> elements = null!;
    }

    public class DamageText
    {
        public static DamageText CreatDamageText(float damagePoint, int damageType, UnityEngine.Vector3 position) => null!;
    }

    public static class ResourceManager
    {
        public static List<UnityEngine.AudioClip> zombieClips = null!;
    }

    public static class GlobalStaticVars
    {
        public static UnityEngine.Vector3 GetAnimationSpritePosition(List<UnityEngine.GameObject> sprites, string childName) => default;
        public static float AudioVolume() => 0f;
        public static UnityEngine.AudioSource CreateAudioAtPoint(UnityEngine.AudioClip clip, UnityEngine.Vector3 position, float volume) => null!;
    }

    public class Zombie : UnityEngine.MonoBehaviour
    {
        public int ID;
        public float fY;
        public float fZ;
        public float healthPoint;
        public float maxHealthPoint;
        public float defense;
        public Armor1Type armor1Type;
        public float armor1Point;
        public float maxArmor1Point;
        public float armor1Toughness;
        public float armor1Defense;
        public Armor2Type armor2Type;
        public float armor2Point;
        public float maxArmor2Point;
        public float armor2Toughness;
        public float armor2Defense;
        public bool isDied;
        public bool isStant;
        public bool poleZombie_pole;
        public ElementManager elementManager = null!;
        private List<UnityEngine.GameObject> animationSprites = null!;
        private List<UnityEngine.GameObject> anim_Armor2Sprites = null!;
        public BuffManager buffManager = null!;
        public float stiffnessTime;

        public bool IsDisabled() => false;
        public void DestroyZombie() { }
        public void Ashe(Damage damage) { }
        public void Flashing(float time, float brightness, string sprite) { }
        private float GetTOU(int type) => 0f;
        private void HurtAudio_Armor1() { }
        private void InjuryStatusUpdate_Armor1(bool noResidue) { }
        private void InjuryStatusUpdate_Armor2(bool noResidue) { }
        private bool InjuryStatusUpdate_Body(bool noResidue) => false;

        private float Hurt_Armor1(Damage damage, Projectile projectile)
        {
            if (damage == null) throw new NullReferenceException();
            if (armor1Type == Armor1Type.none) return damage.damagePoint;

            float displayDamage = damage.damagePoint;
            float remainder = damage.damagePoint;
            Flashing(0.2f, 4f, "body");
            float originalDamage = damage.damagePoint;
            float baseDefense = armor1Point / maxArmor1Point * armor1Defense;
            float defensePoint = Math.Max(MathF.Round(buffManager.GetIncrement(baseDefense, "Def") + baseDefense), 0f);
            float actualDamage = damage.damageAttribute == DamageAttribute.real
                ? originalDamage
                : (defensePoint < originalDamage ? originalDamage - defensePoint + defensePoint * 0.1f : originalDamage * 0.1f);

            if (armor1Type == Armor1Type.iceCube)
            {
                foreach (var element in damage.elements)
                {
                    if (element.type == ElementType.fire_ice)
                        armor1Point = Math.Clamp(armor1Point + element.point * 0.2f, 0f, maxArmor1Point);
                }
            }

            if (damage.damageAttribute != DamageAttribute.throughout)
            {
                if (armor1Type != Armor1Type.none && armor1Point > 0f && actualDamage > 0f)
                {
                    if (actualDamage < armor1Point)
                    {
                        displayDamage = actualDamage;
                        armor1Point -= actualDamage;
                        remainder = 0f;
                    }
                    else
                    {
                        float oldArmor = armor1Point;
                        displayDamage = oldArmor;
                        remainder = actualDamage - oldArmor;
                        armor1Point = 0f;
                    }
                }
            }
            else if (armor1Point > 0f && originalDamage > 0f)
            {
                displayDamage = actualDamage;
                armor1Point -= actualDamage;
                float ratio = damage.penetrationVigour / (GetTOU(1) + damage.penetrationVigour + 1f);
                remainder = (float)(int)(ratio * originalDamage);
            }

            if (displayDamage >= 300f && (displayDamage >= maxArmor1Point / 3f || displayDamage >= 1800f))
            {
                string childName = armor1Type switch
                {
                    Armor1Type.cone => "anim_cone",
                    Armor1Type.bucket => "anim_bucket",
                    Armor1Type.brick => "anim_brick",
                    Armor1Type.iceCube => "IceCube1",
                    Armor1Type.heavyHelmet => "anim_bucket",
                    Armor1Type.saboteursArmor => "LadderSaboteurs_helmet1",
                    _ => string.Empty,
                };
                var position = GlobalStaticVars.GetAnimationSpritePosition(animationSprites, childName);
                position.y += 25f;
                DamageText.CreatDamageText(displayDamage, 0, position);
            }

            HurtAudio_Armor1();
            InjuryStatusUpdate_Armor1(damage.noResidue);
            return remainder;
        }

        private float Hurt_Armor2(Damage damage, Projectile projectile)
        {
            if (damage == null) throw new NullReferenceException();
            if (armor2Type == Armor2Type.none) return damage.damagePoint;

            float displayDamage = damage.damagePoint;
            float result = damage.damagePoint;
            Flashing(0.2f, 4f, "armor2");
            float originalDamage = damage.damagePoint;
            float baseDefense = armor2Point / maxArmor2Point * armor2Defense;
            float defensePoint = Math.Max(MathF.Round(buffManager.GetIncrement(baseDefense, "Def") + baseDefense), 0f);
            float actualDamage = damage.damageAttribute == DamageAttribute.real
                ? originalDamage
                : (defensePoint < originalDamage ? originalDamage - defensePoint + defensePoint * 0.1f : originalDamage * 0.1f);

            if (armor2Point > 0f)
            {
                if (damage.damageAttribute != DamageAttribute.throughout)
                {
                    if (actualDamage > 0f)
                    {
                        displayDamage = actualDamage;
                        if (actualDamage <= armor2Point) armor2Point -= actualDamage;
                        else armor2Point = 0f;
                        result = 0f;
                    }
                }
                else if (originalDamage > 0f)
                {
                    displayDamage = actualDamage;
                    armor2Point -= actualDamage;
                    float ratio = damage.penetrationVigour / (GetTOU(2) + damage.penetrationVigour + 1f);
                    result = (float)(int)(ratio * originalDamage);
                }
            }

            if (displayDamage >= 300f && (displayDamage >= maxArmor2Point / 3f || displayDamage >= 1800f))
            {
                var position = anim_Armor2Sprites[0].transform.position;
                position.y += 35f;
                DamageText.CreatDamageText(displayDamage, 0, position);
            }

            if (armor2Type == Armor2Type.screendoor || armor2Type == Armor2Type.heavyShield || armor2Type == Armor2Type.ladder)
            {
                int index = UnityEngine.Random.Range(2, 4);
                if (index >= 0 && index < ResourceManager.zombieClips.Count)
                {
                    var clip = ResourceManager.zombieClips[index];
                    var main = UnityEngine.Camera.main;
                    if (main == null) throw new NullReferenceException();
                    var cameraTransform = main.transform;
                    if (cameraTransform == null) throw new NullReferenceException();
                    var position = cameraTransform.position;
                    float volume = GlobalStaticVars.AudioVolume();
                    GlobalStaticVars.CreateAudioAtPoint(clip, position, volume);
                }
            }

            InjuryStatusUpdate_Armor2(damage.noResidue);
            return result;
        }

        private bool Hurt_Artillery(Damage damage, Projectile projectile)
        {
            float remainder = damage.damagePoint;
            if (!damage.acrossArmor2) Hurt_Armor2(damage, projectile);
            foreach (var element in damage.elements)
            {
                _ = Math.Sign(element.point);
                if (element.type == ElementType.fire_ice && armor1Type == Armor1Type.iceCube && armor1Point > 0f)
                    continue;
                if (element.type == ElementType.fire_ice)
                    elementManager.ToEffect(element);
            }
            if (!damage.acrossArmor1) remainder = Hurt_Armor1(damage, projectile);
            if (remainder <= 0f) return false;
            damage.damagePoint = remainder;
            return Hurt_Body(damage, projectile);
        }

        private bool Hurt_Ashes(Damage damage, Projectile projectile)
        {
            if (IsDisabled() && isDied)
            {
                DestroyZombie();
                return false;
            }

            if (damage == null) throw new NullReferenceException();
            float remainder = damage.damagePoint;
            if (damage.damagePoint < healthPoint)
            {
                if (!damage.acrossArmor2) Hurt_Armor2(damage, projectile);
                foreach (var element in damage.elements)
                {
                    _ = Math.Sign(element.point);
                    if (element.type == ElementType.fire_ice && armor1Type == Armor1Type.iceCube && armor1Point > 0f)
                        continue;
                    if (element.type == ElementType.fire_ice)
                        elementManager.ToEffect(element);
                }
                if (!damage.acrossArmor1) remainder = Hurt_Armor1(damage, projectile);
                if (remainder <= 0f) return false;
                damage.damagePoint = remainder;
                return Hurt_Body(damage, projectile);
            }

            bool disabled = IsDisabled();
            Ashe(damage);
            return !disabled;
        }

        private bool Hurt_Body(Damage damage, Projectile projectile)
        {
            if (damage == null) throw new NullReferenceException();
            float originalDamage = damage.damagePoint;
            float baseDefense = healthPoint / maxHealthPoint * defense;
            float defensePoint = Math.Max(MathF.Round(buffManager.GetIncrement(baseDefense, "Def") + baseDefense), 0f);
            float actualDamage = damage.damageAttribute == DamageAttribute.real
                ? originalDamage
                : (defensePoint < originalDamage ? originalDamage - defensePoint + defensePoint * 0.1f : originalDamage * 0.1f);

            if (damage.damageAttribute != DamageAttribute.real && ID == 3 && poleZombie_pole && !isStant && stiffnessTime <= 0f)
            {
                var buff = buffManager.FindBuff("PoleCommander.speed");
                if (buff is StatsIncreased stats)
                {
                    float reduction = Math.Clamp((stats.value + 1f) * 0.3f, 0f, 0.95f);
                    actualDamage = (1f - reduction) * actualDamage;
                }
            }

            Flashing(0.2f, 4f, "body");
            healthPoint -= actualDamage;
            if (healthPoint < 0f) healthPoint = 0f;

            if (actualDamage >= 300f && (actualDamage >= maxHealthPoint / 3f || actualDamage >= 1800f))
            {
                var position = transform.position;
                position.y = fZ + fY + 134f;
                DamageText.CreatDamageText(actualDamage, 0, position);
            }

            return InjuryStatusUpdate_Body(damage.noResidue);
        }

        private float Hurt_FinalDamageReduction(float originalDamagePoint, DamageAttribute damageAttribute)
        {
            if (damageAttribute == DamageAttribute.real) return originalDamagePoint;
            if (ID != 3 || !poleZombie_pole || isStant || stiffnessTime > 0f) return originalDamagePoint;
            var buff = buffManager.FindBuff("PoleCommander.speed");
            if (buff is not StatsIncreased stats) return originalDamagePoint;
            float reduction = Math.Clamp((stats.value + 1f) * 0.3f, 0f, 0.95f);
            return (1f - reduction) * originalDamagePoint;
        }

        private bool Hurt_Normal(Damage damage, Projectile projectile)
        {
            float remainder = damage.damagePoint;
            if (!damage.acrossArmor2) remainder = Hurt_Armor2(damage, projectile);
            if (remainder > 0f)
            {
                foreach (var element in damage.elements)
                {
                    _ = Math.Sign(element.point);
                    if (element.type == ElementType.fire_ice && armor1Type == Armor1Type.iceCube && armor1Point > 0f)
                        continue;
                    if (element.type == ElementType.fire_ice)
                        elementManager.ToEffect(element);
                }
            }
            if (!damage.acrossArmor1) remainder = Hurt_Armor1(damage, projectile);
            if (remainder <= 0f) return false;
            damage.damagePoint = remainder;
            return Hurt_Body(damage, projectile);
        }

        private bool Hurt_Real(Damage damage, Projectile projectile)
        {
            if (!damage.acrossArmor2) Hurt_Armor2(damage, projectile);
            foreach (var element in damage.elements)
            {
                _ = Math.Sign(element.point);
                if (element.type == ElementType.fire_ice && armor1Type == Armor1Type.iceCube && armor1Point > 0f)
                    continue;
                if (element.type == ElementType.fire_ice)
                    elementManager.ToEffect(element);
            }
            if (!damage.acrossArmor1) Hurt_Armor1(damage, projectile);
            return Hurt_Body(damage, projectile);
        }

        private bool Hurt_Throughout(Damage damage, Projectile projectile)
        {
            float originalDamage = damage.damagePoint;
            float remainder = originalDamage;
            if (!damage.acrossArmor2) remainder = Hurt_Armor2(damage, projectile);
            if (remainder > 0f)
            {
                foreach (var element in damage.elements)
                {
                    _ = Math.Sign(element.point);
                    if (element.type == ElementType.fire_ice && armor1Type == Armor1Type.iceCube && armor1Point > 0f)
                        continue;
                    if (element.type == ElementType.fire_ice)
                        elementManager.ToEffect(element);
                }
            }
            if (!damage.acrossArmor1) remainder = Hurt_Armor1(damage, projectile);
            if (remainder <= 0f) return false;
            damage.damagePoint = remainder;
            bool result = Hurt_Body(damage, projectile);
            damage.damagePoint = originalDamage;
            return result;
        }
    }
}
