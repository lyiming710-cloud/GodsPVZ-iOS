using System;
using UnityEngine;

namespace UnityEngine
{
    public class Object
    {
        public static implicit operator bool(Object? value) => !ReferenceEquals(value, null);
    }
    public class Component : Object { }
    public class Behaviour : Component { }
    public class MonoBehaviour : Behaviour { }
    public class Animator : Behaviour
    {
        public float GetFloat(int id) => 0f;
        public void SetFloat(int id, float value) { }
    }
    public static class Random
    {
        public static float Range(float minInclusive, float maxInclusive) => 0f;
    }
}

namespace Template
{
    public enum ElementType { fire_ice = 0 }

    public class Element
    {
        public float point;
        public float burstTime;
    }

    public class ElementManager
    {
        public Element? GetElement(ElementType type) => null;
    }

    public class BuffManager
    {
        public float GetIncrement(float baseValue, string stat) => 0f;
    }

    public class Plant : MonoBehaviour
    {
        public int ID;
        public float updateRate;
        public Animator animator = null!;
        public BuffManager buffManager = null!;
        public ElementManager elementManager = null!;
        private static int Ani_SpeedHash;
        private static int Ani_AttackSpeedHash;

        public float GetAS() => 0f;

        private void ResetUpdateRate(float newUpdateRate)
        {
            if (animator)
            {
                if (updateRate == 0f)
                {
                    updateRate = newUpdateRate;
                    float increment = buffManager.GetIncrement(1f, "Is");
                    float speed = Math.Max(increment + 1f, 0.05f) * updateRate;
                    animator.SetFloat(Ani_SpeedHash, speed);
                    animator.SetFloat(Ani_AttackSpeedHash, GetAS());
                }
                else
                {
                    float oldSpeed = animator.GetFloat(Ani_SpeedHash);
                    animator.SetFloat(Ani_SpeedHash, newUpdateRate / updateRate * oldSpeed);
                    float oldAttackSpeed = animator.GetFloat(Ani_AttackSpeedHash);
                    animator.SetFloat(Ani_AttackSpeedHash, newUpdateRate / updateRate * oldAttackSpeed);
                }
            }
            updateRate = newUpdateRate;
        }

        public void SetUpdateRate()
        {
            Element? element = elementManager.GetElement(ElementType.fire_ice);
            float targetRate = 1f;
            if (element != null && ID != 5 && element.point > 0f)
            {
                if (element.burstTime > 0f)
                {
                    if (updateRate != 0f && animator)
                    {
                        float oldSpeed = animator.GetFloat(Ani_SpeedHash);
                        float attackSpeed = GetAS();
                        float attackFactor = 0f / updateRate;
                        float speedFactor = 0f / updateRate;
                        animator.SetFloat(Ani_SpeedHash, speedFactor * oldSpeed);
                        animator.SetFloat(Ani_AttackSpeedHash, attackFactor * attackSpeed);
                    }
                    updateRate = 0f;
                    return;
                }

                if (ID != 6)
                {
                    float scaled = MathF.Ceiling(element.point / 1000f) * 0.05f;
                    targetRate = Math.Max(0.05f, 1f - scaled);
                }
            }

            if (updateRate != targetRate)
                ResetUpdateRate(targetRate);
            updateRate = targetRate;
        }
    }

    public class Zombie : MonoBehaviour
    {
        public int ID;

        public float GetRandenAnimationSpeedMagnification()
        {
            switch (ID)
            {
                case 0:
                case 2:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 11:
                case 12:
                case 14:
                    return UnityEngine.Random.Range(0.75f, 1.3f);
                default:
                    return 1f;
            }
        }
    }
}
