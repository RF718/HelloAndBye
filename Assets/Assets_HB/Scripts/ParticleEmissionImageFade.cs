using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ParticleEmissionImageFade : MonoBehaviour
{
    public ParticleSystem particleSystem; // ��Ҫ������Particle System
    public Image targetImage; // ��Ҫ�ı�͸���ȵ�Image
    public float fadeDuration = 1.0f; // ͸���ȴӲ�͸����Ϊ͸����ʱ��
    public float waitDuration = 0.5f;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.Particle[] particles;
    private AudioSource player;

    void Start()
    {
        // ��ʼ����������
        particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        emissionModule = particleSystem.emission;
        player = GetComponent<AudioSource>();
    }

    void Update()
    {
        // ��ȡ��ǰ������������
        int numParticlesAlive = particleSystem.GetParticles(particles);

        // ������µ��������ɣ��򴥷����뵭��Ч��
        if (numParticlesAlive > 0)
        {
            StopAllCoroutines(); // ֹ֮ͣǰ���κε��뵭������
            StartCoroutine(FadeImage());
        }
    }

    private IEnumerator FadeImage()
    {
        yield return new WaitForSecondsRealtime(waitDuration);
        // ������Image����Ϊ��͸��

        SetImageAlpha(1.0f);
        player.Play();
        // Ȼ����ָ����ʱ�������Ա�Ϊ͸��
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
            SetImageAlpha(alpha);
            yield return null;
        }

        // ȷ��������ȫ͸��
        SetImageAlpha(0.0f);
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = targetImage.color;
        color.a = alpha;
        targetImage.color = color;
    }
}