using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeEffect : MonoBehaviour
{
    public bool shake = false; // �����Ƿ�ʼ��
    public float initialShakeFrequency = 0.1f; // ��ʼ��Ƶ��
    public float maxShakeFrequency = 5f; // �����Ƶ��
    public float shakeDuration = 5f; // �𶯳���ʱ��
    public float shakeMagnitude = 5f; // �𶯷���
    public float rotationMagnitude = 5f; // ��ת�仯�����Ƕ�
    public float individualVariance = 0.5f; // ÿ���������Ƶ�ʲ���

    private List<RectTransform> childrenTransforms = new List<RectTransform>();
    private Dictionary<RectTransform, Vector2> originalPositions = new Dictionary<RectTransform, Vector2>();
    private Dictionary<RectTransform, Quaternion> originalRotations = new Dictionary<RectTransform, Quaternion>();
    private bool isShaking = false;
    private IEnumerator ShakeCorou;

    void Start()
    {
        // ��ȡ�����������RectTransform����¼ԭʼλ�ú���ת
        foreach (Transform child in transform)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                childrenTransforms.Add(rectTransform);
                originalPositions[rectTransform] = rectTransform.anchoredPosition; // ��¼��ʼλ��
                originalRotations[rectTransform] = rectTransform.localRotation;    // ��¼��ʼ��ת
            }
        }
    }

    void Update()
    {
        if (shake && !isShaking)
        {
            ShakeCorou = StartShaking();
            StartCoroutine(ShakeCorou);
        }
        
        if (!shake && isShaking && ShakeCorou != null)
        {
            StopCoroutine(ShakeCorou);
            ShakeCorou =null;
            foreach (RectTransform rectTransform in childrenTransforms)
            {
                rectTransform.anchoredPosition = originalPositions[rectTransform];
                rectTransform.localRotation = originalRotations[rectTransform];
            }
            isShaking = false;

        }

    }

    IEnumerator StartShaking()
    {
        isShaking = true;
        float elapsedTime = 0f;

        // Ϊÿ�������崴��һ�������Ƶ�ʲ���
        Dictionary<RectTransform, float> shakeFrequencies = new Dictionary<RectTransform, float>();
        foreach (RectTransform rectTransform in childrenTransforms)
        {
            float randomVariance = Random.Range(-individualVariance, individualVariance);
            shakeFrequencies[rectTransform] = initialShakeFrequency + randomVariance;
        }

        while (true)
        {
            if(elapsedTime < shakeDuration)
                elapsedTime += Time.deltaTime;

            // ��ǰʱ���ڵ�Ƶ�ʣ�Ƶ�ʻ�������
            float currentFrequency = Mathf.Lerp(initialShakeFrequency, maxShakeFrequency, elapsedTime / shakeDuration);

            foreach (RectTransform rectTransform in childrenTransforms)
            {
                // ��ȡÿ��������Ļ���Ƶ�ʣ���Ӧ����
                float shakeFrequency = shakeFrequencies[rectTransform] * currentFrequency;

                // λ����
                float offsetX = Mathf.Sin(Time.time * shakeFrequency) * shakeMagnitude;
                float offsetY = Mathf.Cos(Time.time * shakeFrequency) * shakeMagnitude;
                rectTransform.anchoredPosition = originalPositions[rectTransform] + new Vector2(offsetX, offsetY) * (elapsedTime / shakeDuration); // �𶯷����𽥼�С

                // ��ת��
                float randomRotation = Mathf.Sin(Time.time * shakeFrequency) * rotationMagnitude;
                rectTransform.localRotation = originalRotations[rectTransform] * Quaternion.Euler(0, 0, randomRotation * (elapsedTime / shakeDuration)); // ��ת�����𽥼�С
            }

            yield return null;
        }
        
    }

    private void ResetShake()
    {
        foreach (RectTransform rectTransform in childrenTransforms)
        {
            rectTransform.anchoredPosition = originalPositions[rectTransform];
            rectTransform.localRotation = originalRotations[rectTransform];
        }
        isShaking = false;
        shake = false;
    }

    private void OnDisable()
    {
        ResetShake();
    }
}
