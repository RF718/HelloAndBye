using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashFadeController : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        MainStageManager.instance.FlashFade.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if(MainStageManager.instance!=null)
            MainStageManager.instance.FlashFade.gameObject.SetActive(false);
    }
}
