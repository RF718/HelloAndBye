using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class SettingManager : MonoBehaviour
{
    public AudioSettingManager audioSetting;
    public ButtonSettingManager buttonSetting;
    public FunctionSettingManager functionSetting;

    public DefaultSetting defaultSetting;

    public MenuManager menuManager;

    private void OnEnable()
    {
        OnAudioSettingClicked();
    }

    public void InitializeSettings()
    {
        if (!PlayerPrefs.HasKey("SettingsInitialized"))
        {
            Debug.Log("��һ�����У�ʹ��Ĭ�����ã�");
            audioSetting.InitializeSettings();
            functionSetting.InitializeSettings();
            buttonSetting.InitializeSettings();
            PlayerPrefs.SetInt("SettingsInitialized", 1);
        }
    }
    public void OnAudioSettingClicked()
    {
        audioSetting.gameObject.SetActive(true);
        buttonSetting.gameObject.SetActive(false);
        functionSetting.gameObject.SetActive(false);
    }

    public void OnButtonSettingClicked()
    {
        buttonSetting.gameObject.SetActive(true);
        functionSetting.gameObject.SetActive(false);
        audioSetting.gameObject.SetActive(false);
    }

    public void OnFunctionSettingClicked()
    {
        functionSetting.gameObject.SetActive(true);
        audioSetting.gameObject.SetActive(false);
        buttonSetting.gameObject.SetActive(false);
    }

    public void OnSaveButtonClicked()
    {
        if (buttonSetting.gameObject.activeSelf)
            buttonSetting.SaveButtonSetting();
        else if (audioSetting.gameObject.activeSelf)
            audioSetting.SaveAudioSetting();
        else
            functionSetting.SaveFunctionSetting();
    }

    public void OnCancelButtonClicked()
    {
        this.gameObject.SetActive(false);
        menuManager.mainMenu.SetActive(true);
    }

    public void Revert()
    {
        if(buttonSetting.gameObject.activeSelf)
            buttonSetting.RevertToInitialSettings();
        else if(audioSetting.gameObject.activeSelf)
            audioSetting.RevertToInitialSettings();
        else
            functionSetting.RevertToInitialSettings();

    }
}
