using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine
{
    public class Object
    {
        public string name { get; set; } = "";
        public static bool operator ==(Object? a, Object? b) => ReferenceEquals(a, b);
        public static bool operator !=(Object? a, Object? b) => !ReferenceEquals(a, b);
        public static implicit operator bool(Object? a) => !ReferenceEquals(a, null);
        public override bool Equals(object? obj) => ReferenceEquals(this, obj);
        public override int GetHashCode() => base.GetHashCode();
    }

    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    }

    public struct Color
    {
        public float r;
        public float g;
        public float b;
        public float a;
        public Color(float r, float g, float b, float a) { this.r = r; this.g = g; this.b = b; this.a = a; }
    }

    public class Transform : Object
    {
        public Vector3 position { get; set; }
        public Vector3 localScale { get; set; }
        public void SetParent(Transform parent, bool worldPositionStays) { }
    }

    public class GameObject : Object
    {
        public Transform transform => null!;
        public T AddComponent<T>() where T : Component => null!;
        public void SetActive(bool value) { }
    }

    public class Component : Object
    {
        public Transform transform => null!;
        public GameObject gameObject => null!;
    }

    public class Behaviour : Component { }
    public class MonoBehaviour : Behaviour { }
    public class Sprite : Object { }
    public class Material : Object { public void SetFloat(string name, float value) { } }
    public class SpriteRenderer : Component
    {
        public string sortingLayerName { get; set; } = "";
        public Color color { get; set; }
        public Sprite sprite { get; set; } = null!;
    }
    public static class Time { public static float deltaTime => 0f; }
    public static class Debug { public static void Log(object message) { } }
}

namespace UnityEngine.UI
{
    public class Image : UnityEngine.Behaviour
    {
        public UnityEngine.Material material => null!;
        public float fillAmount { get; set; }
    }
}

namespace Template
{
    public enum ElementType { fire_ice = 0 }
    public enum BuffType { Routine = 0 }

    public class Buff
    {
        public virtual void Awake(bool child) { }
    }

    public class StatsIncreased : Buff
    {
        public string name = "";
        public BuffType buffType;
        public float duration;
        public float value;
        public List<StatsIncreased> childBuffs = null!;
        public Plant originalPlant = null!;
        public Zombie originalZombie = null!;
        public StatsIncreased(string stat, float value, bool multi) { }
    }

    public class BuffManager
    {
        public List<Buff> buffs_toAdd = null!;
        public float GetIncrement(float value, string name) => 0f;

        public void AddBuff(Buff buff)
        {
            buffs_toAdd.Add(buff);
            buff.Awake(false);
        }
    }

    public class BoardEntry
    {
        public string key = "";
    }

    public static class ResourceManager
    {
        public static List<Sprite> vfxSprites = null!;
    }

    public class Element
    {
        public ElementManager manager = null!;
        public ElementType type;
        public float point;
        public ElementUIController UIController = null!;
        public float burstTime;
        public GameObject burstVFX = null!;
        public bool shuttle_able;
        public float decaySpeedE;

        public void Shuttle() { }
        public void BurstEnd() { }

        public void Burst()
        {
            burstTime = 10f;
            int sign = Math.Sign(point);
            point = sign * 15000f;

            if (manager.plant)
            {
                Plant plant = manager.plant;
                byte preference = manager.plant.GetElementPreference(type);
                if (sign != preference)
                {
                    if (type == ElementType.fire_ice)
                    {
                        if (burstVFX == null)
                        {
                            burstVFX = new GameObject();
                            SpriteRenderer renderer = burstVFX.AddComponent<SpriteRenderer>();
                            renderer.sortingLayerName = "Particles";
                            renderer.color = new Color(1f, 1f, 1f, 0.75f);
                            renderer.sprite = ResourceManager.vfxSprites[0];
                            burstVFX.transform.localScale = new Vector3(65f, 65f, 1f);
                            Vector3 position = plant.transform.position;
                            position.y += plant.fZ + 30f;
                            burstVFX.transform.position = position;
                            burstVFX.transform.SetParent(plant.animationGroup.transform, true);
                        }

                        StatsIncreased parent = new StatsIncreased("Arm", 20f, false);
                        parent.name = "ice_decreaseDEF";
                        parent.buffType = BuffType.Routine;
                        parent.duration = 10f;
                        StatsIncreased child = new StatsIncreased("Def", -0.5f, true);
                        child.originalPlant = plant;
                        if (ElementManager.GetEffectBoardEntry(ElementType.fire_ice, 1) != null)
                            child.value -= 0.5f;
                        parent.childBuffs.Add(child);
                        plant.buffManager.AddBuff(parent);
                    }
                }
                else
                {
                    manager.plant.ElementLevelUp(type, sign);
                }
            }

            if (manager.zombie)
            {
                Zombie zombie = manager.zombie;
                if (type == ElementType.fire_ice)
                {
                    if (burstVFX == null)
                    {
                        burstVFX = new GameObject();
                        SpriteRenderer renderer = burstVFX.AddComponent<SpriteRenderer>();
                        renderer.sortingLayerName = "Particles";
                        renderer.color = new Color(1f, 1f, 1f, 0.75f);
                        renderer.sprite = ResourceManager.vfxSprites[6];
                        burstVFX.transform.localScale = new Vector3(142.5f, 142.5f, 1f);
                        Vector3 position = zombie.transform.position;
                        position.y += zombie.fZ + 8f;
                        burstVFX.transform.position = position;
                        burstVFX.transform.SetParent(zombie.transform, true);
                    }

                    StatsIncreased parent = new StatsIncreased("Tou", -15f, false);
                    parent.name = "ice_decreaseDEF";
                    parent.buffType = BuffType.Routine;
                    parent.duration = 10f;
                    StatsIncreased child = new StatsIncreased("Def", -0.5f, true);
                    child.originalZombie = zombie;
                    parent.childBuffs.Add(child);
                    zombie.buffManager.AddBuff(parent);
                }
            }
        }

