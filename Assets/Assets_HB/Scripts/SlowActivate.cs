using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlowActivate : MonoBehaviour
{
    public Image targetImage; // 需要改变透明度的Image
    public float fadeInDuration = 2.0f; // 总的淡入时间
    public float shakeDuration = 0.2f; // 抖动持续时间
    public float shakeMagnitude = 10f; // 抖动幅度

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = targetImage.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        // 当GameObject激活时，开始淡入效果
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = targetImage.color;

        // 透明度从0到1的淡入效果
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Pow(elapsedTime / fadeInDuration, 2);
            color.a = Mathf.Lerp(0, 1, t);
            targetImage.color = color;

            yield return null;
        }

        // 确保最终完全不透明
        color.a = 1f;
        targetImage.color = color;

        // 开始抖动效果
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

        // 恢复到原始位置
        rectTransform.anchoredPosition = originalPosition;
    }
}