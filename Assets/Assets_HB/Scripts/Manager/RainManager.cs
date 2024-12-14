using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RainManager : MonoBehaviour
{
    private AudioManager audioManager;
    public GameObject drizzlePrefab;
    public GameObject rainStormPrefab;
    public string drizzleSoundName;
    public string rainStromSoundName;

    private void Awake()
    {
        audioManager = MainStageManager.instance.audioManager;
    }

    public void SwitchDrizzle(bool switcher)
    {
        drizzlePrefab.SetActive(switcher);
        if(audioManager?.gameObject==null||audioManager.IsDestroyed())
            return;
        if (switcher)
            audioManager.PlayAudioEffect(drizzleSoundName);
        else
            audioManager.StopAudioEffect(drizzleSoundName);    
    }

    public void SwitchRainStrom(bool switcher)
    {
        rainStormPrefab.SetActive(switcher);

        if (switcher)
            audioManager.PlayAudioEffect(rainStromSoundName);
        else
            audioManager.StopAudioEffect(rainStromSoundName);
    }
}
