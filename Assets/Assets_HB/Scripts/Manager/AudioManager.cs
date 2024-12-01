using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds;                                  //���������Լ���Ч

    public AudioMixer mixer;                                    //������

    public AudioSource bgmPlayer;                              //bgm������

    public string changeableMixerGroupName;                     //�ɱ���Ч��������������

    public float volumeChangeSpeed = 10f;                       //�ɱ���Ч�仯�ٶ�

    public float smoothTime = 0.1f;                             //�ɱ���Ч����ʱ��

    public float minChangeableAudioEffectVolume = -30f;         //�ɱ���Ч��С����

    public float maxChangeableAudioEffectVolume = 30f;          //�ɱ���Ч�������

    public KeyCode bgmPauseOrResume = KeyCode.Space;            //bgm��ͣ��������Ű���

    public KeyCode bgmStop = KeyCode.LeftShift;                 //bgmֹͣ����

    public KeyCode bgmReplay = KeyCode.LeftAlt;                 //bgm�ز�����

    public KeyCode audioEffectCrescendo = KeyCode.Plus;         //�ɱ���Ч��ǿ����

    public KeyCode audioEffectDecrescendo = KeyCode.Minus;      //�ɱ���Ч��������

    public KeyCode audioEffectStopAll = KeyCode.LeftControl;    //ֹͣ������Ч����

    private Dictionary<string, Sound> bgms;                     //bgm��

    private Dictionary<string, AudioSource> audioEffects;       //��Ч��

    private float targetAudioEffectVolume;                      //�ɱ���ЧĿ������

    private float currentAudioEffectVolume;                     //�ɱ���Ч��ǰ����

    private void Awake()
    {
        //��ʼ��
        if (bgmPlayer == null)
            bgmPlayer=this.gameObject.GetComponent<AudioSource>();
        
        bgms = new Dictionary<string, Sound>();

        audioEffects = new Dictionary<string, AudioSource>();
    }

    private void Start()
    {
        //���������ֹ��ൽ��Ӧ��
        foreach (var sound in sounds)
        {
            if (sound.isBGM)
            {
                bgms[sound.name] = sound;
                //�����ʵ���Բ���Ҫ����ǰ�������������������д�ϰɣ���һ��
                if (sound.playOnAwake)
                {
                    SetAndPlayBgmPlayer(sound);
                    bgmPlayer.Play();
                }
            }
            else
            {
                //Ϊÿ����Ч���ɵ����Ĳ���������Ϊ���ܻ��ص����š�
                var audioSource = this.gameObject.AddComponent<AudioSource>();
                SetAudioSource(sound, audioSource);
            }    
        }

        //��ʼ���ɱ���Ч���ݡ�
        mixer.GetFloat(changeableMixerGroupName, out currentAudioEffectVolume);

        targetAudioEffectVolume = currentAudioEffectVolume;

        targetAudioEffectVolume = Mathf.Clamp(targetAudioEffectVolume, minChangeableAudioEffectVolume, maxChangeableAudioEffectVolume);
    }

    private void Update()
    {
        //�����Ч��ذ����¼�
        if (Input.GetKey(audioEffectCrescendo))
            CrescendoAudioEffect();
        else if (Input.GetKey(audioEffectDecrescendo))
            DerescendoAudioEffect();
        else if(Input.GetKeyDown(audioEffectStopAll))
            StopAllAudioEffect();

        //�ɱ���Ч����������Ȼ
        targetAudioEffectVolume = Mathf.Clamp(targetAudioEffectVolume, minChangeableAudioEffectVolume, maxChangeableAudioEffectVolume);

        currentAudioEffectVolume = Mathf.Lerp(currentAudioEffectVolume, targetAudioEffectVolume, Time.deltaTime / smoothTime);

        mixer.SetFloat(changeableMixerGroupName, currentAudioEffectVolume);

        foreach (var sound in sounds)
        {
            //���ֹͣ��Ч��Ҫ�Ȱ����Ҳ�alt���ٰ�����Ч��Ӧ�İ�����
            if (sound.isBGM && Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(sound.key))
            {
                StopAudioEffect(audioEffects[sound.name]);
                return;
            }
            
            //��ز������֣�������Ч��bgm
            if (Input.GetKeyDown(sound.key))
            {
                if (sound.isBGM)
                    SetAndPlayBgmPlayer(sound);
                else
                    PlayAudioEffect(audioEffects[sound.name]);
                return;   
            }

        }

        //���bgm�������
        if(Input.GetKeyDown(bgmPauseOrResume))
            PauseOrResumeBgmPlayer();
        else if(Input.GetKeyDown(bgmStop))
            StopBgmPlayer();
        else if(Input.GetKeyDown(bgmReplay))
            ReplayBgmPlayer();

    }

    /// <summary>
    /// ���ò��Ҳ���Bgm���������������Ա��ⲿ���á�
    /// </summary>
    /// <param name="name">sound������</param>
    public void SetAndPlayBgmPlayer(string name)
    {
        if (bgms.ContainsKey(name))
            SetAndPlayBgmPlayer(bgms[name]);
    }

    /// <summary>
    /// ���ò�����bgm
    /// </summary>
    /// <param name="sound">sound</param>
    private void SetAndPlayBgmPlayer(Sound sound)
    {
        StopBgmPlayer();
        SetAudioSource(sound, bgmPlayer);
        bgmPlayer.Play();
    }

    /// <summary>
    /// ��ͣ��������ŵ�ǰbgm
    /// </summary>
    private void PauseOrResumeBgmPlayer()
    {
        if (bgmPlayer.clip == null)
            return;
        if (bgmPlayer.isPlaying)
            bgmPlayer.Pause();
        else
            bgmPlayer.UnPause();
    }

    /// <summary>
    /// ֹͣ��ǰbgm
    /// </summary>
    public void StopBgmPlayer()
    {
        if (bgmPlayer.clip == null)
            return;
        bgmPlayer.Stop();
    }

    /// <summary>
    /// �ز���ǰbgm
    /// </summary>
    private void ReplayBgmPlayer()
    {
        if(bgmPlayer.clip==null)
            return;
        bgmPlayer.Stop();
        bgmPlayer.Play();
    }

    /// <summary>
    /// ������Ч
    /// </summary>
    /// <param name="name"></param>
    public void PlaypAudioEffect(string name)
    {
        if (audioEffects.ContainsKey(name))
            PlayAudioEffect(audioEffects[name]);
    }

    /// <summary>
    /// ������Ч��������ڲ��ţ����������ص�����һ����
    /// </summary>
    /// <param name="audioSource"></param>
    private void PlayAudioEffect(AudioSource audioSource)
    {
        //�ⲿ��һ��ʼ���Ϊ����ڲ�����ֹͣ�����ǿ��ǵ�ũ�������¿��ٵ��ʱ����Ч���ܻ����bug���޸�Ϊ������ʽ��
        if (audioSource.isPlaying)
            audioSource.PlayOneShot(audioSource.clip);
        else
            audioSource.Play();
    }

    /// <summary>
    /// ֹͣ��Ч ����
    /// </summary>
    /// <param name="audioSoundName"></param>
    public void StopAudioEffect(string audioSoundName)
    {
        if(audioEffects.ContainsKey(audioSoundName))
            StopAudioEffect(audioEffects[audioSoundName]);
    }

    /// <summary>
    /// ֹͣ��Ч
    /// </summary>
    /// <param name="audioSource"></param>
    private void StopAudioEffect(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    /// <summary>
    /// ��Ч��ǿ
    /// </summary>
    private void CrescendoAudioEffect()
    {
        targetAudioEffectVolume += volumeChangeSpeed * Time.deltaTime;
    }

    /// <summary>
    /// ��Ч����
    /// </summary>
    private void DerescendoAudioEffect()
    {
        targetAudioEffectVolume -= volumeChangeSpeed * Time.deltaTime; 
    }

    /// <summary>
    /// ֹͣ������Ч
    /// </summary>
    public void StopAllAudioEffect()
    {
        foreach (var effect in audioEffects)
        {
            effect.Value.Stop();
        }
    }

    /// <summary>
    /// ����AudioSource
    /// </summary>
    /// <param name="sound">sound��Ϣ����</param>
    /// <param name="source">audiosource</param>
    private void SetAudioSource(Sound sound,AudioSource source)
    {
        source.clip = sound.clip;
        source.volume = sound.volume;
        source.loop = sound.loop;
        source.playOnAwake = sound.playOnAwake;
        source.outputAudioMixerGroup = sound.mixerGroup;
    }

}
