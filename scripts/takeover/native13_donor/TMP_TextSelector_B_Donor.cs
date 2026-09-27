using UnityEngine;
using TMPro;

namespace TMPro.Examples
{
    internal sealed class TMP_TextSelector_B_Donor
    {
        private RectTransform m_TextPopup_RectTransform;
        private TextMeshProUGUI m_TextPopup_TMPComponent;
        private const string k_LinkText = "You have selected link <#ffff00>";
        private TextMeshProUGUI m_TextMeshPro;
        private Camera m_Camera;
        private bool isHoveringObject;
        private int m_selectedWord = -1;
        private int m_selectedLink = -1;
        private int m_lastIndex = -1;
        private Matrix4x4 m_matrix;

        private void RestoreCachedVertexAttributes(int index) { }

        public void LateUpdate()
        {
            if (isHoveringObject)
            {
                int charIndex = TMP_TextUtilities.FindIntersectingCharacter(m_TextMeshPro, Input.mousePosition, m_Camera, true);

                if (charIndex == -1 || charIndex != m_lastIndex)
                {
                    RestoreCachedVertexAttributes(m_lastIndex);
                    m_lastIndex = -1;
                }

                if (charIndex != -1 && charIndex != m_lastIndex &&
                    (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                {
                    m_lastIndex = charIndex;
                    int materialIndex = m_TextMeshPro.textInfo.characterInfo[charIndex].materialReferenceIndex;
                    int vertexIndex = m_TextMeshPro.textInfo.characterInfo[charIndex].vertexIndex;
                    Vector3[] vertices = m_TextMeshPro.textInfo.meshInfo[materialIndex].vertices;
                    Vector2 charMidBasline = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2;
                    Vector3 offset = charMidBasline;

                    vertices[vertexIndex] -= offset;
                    vertices[vertexIndex + 1] -= offset;
                    vertices[vertexIndex + 2] -= offset;
                    vertices[vertexIndex + 3] -= offset;

                    m_matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 1.5f);
                    vertices[vertexIndex] = m_matrix.MultiplyPoint3x4(vertices[vertexIndex]);
                    vertices[vertexIndex + 1] = m_matrix.MultiplyPoint3x4(vertices[vertexIndex + 1]);
                    vertices[vertexIndex + 2] = m_matrix.MultiplyPoint3x4(vertices[vertexIndex + 2]);
                    vertices[vertexIndex + 3] = m_matrix.MultiplyPoint3x4(vertices[vertexIndex + 3]);

                    vertices[vertexIndex] += offset;
                    vertices[vertexIndex + 1] += offset;
                    vertices[vertexIndex + 2] += offset;
                    vertices[vertexIndex + 3] += offset;

                    Color32 c = new Color32(255, 255, 192, 255);
                    Color32[] vertexColors = m_TextMeshPro.textInfo.meshInfo[materialIndex].colors32;
                    vertexColors[vertexIndex] = c;
                    vertexColors[vertexIndex + 1] = c;
                    vertexColors[vertexIndex + 2] = c;
                    vertexColors[vertexIndex + 3] = c;

                    TMP_MeshInfo meshInfo = m_TextMeshPro.textInfo.meshInfo[materialIndex];
                    int lastVertexIndex = vertices.Length - 4;
                    meshInfo.SwapVertexData(vertexIndex, lastVertexIndex);
                    m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
                }

                int wordIndex = TMP_TextUtilities.FindIntersectingWord(m_TextMeshPro, Input.mousePosition, m_Camera);

                if (m_TextPopup_RectTransform != null && m_selectedWord != -1 &&
                    (wordIndex == -1 || wordIndex != m_selectedWord))
                {
                    TMP_WordInfo wInfo = m_TextMeshPro.textInfo.wordInfo[m_selectedWord];
                    for (int i = 0; i < wInfo.characterCount; i++)
                    {
                        int characterIndex = wInfo.firstCharacterIndex + i;
                        int meshIndex = m_TextMeshPro.textInfo.characterInfo[characterIndex].materialReferenceIndex;
                        int vertexIndex = m_TextMeshPro.textInfo.characterInfo[characterIndex].vertexIndex;
                        Color32[] vertexColors = m_TextMeshPro.textInfo.meshInfo[meshIndex].colors32;
                        Color32 c = vertexColors[vertexIndex].Tint(1.33333f);
                        vertexColors[vertexIndex] = c;
                        vertexColors[vertexIndex + 1] = c;
                        vertexColors[vertexIndex + 2] = c;
                        vertexColors[vertexIndex + 3] = c;
                    }
                    m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
                    m_selectedWord = -1;
                }

                if (wordIndex != -1 && wordIndex != m_selectedWord &&
                    !(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                {
                    m_selectedWord = wordIndex;
                    TMP_WordInfo wInfo = m_TextMeshPro.textInfo.wordInfo[wordIndex];
                    for (int i = 0; i < wInfo.characterCount; i++)
                    {
                        int characterIndex = wInfo.firstCharacterIndex + i;
                        int meshIndex = m_TextMeshPro.textInfo.characterInfo[characterIndex].materialReferenceIndex;
                        int vertexIndex = m_TextMeshPro.textInfo.characterInfo[characterIndex].vertexIndex;
                        Color32[] vertexColors = m_TextMeshPro.textInfo.meshInfo[meshIndex].colors32;
                        Color32 c = vertexColors[vertexIndex].Tint(0.75f);
                        vertexColors[vertexIndex] = c;
                        vertexColors[vertexIndex + 1] = c;
                        vertexColors[vertexIndex + 2] = c;
                        vertexColors[vertexIndex + 3] = c;
                    }
                    m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
                }

                int linkIndex = TMP_TextUtilities.FindIntersectingLink(m_TextMeshPro, Input.mousePosition, m_Camera);
                if ((linkIndex == -1 && m_selectedLink != -1) || linkIndex != m_selectedLink)
                {
                    m_TextPopup_RectTransform.gameObject.SetActive(false);
                    m_selectedLink = -1;
                }

                if (linkIndex != -1 && linkIndex != m_selectedLink)
                {
                    m_selectedLink = linkIndex;
                    TMP_LinkInfo linkInfo = m_TextMeshPro.textInfo.linkInfo[linkIndex];
                    Vector3 worldPointInRectangle;
                    RectTransformUtility.ScreenPointToWorldPointInRectangle(m_TextMeshPro.rectTransform,
                        Input.mousePosition, m_Camera, out worldPointInRectangle);

                    switch (linkInfo.GetLinkID())
                    {
                        case "id_01":
                            m_TextPopup_RectTransform.position = worldPointInRectangle;
                            m_TextPopup_RectTransform.gameObject.SetActive(true);
                            m_TextPopup_TMPComponent.text = k_LinkText + " ID 01";
                            break;
                        case "id_02":
                            m_TextPopup_RectTransform.position = worldPointInRectangle;
                            m_TextPopup_RectTransform.gameObject.SetActive(true);
                            m_TextPopup_TMPComponent.text = k_LinkText + " ID 02";
                            break;
                    }
                }
            }
            else if (m_lastIndex != -1)
            {
                RestoreCachedVertexAttributes(m_lastIndex);
                m_lastIndex = -1;
            }
        }
    }
}
