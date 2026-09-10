using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine
{
    public class Object
    {
        public static implicit operator bool(Object? value) => !ReferenceEquals(value, null);
    }

    public class Component : Object
    {
        public Transform transform = null!;
    }

    public class Behaviour : Component { }
    public class MonoBehaviour : Behaviour { }

    public class GameObject : Object
    {
        public Transform transform = null!;
    }

    public class Transform : Component
    {
        public Vector3 localPosition { get; set; }
        public Transform GetChild(int index) => null!;
    }

    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
    }

    public static class Time
    {
        public static float deltaTime => 0f;
    }
}

namespace UnityEngine.UI
{
    public class Image : UnityEngine.Behaviour
    {
        public float fillAmount { get; set; }
    }
}

namespace TMPro
{
    public class TextMeshProUGUI : UnityEngine.Behaviour
    {
        public void SetText(string sourceText, bool syncTextInputBox = true) { }
    }
}

namespace Template
{
    public class FlagMeter : MonoBehaviour
    {
        private float progress;
        public int flagsNum;
        public int wavesNum;
        public int theFlagID;
        private Image flagMeter1 = null!;
        private Image flagMeter_Head = null!;
        private GameObject flagMeter_Flags = null!;
        private List<GameObject> flagMeter_FlagList = null!;
        private TextMeshProUGUI boardName = null!;
        private TextMeshProUGUI text_currentWave = null!;

        private void Update()
        {
            flagMeter1.fillAmount = progress;
            float targetX = 218f - progress * 436f;

            Transform headTransform = flagMeter_Head.transform;
            Vector3 headPosition = headTransform.localPosition;
            if (headPosition.x > targetX)
            {
                headPosition.x -= Time.deltaTime * 36f;
                headTransform.localPosition = headPosition;
            }

            if (theFlagID >= 0)
            {
                GameObject flag = flagMeter_FlagList[theFlagID];
                Transform child = flag.transform.GetChild(1);
                Vector3 flagPosition = child.localPosition;
                if (flagPosition.y <= 90f)
                {
                    flagPosition.y += Time.deltaTime * 100f;
                    child.localPosition = flagPosition;
                }
            }
        }

        public void UpdateMeter(int theFlag, int theWave)
        {
            int currentWave = theWave + 1;
            progress = (float)currentWave / (float)wavesNum;
            if (currentWave % 10 == 0)
                theFlagID = theFlag;

            text_currentWave.SetText(currentWave.ToString() + "/" + wavesNum.ToString());
        }
    }
}
