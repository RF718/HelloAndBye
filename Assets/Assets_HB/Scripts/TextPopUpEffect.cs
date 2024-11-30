using System.Collections;
using UnityEngine;
using TMPro;

public class TextPopUpEffect : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // TextMeshPro组件
    public float animationDuration = 1.0f; // 动画持续时间
    public float maxScale = 1.5f; // 最大缩放比例
    public float bounceScale = 0.9f; // 反弹时缩小到的比例
    public float bounceDuration = 0.2f; // 反弹缩小的持续时间

    private Vector3 originalScale; // Text的初始大小

    void OnEnable()
    {
        originalScale = textMeshPro.rectTransform.localScale;
        textMeshPro.rectTransform.localScale = Vector3.zero; // 初始设置为非常小的状态
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        float elapsedTime = 0f;

        // 第一步：从小变大到最大尺寸
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            // 逐渐从小放大到指定最大比例
            textMeshPro.rectTransform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(maxScale, maxScale, maxScale), Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        // 第二步：反弹效果，缩小一点
        elapsedTime = 0f;
        while (elapsedTime < bounceDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / bounceDuration;

            // 从最大缩放到反弹的缩小比例
            textMeshPro.rectTransform.localScale = Vector3.Lerp(new Vector3(maxScale, maxScale, maxScale), new Vector3(bounceScale * maxScale, bounceScale * maxScale, bounceScale * maxScale), t);
            yield return null;
        }

        // 第三步：恢复到正常的最大比例
        textMeshPro.rectTransform.localScale = new Vector3(originalScale.x * maxScale, originalScale.y * maxScale, originalScale.z * maxScale);
    }
}
