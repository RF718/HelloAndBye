using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{

    public TextMeshProUGUI titel;
    public TextMeshProUGUI value;
    public Slider slider;

    public Image playButtonImage;

    public Sprite playImage;

    public Sprite pauseImage;

    public bool isBGM;

    private bool isPlayed;

    private void Update()
    {
        int decimalPlaces = BitConverter.GetBytes(decimal.GetBits((decimal)slider.value)[3])[2];
        if (decimalPlaces > 2)
        {
            // 超过两位小数时保留两位
            value.text = slider.value.ToString("F2");
        }
        else
        {
            // 没有超过两位小数时，直接转换为字符串
            value.text = slider.value.ToString();
        }
    }

    private void FixedUpdate()
    {
        if(pauseImage==null||playImage==null)
            return;
        isPlayed = MainStageManager.instance.audioManager.isSoundPlayed(this.name);

        if (isPlayed)
            playButtonImage.sprite = pauseImage;
        else
            playButtonImage.sprite = playImage;
    }

    public void UpdateUIAudio()
    {
        float value = 0;
        MainStageManager.instance.audioManager.mixer.GetFloat(this.name, out value);
        slider.value = value;
    }

    public void UpdateUIFunction(float value)
    {
        slider.value = value;
    }
    public void OnPlayButtonClicked()
    {

        MainStageManager.instance.audioManager.StopAllAudioEffect();
        MainStageManager.instance.audioManager.StopBgmPlayer();
        if(this.name=="Click")
            MainStageManager.instance.audioManager.PlayAudioEffect("SuccessSound");
        if (!isPlayed)
        {

            if (isBGM)
                MainStageManager.instance.audioManager.SetAndPlayBgmPlayer(this.name);
            else
                MainStageManager.instance.audioManager.PlayAudioEffect(this.name);      
        }
    }
}
