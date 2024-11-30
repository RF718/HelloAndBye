using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlowActivate : MonoBehaviour
{
    public Image targetImage; // ��Ҫ�ı�͸���ȵ�Image
    public float fadeInDuration = 2.0f; // �ܵĵ���ʱ��
    public float shakeDuration = 0.2f; // ��������ʱ��
    public float shakeMagnitude = 10f; // ��������

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = targetImage.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        // ��GameObject����ʱ����ʼ����Ч��
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = targetImage.color;

        // ͸���ȴ�0��1�ĵ���Ч��
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Pow(elapsedTime / fadeInDuration, 2);
            color.a = Mathf.Lerp(0, 1, t);
            targetImage.color = color;

            yield return null;
        }

        // ȷ��������ȫ��͸��
        color.a = 1f;
        targetImage.color = color;

        // ��ʼ����Ч��
        //yield return null;
    }

    private IEnumerator Shake()
    {
        Vector3 originalPosition = rectTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;

            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            rectTransform.anchoredPosition = new Vector3(originalPosition.x+ offsetX,originalPosition.y+offsetY,originalPosition.z);

            yield return null;
        }

        // �ָ���ԭʼλ��
        rectTransform.anchoredPosition = originalPosition;
    }
}