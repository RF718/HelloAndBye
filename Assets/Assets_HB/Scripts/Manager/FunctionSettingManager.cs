using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionSettingManager : MonoBehaviour
{
    public SliderController game1;
    public SliderController game2;
    public SliderController game3;
    public SliderController game4;
    public SliderController blackFade;
    public SliderController audioEffectSpeed;
    // Start is called before the first frame update
    private void Awake()
    {
        //TODO:读取持久化数据
    }

    private void OnEnable()
    {
        //TODO；读取持久化数据
    }

    public void SaveFunctionSetting()
    {
        //TODO: 存储持久化数据；
    }
}
