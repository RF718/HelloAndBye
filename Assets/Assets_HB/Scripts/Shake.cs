using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeEffect : MonoBehaviour
{
    public bool shake = false; // 控制是否开始震动
    public float initialShakeFrequency = 0.1f; // 起始震动频率
    public float maxShakeFrequency = 5f; // 最大震动频率
    public float shakeDuration = 5f; // 震动持续时间
    public float shakeMagnitude = 5f; // 震动幅度
    public float rotationMagnitude = 5f; // 旋转变化的最大角度
    public float individualVariance = 0.5f; // 每个子物体的频率差异

    private List<RectTransform> childrenTransforms = new List<RectTransform>();
    private Dictionary<RectTransform, Vector2> originalPositions = new Dictionary<RectTransform, Vector2>();
    private Dictionary<RectTransform, Quaternion> originalRotations = new Dictionary<RectTransform, Quaternion>();
    private bool isShaking = false;
    private IEnumerator ShakeCorou;

    void Start()
    {
        // 获取所有子物体的RectTransform并记录原始位置和旋转
        foreach (Transform child in transform)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                childrenTransforms.Add(rectTransform);
                originalPositions[rectTransform] = rectTransform.anchoredPosition; // 记录初始位置
                originalRotations[rectTransform] = rectTransform.localRotation;    // 记录初始旋转
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

        // 为每个子物体创建一个随机震动频率差异
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

            // 当前时间内的频率，频率会逐渐增加
            float currentFrequency = Mathf.Lerp(initialShakeFrequency, maxShakeFrequency, elapsedTime / shakeDuration);

            foreach (RectTransform rectTransform in childrenTransforms)
            {
                // 获取每个子物体的基础频率，并应用震动
                float shakeFrequency = shakeFrequencies[rectTransform] * currentFrequency;

                // 位置震动
                float offsetX = Mathf.Sin(Time.time * shakeFrequency) * shakeMagnitude;
                float offsetY = Mathf.Cos(Time.time * shakeFrequency) * shakeMagnitude;
                rectTransform.anchoredPosition = originalPositions[rectTransform] + new Vector2(offsetX, offsetY) * (elapsedTime / shakeDuration); // 震动幅度逐渐减小

                // 旋转震动
                float randomRotation = Mathf.Sin(Time.time * shakeFrequency) * rotationMagnitude;
                rectTransform.localRotation = originalRotations[rectTransform] * Quaternion.Euler(0, 0, randomRotation * (elapsedTime / shakeDuration)); // 旋转幅度逐渐减小
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
