using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionSettingManager : MonoBehaviour
{
    public SettingManager settingManager;
    public SliderController countDownTime_1;
    public SliderController countDownTime_2;
    public SliderController countDownTime_3;
    public SliderController countDownTime_4;
    public SliderController blackFade;
    public SliderController audioEffectSpeed;
    // Start is called before the first frame update
    
    public void InitializeSettings()
    {
        RevertToInitialSettings();
        SaveFunctionSetting();
    }
    private void Awake()
    {
        countDownTime_1.slider.value = PlayerPrefs.GetInt("countDownTime_1");
        countDownTime_2.slider.value = PlayerPrefs.GetInt("countDownTime_2");
        countDownTime_3.slider.value = PlayerPrefs.GetInt("countDownTime_3");
        countDownTime_4.slider.value = PlayerPrefs.GetInt("countDownTime_4");
        blackFade.slider.value = PlayerPrefs.GetFloat("blackFade");
        audioEffectSpeed.slider.value = PlayerPrefs.GetFloat("audioEffectSpeed");

    }

    private void OnEnable()
    {
        countDownTime_1.slider.value = PlayerPrefs.GetInt("countDownTime_1");
        countDownTime_2.slider.value = PlayerPrefs.GetInt("countDownTime_2");
        countDownTime_3.slider.value = PlayerPrefs.GetInt("countDownTime_3");
        countDownTime_4.slider.value = PlayerPrefs.GetInt("countDownTime_4");
        blackFade.slider.value = PlayerPrefs.GetFloat("blackFade");
        audioEffectSpeed.slider.value = PlayerPrefs.GetFloat("audioEffectSpeed");
    }

    public void SaveFunctionSetting()
    {
        PlayerPrefs.SetInt("countDownTime_1", (int)countDownTime_1.slider.value);
        PlayerPrefs.SetInt("countDownTime_2", (int)countDownTime_2.slider.value);
        PlayerPrefs.SetInt("countDownTime_3", (int)countDownTime_3.slider.value);
        PlayerPrefs.SetInt("countDownTime_4", (int)countDownTime_4.slider.value);
        PlayerPrefs.SetFloat("blackFade", blackFade.slider.value);
        PlayerPrefs.SetFloat("audioEffectSpeed", audioEffectSpeed.slider.value);
        PlayerPrefs.Save();
        UpdateFunctionSetting();
    }

    private void UpdateFunctionSetting()
    {
        MainStageManager.instance.scenesManager.scenes[1].countdownTime = countDownTime_1.slider.value;
        MainStageManager.instance.scenesManager.scenes[3].countdownTime = countDownTime_2.slider.value;
        MainStageManager.instance.scenesManager.scenes[6].countdownTime = countDownTime_3.slider.value;
        MainStageManager.instance.scenesManager.scenes[9].countdownTime = countDownTime_4.slider.value;
        MainStageManager.instance.scenesManager.fadeDuration = blackFade.slider.value;
        MainStageManager.instance.audioManager.volumeChangeSpeed = audioEffectSpeed.slider.value;
    }

    public void RevertToInitialSettings()
    {
        countDownTime_1.UpdateUIFunction(settingManager.defaultSetting.countDownTime_1);
        countDownTime_2.UpdateUIFunction(settingManager.defaultSetting.countDownTime_2);
        countDownTime_3.UpdateUIFunction(settingManager.defaultSetting.countDownTime_3);
        countDownTime_4.UpdateUIFunction(settingManager.defaultSetting.countDownTime_4);
        blackFade.UpdateUIFunction(settingManager.defaultSetting.blackFade);
        audioEffectSpeed.UpdateUIFunction(settingManager.defaultSetting.audioEffectSpeed);
    }
}
