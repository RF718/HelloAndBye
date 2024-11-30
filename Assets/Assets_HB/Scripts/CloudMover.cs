using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CloudMover : MonoBehaviour
{
    public RectTransform cloud;         // ���Ƶ�RectTransform
    public Image background;            // ������Image���
    public Vector3 startPosition;       // ���Ƶĳ�ʼλ��
    public Vector3 endPosition;         // �����ƶ����յ㣨�������ߣ�
    public float moveDuration = 2.0f;   // �ƶ��ĳ���ʱ��
    public float darkenAmount = 0.2f;   // �����䰵�ķ��ȣ�0��1����ʾ���ٶ������ȣ�

    private Color originalBackgroundColor; // ������ԭʼ��ɫ

    private void OnEnable()
    {
        // ��ȡ������ԭʼ��ɫ
        originalBackgroundColor = background.color;

        // ��ʼ�����ƶ���ƽ���䰵��Э��
        cloud.anchoredPosition = startPosition; // �������Ƶĳ�ʼλ��
        StartCoroutine(MoveCloudAndDimBackground());
    }

    IEnumerator MoveCloudAndDimBackground()
    {
        float elapsedTime = 0f;

        // ƽ���ƶ����ƺͱ䰵����
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;

            // �ƶ����ƴӳ�ʼλ�õ��յ�λ��
            cloud.anchoredPosition = Vector3.Lerp(startPosition, endPosition, t);

            // �����䰵��ͨ������RGB�����ȣ�������͸���ȣ�Alpha��
            Color newColor = originalBackgroundColor * (1 - darkenAmount * t);
            newColor.a = originalBackgroundColor.a; // ����͸���Ȳ���
            background.color = newColor;

            yield return null;
        }

        // ȷ������λ�ú���ɫ��ȷ
        cloud.anchoredPosition = endPosition;

        // ����ʹ������΢�䰵�������ֿɼ�
        Color finalColor = originalBackgroundColor * (1 - darkenAmount);
        finalColor.a = originalBackgroundColor.a; // ����͸���Ȳ���
        background.color = finalColor;
    }
}
