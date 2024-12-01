using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText;   //用于显示倒计时的组件吗

    public float countdownTime = 60;        //总时长

    public Color startColor = Color.green;  // 初始颜色
    public Color endColor = Color.red;      // 最终颜色

    public string audioEffectName;
    public string audioEffectEndName;

    private float currentTime;              //当前时间

    private bool audioEffectPlayed;      // 确保方法只调用一次
    private bool audioEffectEndPlayed;      // 确保方法只调用一次

    private bool isPaused;                  //是否暂停

    private void OnEnable()
    {
        //初始化设置
        currentTime = countdownTime;
        audioEffectPlayed = false;
        audioEffectEndPlayed = false;
        isPaused = true;

        UpdateCountDownText();

        countdownText.color = startColor;

    }

    void Update()
    {
        //如果暂停则不更新
        if(isPaused)
            return;

        if (currentTime > 0)
        {
            // 减少时间
            currentTime -= Time.deltaTime;

            // 更新颜色：倒计时小于等于 15 秒时，从绿色逐渐变为红色
            if (currentTime <= 15 && currentTime >= 10)
            {
                float t = Mathf.InverseLerp(15f, 10f, currentTime); // 归一化时间 (0 到 1)
                countdownText.color = Color.Lerp(startColor, endColor, t);
            }

            // 剩余时间小于等于 10 秒时调用方法（只调用一次）
            if (currentTime <= 10 && !audioEffectPlayed)
            {
                MainStageManager.instance.audioManager.PlayAudioEffect(audioEffectName);
                audioEffectPlayed = true; // 防止重复调用
            }

            // 更新倒计时文本
            UpdateCountDownText();
        }
        else
        {
            //到时候结束倒计时音效，并且播放结束音效，同样的只执行一次。
            if (!audioEffectEndPlayed)
            {
                MainStageManager.instance.audioManager.StopAudioEffect(audioEffectName);
                MainStageManager.instance.audioManager.PlayAudioEffect(audioEffectEndName);
                audioEffectEndPlayed = true;
            }
            currentTime = 0; // 确保时间不会低于 0

            UpdateCountDownText();
        }
    }

    /// <summary>
    /// 更新数字
    /// </summary>
    private void UpdateCountDownText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// 显示并设置倒计时，但是并不开始
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
    /// 暂停或开始倒计时
    /// </summary>
    public void PauseOrResumeCountDown()
    {
        isPaused = !isPaused;

        //如果最后十秒，还要继续或者暂停音效
        if (audioEffectPlayed)
        {
            if (isPaused)
                MainStageManager.instance.audioManager.StopAudioEffect(audioEffectName);
            else
                MainStageManager.instance.audioManager.PlayAudioEffect(audioEffectName);
        }
    }

    /// <summary>
    /// 重置倒计时
    /// </summary>
    public void ResetCountDown()
    {
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// 关闭倒计时
    /// </summary>
    public void CountDownTimeDisable()
    {
        this.gameObject.SetActive(false);
    }
}
