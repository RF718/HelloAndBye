using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public TextMeshProUGUI titel;
    public TextMeshProUGUI value;
    public Slider slider;
    private void Update()
    {
        int decimalPlaces = BitConverter.GetBytes(decimal.GetBits((decimal)slider.value)[3])[2];
        if (decimalPlaces > 2)
        {
            // ������λС��ʱ������λ
            value.text = slider.value.ToString("F2");
        }
        else
        {
            // û�г�����λС��ʱ��ֱ��ת��Ϊ�ַ���
            value.text = slider.value.ToString();
        }
    }
}
