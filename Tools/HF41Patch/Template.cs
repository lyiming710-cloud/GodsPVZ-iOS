using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public class Object
    {
        public static bool operator ==(Object? a, Object? b) => ReferenceEquals(a, b);
        public static bool operator !=(Object? a, Object? b) => !ReferenceEquals(a, b);
        public static implicit operator bool(Object? a) => !ReferenceEquals(a, null);
        public override bool Equals(object? obj) => ReferenceEquals(this, obj);
        public override int GetHashCode() => base.GetHashCode();
    }
    public struct Color
    {
        public float r, g, b, a;
        public Color(float r, float g, float b, float a) { this.r=r; this.g=g; this.b=b; this.a=a; }
    }
    public class GameObject : Object
    {
        public T GetComponent<T>() where T : Component => null!;
    }
    public class Component : Object { public GameObject gameObject => null!; }
    public class Behaviour : Component { }
    public class MonoBehaviour : Behaviour { }
    public class Material : Object { public void SetFloat(string name, float value) { } }
    public class Renderer : Component { public Material material => null!; }
    public class SpriteRenderer : Renderer { public Color color { get; set; } }
    public class AudioSource : Behaviour { public float volume { get; set; } }
    public static class Time { public static float fixedDeltaTime => 0f; }
}

namespace Template
{
    public enum ElementType { fire_ice = 0 }

    public class Element
    {
        public float point;
    }

    public class Board : MonoBehaviour
    {
        public bool gameStart;
        public AudioSource[] audioSource = null!;
    }

    public static class GlobalStaticVars
    {
        public static float AudioVolume() => 0f;
        public static float BGMVolume() => 0f;
    }

    public class Plant : MonoBehaviour
    {
        public int ID;
        public byte GetElementPreference(ElementType elementType) => 0;
    }

    public class ElementManager
    {
        public Zombie zombie = null!;
        public Plant plant = null!;
        public Element? GetElement(ElementType elementType) => null;

        public Color GetElementColor()
        {
            Color result = new Color(1f, 1f, 1f, 1f);
            if (zombie != null)
            {
                Element? element = GetElement(ElementType.fire_ice);
                if (element != null && element.point > 0f)
                {
                    float t = MathF.Ceiling(element.point / 1000f) * 0.05f;
                    if (t < 0f) t = 0f;
                    else if (t > 1f) t = 1f;
                    result.r = 0.5f + t * -0.5f;
                    result.g = 0.8f + t * -0.8f;
                    return result;
                }
            }

            if (plant != null)
            {
                Element? element = GetElement(ElementType.fire_ice);
                if (element != null && element.point > 0f &&
                    plant.GetElementPreference(ElementType.fire_ice) != 1 && plant.ID != 6)
                {
                    float t = MathF.Ceiling(element.point / 1000f) * 0.05f;
                    if (t < 0f) t = 0f;
                    else if (t > 1f) t = 1f;
                    result.r = 0.2f + t * 0.1f;
                    result.g = 0.88f + t * -0.58f;
                }
            }
            return result;
        }
    }

    public class Zombie : MonoBehaviour
    {
        public Board board = null!;
        public int ID;
        public float healthPoint;
        public bool isDying;
        public bool isDied;
        public bool ashes;
        public bool isOnBoard;
        public ElementManager elementManager = null!;
        private AudioSource audioSource_BGM = null!;
        private List<GameObject> animationSprites = null!;
        public Color color;
        private static readonly string Mat_Alpha = "";

        public bool InjuryStatusUpdate_Body(bool noResidue) => false;

        private void Update_Color()
        {
            float r = color.r;
            float g = color.g;
            float b = color.b;
            float alpha = color.a;
            if (!ashes)
            {
                Color elementColor = elementManager.GetElementColor();
                r *= elementColor.r;
                g *= elementColor.g;
                b *= elementColor.b;
            }
            else
            {
                r = 0f;
                g = 0f;
                b = 0f;
            }

            foreach (GameObject sprite in animationSprites)
            {
                SpriteRenderer renderer = sprite.GetComponent<SpriteRenderer>();
                if (renderer == null)
                    continue;
                Color old = renderer.color;
                renderer.color = new Color(r, g, b, old.a);
                renderer.material.SetFloat(Mat_Alpha, alpha);
            }
            color = new Color(1f, 1f, 1f, 1f);
        }

        private void FixedUpdate_BGM()
        {
            if (!audioSource_BGM)
                return;

            float volume = GlobalStaticVars.AudioVolume();
            if (ID == 17)
            {
                volume *= 2.5f;
            }
            else if (ID == 18 && board != null)
            {
                if (!isDied)
                {
                    float current = audioSource_BGM.volume;
                    volume = current;
                    if (GlobalStaticVars.BGMVolume() * 1.5f > current)
                        volume = current + GlobalStaticVars.BGMVolume() * 0.02f * 0.4f;

                    if (board.audioSource[0].volume > 0f)
                        board.audioSource[0].volume = board.audioSource[0].volume - GlobalStaticVars.BGMVolume() * 0.02f * 0.5f;
                }
                else
                {
                    float current = audioSource_BGM.volume;
                    volume = current;
                    if (current > 0f)
                        volume = current - GlobalStaticVars.BGMVolume() * 0.02f * 0.4f;

                    if (GlobalStaticVars.BGMVolume() > board.audioSource[0].volume)
                        board.audioSource[0].volume = board.audioSource[0].volume + GlobalStaticVars.BGMVolume() * 0.02f * 0.4f;
                }
            }
            audioSource_BGM.volume = volume;
        }

        private void FixedUpdate()
        {
            Update_Color();
            FixedUpdate_BGM();
            if (isOnBoard && board.gameStart && isDying && !isDied)
            {
                healthPoint -= Time.fixedDeltaTime * 100f;
                InjuryStatusUpdate_Body(false);
            }
        }
    }
}
