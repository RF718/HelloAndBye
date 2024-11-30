using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CloudMover : MonoBehaviour
{
    public RectTransform cloud;         // 乌云的RectTransform
    public Image background;            // 背景的Image组件
    public Vector3 startPosition;       // 乌云的初始位置
    public Vector3 endPosition;         // 乌云移动的终点（背景中线）
    public float moveDuration = 2.0f;   // 移动的持续时间
    public float darkenAmount = 0.2f;   // 背景变暗的幅度（0到1，表示减少多少亮度）

    private Color originalBackgroundColor; // 背景的原始颜色

    private void OnEnable()
    {
        // 获取背景的原始颜色
        originalBackgroundColor = background.color;

        // 开始乌云移动和平滑变暗的协程
        cloud.anchoredPosition = startPosition; // 设置乌云的初始位置
        StartCoroutine(MoveCloudAndDimBackground());
    }

    IEnumerator MoveCloudAndDimBackground()
    {
        float elapsedTime = 0f;

        // 平滑移动乌云和变暗背景
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;

            // 移动乌云从初始位置到终点位置
            cloud.anchoredPosition = Vector3.Lerp(startPosition, endPosition, t);

            // 背景变暗，通过减少RGB的亮度，但保持透明度（Alpha）
            Color newColor = originalBackgroundColor * (1 - darkenAmount * t);
            newColor.a = originalBackgroundColor.a; // 保持透明度不变
            background.color = newColor;

            yield return null;
        }

        // 确保最终位置和颜色正确
        cloud.anchoredPosition = endPosition;

        // 最终使背景稍微变暗，但保持可见
        Color finalColor = originalBackgroundColor * (1 - darkenAmount);
        finalColor.a = originalBackgroundColor.a; // 保持透明度不变
        background.color = finalColor;
    }
}
