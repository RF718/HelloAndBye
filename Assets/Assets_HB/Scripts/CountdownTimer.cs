using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText;   //������ʾ����ʱ�������

    public float countdownTime = 60;        //��ʱ��

    public Color startColor = Color.green;  // ��ʼ��ɫ
    public Color endColor = Color.red;      // ������ɫ

    public string audioEffectName;
    public string audioEffectEndName;

    private float currentTime;              //��ǰʱ��

    private bool audioEffectPlayed;      // ȷ������ֻ����һ��
    private bool audioEffectEndPlayed;      // ȷ������ֻ����һ��

    private bool isPaused;                  //�Ƿ���ͣ

    private void OnEnable()
    {
        //��ʼ������
        currentTime = countdownTime;
        audioEffectPlayed = false;
        audioEffectEndPlayed = false;
        isPaused = true;

        UpdateCountDownText();

        countdownText.color = startColor;

    }

    void Update()
    {
        //�����ͣ�򲻸���
        if(isPaused)
            return;

        if (currentTime > 0)
        {
            // ����ʱ��
            currentTime -= Time.deltaTime;

            // ������ɫ������ʱС�ڵ��� 15 ��ʱ������ɫ�𽥱�Ϊ��ɫ
            if (currentTime <= 15 && currentTime >= 10)
            {
                float t = Mathf.InverseLerp(15f, 10f, currentTime); // ��һ��ʱ�� (0 �� 1)
                countdownText.color = Color.Lerp(startColor, endColor, t);
            }

            // ʣ��ʱ��С�ڵ��� 10 ��ʱ���÷�����ֻ����һ�Σ�
            if (currentTime <= 10 && !audioEffectPlayed)
            {
                MainStageManager.instance.audioManager.PlayAudioEffect(audioEffectName);
                audioEffectPlayed = true; // ��ֹ�ظ�����
            }

            // ���µ���ʱ�ı�
            UpdateCountDownText();
        }
        else
        {
            //��ʱ���������ʱ��Ч�����Ҳ��Ž�����Ч��ͬ����ִֻ��һ�Ρ�
            if (!audioEffectEndPlayed)
            {
                MainStageManager.instance.audioManager.StopAudioEffect(audioEffectName);
                MainStageManager.instance.audioManager.PlayAudioEffect(audioEffectEndName);
                audioEffectEndPlayed = true;
            }
            currentTime = 0; // ȷ��ʱ�䲻����� 0

            UpdateCountDownText();
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    private void UpdateCountDownText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// ��ʾ�����õ���ʱ�����ǲ�����ʼ
    /// </summary>
    /// <param name="time"></param>
    public void ShowAndSetCountDown(float time)
    {
        if (this.gameObject.activeSelf) 
            this.gameObject.SetActive(false);

        countdownTime = time;
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// ��ͣ��ʼ����ʱ
    /// </summary>
    public void PauseOrResumeCountDown()
    {
        isPaused = !isPaused;

        //������ʮ�룬��Ҫ����������ͣ��Ч
        if (audioEffectPlayed)
        {
            if (isPaused)
                MainStageManager.instance.audioManager.StopAudioEffect(audioEffectName);
            else
                MainStageManager.instance.audioManager.PlayAudioEffect(audioEffectName);
        }
    }

    /// <summary>
    /// ���õ���ʱ
    /// </summary>
    public void ResetCountDown()
    {
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// �رյ���ʱ
    /// </summary>
    public void CountDownTimeDisable()
    {
        this.gameObject.SetActive(false);
    }
}
