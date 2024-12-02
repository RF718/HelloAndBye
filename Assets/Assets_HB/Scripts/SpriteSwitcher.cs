using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;
using TMPro;

public class SpriteSwitcher : MonoBehaviour
{
    public Image targetImage; // Ҫ�л�sprite��Image���
    public Sprite sprite1; // ��һ��ͼƬ
    public Sprite sprite2; // �ڶ���ͼƬ
    public Sprite[] spriteGroup; // һ��ͼƬ

    public float switchInterval = 0.2f; // �л�ʱ����
    public float initialDuration = 3f; // ����ͼƬ�л��ĳ���ʱ��
    public float groupSwitchDuration = 5f; // ͼƬ���л��ĳ���ʱ��

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
        bool useFirstPair = true; // ��ʹ������ͼƬ�����л�

        while (elapsedTime < initialDuration)
        {
            // �л�����ͼƬ
            targetImage.sprite = useFirstPair ? sprite1 : sprite2;
            useFirstPair = !useFirstPair;

            elapsedTime += switchInterval;
            yield return new WaitForSeconds(switchInterval);
        }

        // �л���ͼƬ��
        int index = 0;
        float groupElapsedTime = 0f;
        while (groupElapsedTime < groupSwitchDuration)
        {
            targetImage.sprite = spriteGroup[index];
            index = (index + 1) % spriteGroup.Length; // ѭ���л�ͼƬ��

            //if (groupElapsedTime+1>=groupSwitchDuration)
            //    text.gameObject.SetActive(true);
            groupElapsedTime += switchInterval;

            yield return new WaitForSeconds(switchInterval);
        }

        // �����Ը�����Ҫֹͣ�����ѭ��
        isSwitching = false;

    }
}
