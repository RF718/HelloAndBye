using System.Collections;
using UnityEngine;
using TMPro;

public class TextPopUpEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // TextMeshPro���
    public float animationDuration = 1.0f; // ��������ʱ��
    public float maxScale = 1.5f; // ������ű���
    public float bounceScale = 0.9f; // ����ʱ��С���ı���
    public float bounceDuration = 0.2f; // ������С�ĳ���ʱ��

    private Vector3 originalScale; // Text�ĳ�ʼ��С

    void OnEnable()
    {
        originalScale = textMeshPro.rectTransform.localScale;
        textMeshPro.rectTransform.localScale = Vector3.zero; // ��ʼ����Ϊ�ǳ�С��״̬
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        float elapsedTime = 0f;

        // ��һ������С������ߴ�
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            // �𽥴�С�Ŵ�ָ��������
            textMeshPro.rectTransform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(maxScale, maxScale, maxScale), Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        // �ڶ���������Ч������Сһ��
        elapsedTime = 0f;
        while (elapsedTime < bounceDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / bounceDuration;

            // ��������ŵ���������С����
            textMeshPro.rectTransform.localScale = Vector3.Lerp(new Vector3(maxScale, maxScale, maxScale), new Vector3(bounceScale * maxScale, bounceScale * maxScale, bounceScale * maxScale), t);
            yield return null;
        }

        // ���������ָ���������������
        textMeshPro.rectTransform.localScale = new Vector3(originalScale.x * maxScale, originalScale.y * maxScale, originalScale.z * maxScale);
    }
}
