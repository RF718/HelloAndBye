using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSettingManager : MonoBehaviour
{
    public List<KeyBindingController> keyBindings;
    public SettingManager settingManager;

    public void InitializeSettings()
    {
        RevertToInitialSettings();
        SaveButtonSetting();
    }
    private void OnEnable()
    {
        foreach (var keyBindingController in keyBindings)
        {
            keyBindingController.button.interactable = true;

            keyBindingController.keyCode = (KeyCode)PlayerPrefs.GetInt(keyBindingController.actionName);

            keyBindingController.UpdateUI();

        }
    }

    public void SaveButtonSetting()
    {
        foreach(var keyBindingController in keyBindings)
        {
            PlayerPrefs.SetInt(keyBindingController.actionName, (int)keyBindingController.keyCode);
        }
        PlayerPrefs.Save();
        UpdateButtonSetting();
    }

    private void UpdateButtonSetting()
    {
        MainStageManager.instance.scenesManager.blackScene = keyBindings.Find(k => k.actionName == "blackScene").keyCode;
        MainStageManager.instance.scenesManager.backToLastStep = keyBindings.Find(k => k.actionName == "backToLastStep").keyCode;
        MainStageManager.instance.audioManager.bgmPauseOrResume = keyBindings.Find(k => k.actionName == "bgmPauseOrResume").keyCode;
        MainStageManager.instance.audioManager.bgmStop = keyBindings.Find(k => k.actionName == "bgmStop").keyCode;
        MainStageManager.instance.audioManager.bgmReplay = keyBindings.Find(k => k.actionName == "bgmReplay").keyCode;
        MainStageManager.instance.audioManager.plusBGM = keyBindings.Find(k => k.actionName == "plusBGM").keyCode;
        MainStageManager.instance.audioManager.minusBGM = keyBindings.Find(k => k.actionName == "minusBGM").keyCode;
        MainStageManager.instance.audioManager.audioEffectCrescendo = keyBindings.Find(k => k.actionName == "audioEffectCrescendo").keyCode;
        MainStageManager.instance.audioManager.audioEffectDecrescendo = keyBindings.Find(k => k.actionName == "audioEffectDecrescendo").keyCode;
        MainStageManager.instance.audioManager.audioEffectStopAll = keyBindings.Find(k => k.actionName == "audioEffectStopAll").keyCode;

    }

    public IEnumerator WaitForKeyPress(KeyBindingController keyBinding)
    {
        keyBinding.buttonText.text = "�밴��";

        // �������а����Ľ���
        foreach (var controller in keyBindings)
        {
            controller.button.interactable = false;
        }

        yield return null;

        while (true) // ѭ���ȴ���������
        {
            if (Input.anyKeyDown) // ����Ƿ������κμ�
            {
                foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(key)) // �����尴�µļ�
                    {
                        Debug.Log($"��⵽����{key.ToString()}");
                        // ����Ƿ�Ϊ���ų��İ���
                        if (key == KeyCode.Alpha1 || key == KeyCode.Alpha2 || key == KeyCode.Alpha3 || key == KeyCode.Alpha4 || key == KeyCode.Alpha5 ||
                            key == KeyCode.Alpha6 || key == KeyCode.Alpha7 || key == KeyCode.Alpha8 || key == KeyCode.Alpha9 || key == KeyCode.Alpha0 ||
                            key == KeyCode.Q || key == KeyCode.W || key == KeyCode.E || key == KeyCode.R || key == KeyCode.T || key == KeyCode.Y || key == KeyCode.U ||
                            key == KeyCode.I || key == KeyCode.O || key == KeyCode.P || key == KeyCode.Tab||key == KeyCode.Mouse0)
                        {
                            keyBinding.buttonText.text = $"{key.ToString()}�޷��趨";
                            break; // ������ǰ����⣬�����ȴ���������
                        }

                        // ���⴦�� Escape ��
                        if (key == KeyCode.Escape)
                        {
                            break;
                        }

                        // ���°�����
                        keyBinding.keyCode = key;
                        keyBinding.UpdateUI();

                        // �ָ���ť����
                        foreach (var keyBindingController in keyBindings)
                        {
                            if (keyBindingController != keyBinding && keyBindingController.keyCode == key)
                            {
                                keyBindingController.keyCode = KeyCode.None;
                                keyBindingController.UpdateUI();
                            }
                                
                            keyBindingController.button.interactable = true;
                        }

                        yield break; // �˳�Э��
                    }
                }
            }

            yield return null; // �ȴ���һ֡
        }
    }

    public void RevertToInitialSettings()
    {
        foreach (var keyBindingController in keyBindings)
        {
            keyBindingController.keyCode = (KeyCode)settingManager.defaultSetting.GetSetting(keyBindingController.actionName);
            keyBindingController.UpdateUI();
        }
    }
}
