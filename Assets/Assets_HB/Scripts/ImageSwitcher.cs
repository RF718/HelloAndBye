using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageSwitcher : MonoBehaviour
{
    public Image targetImage;           // ��Ҫ�л�ͼƬ��UI Image
    public Sprite[] sprites;            // ͼƬ����
    public float switchInterval = 1.0f; // ͼƬ�л��ļ��ʱ�䣨�룩

    private int currentIndex = 0;       // ��ǰ��ʾ��ͼƬ����

    void OnEnable()
    {
        if (sprites.Length > 0)
        {
            // ��ʼ�л�ͼƬ��Э��
            StartCoroutine(SwitchImages());
        }
    }

    IEnumerator SwitchImages()
    {
        while (true)
        {
            // �л�����һ��ͼƬ
            targetImage.sprite = sprites[currentIndex];

            // ����������ȷ��ѭ���ص���һ��ͼƬ
            currentIndex = (currentIndex + 1) % sprites.Length;

            // �ȴ�ָ����ʱ����
            yield return new WaitForSeconds(switchInterval);
        }
    }
}