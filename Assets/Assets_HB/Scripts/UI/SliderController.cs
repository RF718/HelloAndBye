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
            // 超过两位小数时保留两位
            value.text = slider.value.ToString("F2");
        }
        else
        {
            // 没有超过两位小数时，直接转换为字符串
            value.text = slider.value.ToString();
        }
    }
}