        public void Decay(int preference)
        {
            if (burstTime > 0f)
            {
                burstTime -= Time.deltaTime;
                if (!(burstTime > 0f))
                    BurstEnd();
                return;
            }

            if (point == 0f)
                return;
            if (point > 0f && preference == 1)
                return;
            if (point < 0f && preference == -1)
                return;

            int sign = Math.Sign(point);
            float signedDecay = sign * decaySpeedE;
            float rate = sign * -100f;
            if (signedDecay < 0f)
                rate += decaySpeedE;
            point += Time.deltaTime * rate;
            if (sign != Math.Sign(point))
                point = 0f;
            decaySpeedE = 0f;
        }
    }

    public class ElementManager
    {
        public Zombie zombie = null!;
        public Plant plant = null!;
        public List<Element> elements = null!;
        public List<Element> elements_toEffect = null!;
        public static List<BoardEntry> boardEntries = null!;

        public static BoardEntry? GetEffectBoardEntry(ElementType elementType, int sign)
        {
            string key = string.Empty;
            if (sign > 0)
            {
                if (elementType == ElementType.fire_ice)
                    key = "ice";
            }
            else if (sign < 0 && elementType == ElementType.fire_ice)
            {
                key = "";
            }

            foreach (BoardEntry entry in boardEntries)
            {
                if (entry.key == key)
                    return entry;
            }
            return null;
        }

        public void Effect(Element element)
        {
            float resistance = 0f;
            if (zombie != null)
                resistance = zombie.GetER();
            if (plant != null)
                resistance = plant.GetER();
            float incoming = element.point * (1f - resistance * 0.01f);

            foreach (Element existing in elements)
            {
                if (existing.type != element.type)
                    continue;
                if (existing.burstTime > 0f)
                    return;

                int oldSign = Math.Sign(existing.point);
                existing.point += incoming;
                int newSign = Math.Sign(existing.point);
                if (oldSign * newSign < 0)
                {
                    if (!element.shuttle_able)
                        existing.point = 0f;
                    else
                        existing.Shuttle();
                }
                if (MathF.Abs(existing.point) >= 15000f)
                    existing.Burst();
                return;
            }
        }

        public void Update()
        {
            foreach (Element element in elements_toEffect)
                Effect(element);
            elements_toEffect.Clear();

            foreach (Element element in elements)
            {
                if (plant)
                    element.Decay(plant.GetElementPreference(element.type));
                if (zombie)
                    element.Decay(0);
            }

            foreach (Element element in elements)
            {
                if ((object)element.UIController != null)
                    element.UIController.UpdateUI(element);
            }
        }
    }

    public class ElementUIController : MonoBehaviour
    {
        public ElementType type;
        public Image back1 = null!;
        public Image image1 = null!;
        public Image back2 = null!;
        public Image image2 = null!;
        public float progress;

