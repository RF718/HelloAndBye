using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds;

    private Dictionary<string, AudioSource> soundSources;

    private void Awake()
    {
        soundSources = new Dictionary<string, AudioSource>();
    }

    private void Start()
    {
        foreach (var sound in sounds)
        {
            GameObject obj = new GameObject(sound.name);

            obj.transform.SetParent(transform);

            AudioSource source = obj.AddComponent<AudioSource>();

            source.clip = sound.clip;
            source.volume = sound.volume;
            source.loop = sound.loop;
            source.playOnAwake = sound.playOnAwake;
            source.outputAudioMixerGroup = sound.mixerGroup;

            if(sound.playOnAwake)
                source.Play();

            soundSources.Add(sound.name, source);

        }
    }

    private void Update()
    {
        foreach (var sound in sounds)
        {
            if (Input.GetKeyDown(sound.key))
                PlayTheAudio(sound.name);
        }

    }

    private void PlayTheAudio(string name)
    {
        soundSources[name].Play();
    }
}
