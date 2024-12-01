using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashManager : MonoBehaviour
{
    private AudioManager audioManager;

    public GameObject flashFade;

    public string flashSound;

    private void Awake()
    {
        audioManager = MainStageManager.instance.audioManager;
    }
    private void OnEnable()
    {
        flashFade.SetActive(true);
        audioManager.PlayAudioEffect(flashSound);

    }

    private void OnDisable()
    {
        flashFade.SetActive(false);
        audioManager.StopAudioEffect(flashSound);
    }

    public void SwitchFlashManager(bool switcher)
    {
        this.gameObject.SetActive(switcher);
    }
}
