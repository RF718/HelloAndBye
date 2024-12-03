using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultSettings", menuName = "HBObject/DefaultSettings")]
public class DefaultSetting : ScriptableObject
{
    [Header("AudioSetting")]
    public float defaultVolume = 0f;


    [Header("FunctionSetting")]
    public int countDownTime_1 =90;
    public int countDownTime_2 = 90;
    public int countDownTime_3 = 90;
    public int countDownTime_4 = 90;
    public float blackFade = 0.5f;
    public float audioEffectSpeed = 10f;

    [Header("ButtonSetting")]
    public KeyCode blackScene = KeyCode.B;
    public KeyCode backToLastStep = KeyCode.Backspace;
    public KeyCode startStep = KeyCode.Return;
    public KeyCode nextStep = KeyCode.RightArrow;
    public KeyCode prevStep = KeyCode.LeftArrow;
    public KeyCode showCountDownTimer = KeyCode.Z;
    public KeyCode startOrPauseCountDownTimer =KeyCode.X;
    public KeyCode stopCountDownTimer = KeyCode.C;
    public KeyCode resetCountDownTimer = KeyCode.V;

    public KeyCode bgmPauseOrResume = KeyCode.Space;            //bgm暂停或继续播放按键

    public KeyCode bgmStop = KeyCode.LeftShift;                 //bgm停止按键

    public KeyCode bgmReplay = KeyCode.LeftAlt;

    public KeyCode plusBGM = KeyCode.UpArrow;

    public KeyCode minusBGM = KeyCode.DownArrow;

    public KeyCode audioEffectCrescendo = KeyCode.Equals;         //可变音效渐强按键

    public KeyCode audioEffectDecrescendo = KeyCode.Minus;      //可变音效减弱按键

    public KeyCode audioEffectStopAll = KeyCode.LeftControl;

    private Dictionary<string, object> settingsDictionary;

    // 初始化字典
    public void InitializeSettingsDictionary()
    {
        settingsDictionary = new Dictionary<string, object>();

        // 获取所有字段并存入字典
        var fields = GetType().GetFields();
        foreach (var field in fields)
        {
            settingsDictionary[field.Name] = field.GetValue(this);
        }
    }

    // 获取属性值
    public object GetSetting(string propertyName)
    {
        if (settingsDictionary == null)
        {
            InitializeSettingsDictionary();
        }

        if (settingsDictionary.TryGetValue(propertyName, out var value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning($"属性名 {propertyName} 不存在！");
            return null;
        }
    }

    // 设置属性值
    public void SetSetting(string propertyName, object value)
    {
        if (settingsDictionary == null)
        {
            InitializeSettingsDictionary();
        }

        if (settingsDictionary.ContainsKey(propertyName))
        {
            settingsDictionary[propertyName] = value;

            // 更新实际字段值
            var field = GetType().GetField(propertyName);
            if (field != null && field.FieldType.IsAssignableFrom(value.GetType()))
            {
                field.SetValue(this, value);
            }
            else
            {
                Debug.LogWarning($"无法更新属性 {propertyName}，类型不匹配！");
            }
        }
        else
        {
            Debug.LogWarning($"属性名 {propertyName} 不存在！");
        }
    }
}
