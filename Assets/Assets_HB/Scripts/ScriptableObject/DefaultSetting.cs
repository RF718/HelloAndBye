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

    public KeyCode bgmPauseOrResume = KeyCode.Space;            //bgm��ͣ��������Ű���

    public KeyCode bgmStop = KeyCode.LeftShift;                 //bgmֹͣ����

    public KeyCode bgmReplay = KeyCode.LeftAlt;

    public KeyCode plusBGM = KeyCode.UpArrow;

    public KeyCode minusBGM = KeyCode.DownArrow;

    public KeyCode audioEffectCrescendo = KeyCode.Equals;         //�ɱ���Ч��ǿ����

    public KeyCode audioEffectDecrescendo = KeyCode.Minus;      //�ɱ���Ч��������

    public KeyCode audioEffectStopAll = KeyCode.LeftControl;

    private Dictionary<string, object> settingsDictionary;

    // ��ʼ���ֵ�
    public void InitializeSettingsDictionary()
    {
        settingsDictionary = new Dictionary<string, object>();

        // ��ȡ�����ֶβ������ֵ�
        var fields = GetType().GetFields();
        foreach (var field in fields)
        {
            settingsDictionary[field.Name] = field.GetValue(this);
        }
    }

    // ��ȡ����ֵ
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
            Debug.LogWarning($"������ {propertyName} �����ڣ�");
            return null;
        }
    }

    // ��������ֵ
    public void SetSetting(string propertyName, object value)
    {
        if (settingsDictionary == null)
        {
            InitializeSettingsDictionary();
        }

        if (settingsDictionary.ContainsKey(propertyName))
        {
            settingsDictionary[propertyName] = value;

            // ����ʵ���ֶ�ֵ
            var field = GetType().GetField(propertyName);
            if (field != null && field.FieldType.IsAssignableFrom(value.GetType()))
            {
                field.SetValue(this, value);
            }
            else
            {
                Debug.LogWarning($"�޷��������� {propertyName}�����Ͳ�ƥ�䣡");
            }
        }
        else
        {
            Debug.LogWarning($"������ {propertyName} �����ڣ�");
        }
    }
}
