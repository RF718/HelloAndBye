using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;
using TMPro;

public class SpriteSwitcher : MonoBehaviour
{
    public Image targetImage; // 要切换sprite的Image组件
    public Sprite sprite1; // 第一张图片
    public Sprite sprite2; // 第二张图片
    public Sprite[] spriteGroup; // 一组图片

    public float switchInterval = 0.2f; // 切换时间间隔
    public float initialDuration = 3f; // 两张图片切换的持续时间
    public float groupSwitchDuration = 5f; // 图片组切换的持续时间

    //public GameObject text;

    private bool isSwitching = false;
    private IEnumerator switchCoro;
    public bool start = false;

    void OnEnable()
    {
        start = true;
        MainStageManager.instance.scenesManager.SetGameStartFlag(true);
    }

    private void OnDisable()
    {
        start = false;
        isSwitching = false;
        if (switchCoro != null)
        {
            StopCoroutine(switchCoro);
            switchCoro = null;
        }
        MainStageManager.instance.scenesManager.SetGameStartFlag(false);
    }

    private void Update()
    {
        if (start&&!isSwitching)
        {
            start = false;
            switchCoro = SwitchSprites();
            StartCoroutine(switchCoro);
        }
    }

    IEnumerator SwitchSprites()
    {
        float elapsedTime = 0f;
        bool useFirstPair = true; // 先使用两张图片来回切换

        while (elapsedTime < initialDuration)
        {
            // 切换两张图片
            targetImage.sprite = useFirstPair ? sprite1 : sprite2;
            useFirstPair = !useFirstPair;

            elapsedTime += switchInterval;
            yield return new WaitForSeconds(switchInterval);
        }

        // 切换到图片组
        int index = 0;
        float groupElapsedTime = 0f;
        while (groupElapsedTime < groupSwitchDuration)
        {
            targetImage.sprite = spriteGroup[index];
            index = (index + 1) % spriteGroup.Length; // 循环切换图片组

            //if (groupElapsedTime+1>=groupSwitchDuration)
            //    text.gameObject.SetActive(true);
            groupElapsedTime += switchInterval;

            yield return new WaitForSeconds(switchInterval);
        }

        // 最后可以根据需要停止或继续循环
        isSwitching = false;

    }
}
