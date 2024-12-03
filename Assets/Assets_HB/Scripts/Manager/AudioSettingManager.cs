using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioSettingManager : MonoBehaviour
{
    public SettingManager settingManager;

    private AudioManager _audioManager;


    public List<SliderController> sliderControllers;

    public AudioClip cd;

    public void InitializeSettings()
    {
        RevertToInitialSettings();
        SaveAudioSetting();
    }

    private void OnEnable()
    {
        foreach (var sliderController in sliderControllers)
        {
            if(sliderController.name != "BGM")
                MainStageManager.instance.audioManager.mixer.SetFloat(sliderController.name, PlayerPrefs.GetFloat(sliderController.name,0));
            sliderController.UpdateUIAudio();
        }
        MainStageManager.instance.audioManager.ResetBGMTarget();
    }

    private void OnDisable()
    {
        ResetSetting();
        MainStageManager.instance.audioManager.ResetBGMTarget();
    }

    public void SaveAudioSetting()
    {
        foreach (var sliderController in sliderControllers)
        {
            PlayerPrefs.SetFloat(sliderController.name, sliderController.slider.value);
            MainStageManager.instance.audioManager.mixer.SetFloat(sliderController.name, sliderController.slider.value);
        }
    }

    public void ResetSetting()
    {
        foreach (var sliderController in sliderControllers)
        {
            MainStageManager.instance.audioManager.mixer.SetFloat(sliderController.name, PlayerPrefs.GetFloat(sliderController.name));
        }
    }

    public void OnSliderValueChanged(float value)
    {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        if (selectedObject != null)
        {
            var changedSlider = selectedObject.GetComponent<SliderNameGetter>();
            if (changedSlider != null)
            {
                MainStageManager.instance.audioManager.mixer.SetFloat(changedSlider.parentName, value);
            }
        }
    }

    public void onCDClicked()
    {
        var cdSource = this.gameObject.GetComponent<AudioSource>();
        cdSource.clip = cd;
        cdSource.loop = false;
        cdSource.Play();

    }

    public void RevertToInitialSettings()
    {
        foreach(var sliderController in sliderControllers)
        {
            MainStageManager.instance.audioManager.mixer.SetFloat(sliderController.name, settingManager.defaultSetting.defaultVolume);
            sliderController.UpdateUIAudio();
        }
    }
}
