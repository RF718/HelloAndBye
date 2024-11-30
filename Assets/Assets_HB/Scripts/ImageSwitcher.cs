using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageSwitcher : MonoBehaviour
{
    public Image targetImage;           // 需要切换图片的UI Image
    public Sprite[] sprites;            // 图片数组
    public float switchInterval = 1.0f; // 图片切换的间隔时间（秒）

    private int currentIndex = 0;       // 当前显示的图片索引

    void OnEnable()
    {
        if (sprites.Length > 0)
        {
            // 开始切换图片的协程
            StartCoroutine(SwitchImages());
        }
    }

    IEnumerator SwitchImages()
    {
        while (true)
        {
            // 切换到下一个图片
            targetImage.sprite = sprites[currentIndex];

            // 更新索引，确保循环回到第一个图片
            currentIndex = (currentIndex + 1) % sprites.Length;

            // 等待指定的时间间隔
            yield return new WaitForSeconds(switchInterval);
        }
    }
}