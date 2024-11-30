using UnityEngine;

[System.Serializable]
public class SceneStep
{
    public GameObject[] activeObjects;
    public GameObject[] inactiveObjects;
    public GameObject house;
    public AudioClip sound;
    public bool loop;
    public bool audioStop;
    public bool flash;
    public bool drizzle;
    public bool rainstorm;
    public bool houseShake;
    public bool fade;

    public void Execute()
    {
        foreach (var item in activeObjects) { item.SetActive(true); }
        foreach (var item in inactiveObjects) { item.SetActive(false); }

        AudioSource audioPlayer = MainStageManager.instance.AudioPlayer;
        if (sound != null)
        {
            
            audioPlayer.clip = sound;
            audioPlayer.loop = loop;
            audioPlayer.Play();
        }
        else if(audioStop)
        {
            audioPlayer.clip = null;
        }

        MainStageManager.instance.Flash.SetActive(flash);
        MainStageManager.instance.Drizzle.SetActive(drizzle);
        MainStageManager.instance.RainStorm.SetActive(rainstorm);

        if (house != null)
        {
            house.GetComponent<EarthquakeEffect>().shake = houseShake;
        }

        if (fade)
        {
            MainStageManager.instance.HouseFade();
        }


    }
}