        public void UpdateUI(Element element)
        {
            float brightness = element.burstTime > 0f ? 4f : 1f;
            if (image1)
                image1.material.SetFloat("_Brightness", brightness);
            if (image2)
                image2.material.SetFloat("_Brightness", brightness);

            float nextProgress = element.point / 15000f;
            if (nextProgress == progress)
                return;
            progress = nextProgress;

            if (element.point == 0f)
            {
                if ((object)back1 != null) back1.gameObject.SetActive(false);
                if ((object)image1 != null) image1.gameObject.SetActive(false);
                if ((object)back2 != null) back2.gameObject.SetActive(false);
                if ((object)image2 != null) image2.gameObject.SetActive(false);
                return;
            }

            if (element.point > 0f)
            {
                if ((object)back1 != null) back1.gameObject.SetActive(true);
                if (image1)
                {
                    image1.gameObject.SetActive(true);
                    image1.fillAmount = progress;
                }
                if ((object)back2 != null) back2.gameObject.SetActive(false);
                if ((object)image2 != null) image2.gameObject.SetActive(false);
                return;
            }

            if (element.point < 0f)
            {
                if ((object)back2 != null) back2.gameObject.SetActive(true);
                if (image2)
                {
                    image2.gameObject.SetActive(true);
                    image2.fillAmount = progress;
                }
                if ((object)back1 != null) back1.gameObject.SetActive(false);
                if ((object)image1 != null) image1.gameObject.SetActive(false);
            }
        }
    }

    public static class GlobalStaticVars
    {
        public static GameObject? GetAnimationSprite_Name(List<GameObject> animationSprites, string spriteName)
        {
            foreach (GameObject sprite in animationSprites)
            {
                if (sprite.name == spriteName)
                    return sprite;
            }
            return null;
        }

        public static void AppearSprite(List<GameObject> animationSprites, string name)
        {
            GameObject? sprite = GetAnimationSprite_Name(animationSprites, name);
            if (sprite)
                sprite.SetActive(true);
        }

        public static void HideSprite(List<GameObject> animationSprites, string name)
        {
            GameObject? sprite = GetAnimationSprite_Name(animationSprites, name);
            if (sprite)
                sprite.SetActive(false);
        }
    }

    public class Plant : MonoBehaviour
    {
        public int ID;
        public float fZ;
        public float armorPoint;
        public float defensePoint;
        public float attackPoint;
        public float maxHealthPoint;
        public float healthPoint;
        public float elementalResistance;
        public int elementLv;
        public float talentMaxTicking;
        public GameObject animationGroup = null!;
        public List<GameObject> animationSprites = null!;
        public BuffManager buffManager = null!;

        public float GetER()
        {
            return Math.Clamp(MathF.Round(buffManager.GetIncrement(elementalResistance, "ER") + elementalResistance), 0f, 100f);
        }

        public byte GetElementPreference(ElementType elementType)
        {
            if (elementType == ElementType.fire_ice)
                return ID == 5 ? (byte)1 : (byte)0;
            return 0;
        }

        public void ElementLevelUp(ElementType elementType, int sign)
        {
            Debug.Log("LevelUp");
            if (elementLv >= 3)
                return;
            elementLv++;

            if (elementType == ElementType.fire_ice && sign == 1)
            {
                armorPoint += 5f;
                defensePoint *= 0.9f;
                attackPoint *= 1.15f;
                maxHealthPoint *= 1.1f;
                healthPoint *= 1.1f;
            }

            if (ID != 5)
                return;

            switch (elementLv)
            {
                case 0:
                    talentMaxTicking = 0f;
                    break;
                case 1:
                    talentMaxTicking = 0f;
                    GlobalStaticVars.AppearSprite(animationSprites, "SnowPea_1");
                    GlobalStaticVars.AppearSprite(animationSprites, "SnowPea_5");
                    GlobalStaticVars.AppearSprite(animationSprites, "SnowPea_7");
                    GlobalStaticVars.HideSprite(animationSprites, "SnowPea_crystals2");
                    break;
                case 2:
                    talentMaxTicking = 8f;
                    GlobalStaticVars.AppearSprite(animationSprites, "SnowPea_8");
                    GlobalStaticVars.AppearSprite(animationSprites, "SnowPea_11");
                    break;
                case 3:
                    talentMaxTicking = 8f;
                    GlobalStaticVars.AppearSprite(animationSprites, "SnowPea_3");
                    GlobalStaticVars.AppearSprite(animationSprites, "SnowPea_6");
                    GlobalStaticVars.AppearSprite(animationSprites, "SnowPea_9");
                    GlobalStaticVars.AppearSprite(animationSprites, "SnowPea_10");
                    GlobalStaticVars.HideSprite(animationSprites, "SnowPea_11");
                    break;
            }
        }
    }

    public class Zombie : MonoBehaviour
    {
        public float fZ;
        public float elementalResistance;
        public BuffManager buffManager = null!;

        public float GetER()
        {
            return Math.Clamp(MathF.Round(buffManager.GetIncrement(elementalResistance, "ER") + elementalResistance), 0f, 100f);
        }
    }
}
