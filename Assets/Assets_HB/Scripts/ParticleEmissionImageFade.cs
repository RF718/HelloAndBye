using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ParticleEmissionImageFade : MonoBehaviour
{
    public ParticleSystem particleSystem; // 需要监听的Particle System
    public Image targetImage; // 需要改变透明度的Image
    public float fadeDuration = 1.0f; // 透明度从不透明变为透明的时间
    public float waitDuration = 0.5f;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.Particle[] particles;
    private AudioSource player;

    void Start()
    {
        // 初始化粒子数组
        particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        emissionModule = particleSystem.emission;
        player = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 获取当前存活的粒子数量
        int numParticlesAlive = particleSystem.GetParticles(particles);

        // 如果有新的粒子生成，则触发淡入淡出效果
        if (numParticlesAlive > 0)
        {
            StopAllCoroutines(); // 停止之前的任何淡入淡出操作
            StartCoroutine(FadeImage());
        }
    }

    private IEnumerator FadeImage()
    {
        yield return new WaitForSecondsRealtime(waitDuration);
        // 立即将Image设置为不透明

        SetImageAlpha(1.0f);
        player.Play();
        // 然后在指定的时间内线性变为透明
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
            SetImageAlpha(alpha);
            yield return null;
        }

        // 确保最终完全透明
        SetImageAlpha(0.0f);
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = targetImage.color;
        color.a = alpha;
        targetImage.color = color;
    }
}