using UnityEngine;

[System.Serializable]
public class SceneStep
{
    public GameObject[] activeObjects;
    public GameObject[] inactiveObjects;
    public GameObject house;
    public string audioEffect;
    public string bgm;
    public bool stopPreviousBgm;
    public bool clearAudioEffect;
    public bool flash;
    public bool drizzle;
    public bool rainstorm;
    public bool houseShake;
    public bool fade;

    public void Execute()
    {
        foreach (var item in activeObjects) { item.SetActive(true); }
        foreach (var item in inactiveObjects) { item.SetActive(false); }

        var mainStageManager = MainStageManager.instance;
        var audioManager = mainStageManager.audioManager;

        if(stopPreviousBgm)
            audioManager.StopBgmPlayer();

        if (!string.IsNullOrWhiteSpace(bgm))
            audioManager.SetAndPlayBgmPlayer(bgm);

        if (clearAudioEffect)
            audioManager.StopAllAudioEffect();

        if (!string.IsNullOrWhiteSpace(audioEffect))
            audioManager.PlaypAudioEffect(audioEffect);

        mainStageManager.FlashManager.SwitchFlashManager(flash);
        mainStageManager.RainManager.SwitchDrizzle(drizzle);
        mainStageManager.RainManager.SwitchRainStrom(rainstorm);

        if (house != null)
        {
            house.GetComponent<EarthquakeEffect>().shake = houseShake;
        }

        if (fade)
        {
            MainStageManager.instance.scenesManager.HouseFade();
        }


    }
}
