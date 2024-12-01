using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds;                                  //所有音乐以及音效

    public AudioMixer mixer;                                    //混响器

    public AudioSource bgmPlayer;                              //bgm播放器

    public string changeableMixerGroupName;                     //可变音效混响器分组名称

    public float volumeChangeSpeed = 10f;                       //可变音效变化速度

    public float smoothTime = 0.1f;                             //可变音效滑动时间

    public float minChangeableAudioEffectVolume = -30f;         //可变音效最小音量

    public float maxChangeableAudioEffectVolume = 30f;          //可变音效最大音量

    public KeyCode bgmPauseOrResume = KeyCode.Space;            //bgm暂停或继续播放按键

    public KeyCode bgmStop = KeyCode.LeftShift;                 //bgm停止按键

    public KeyCode bgmReplay = KeyCode.LeftAlt;                 //bgm重播按键

    public KeyCode audioEffectCrescendo = KeyCode.Plus;         //可变音效渐强按键

    public KeyCode audioEffectDecrescendo = KeyCode.Minus;      //可变音效减弱按键

    public KeyCode audioEffectStopAll = KeyCode.LeftControl;    //停止所有音效按键

    private Dictionary<string, Sound> bgms;                     //bgm库

    private Dictionary<string, AudioSource> audioEffects;       //音效库

    private float targetAudioEffectVolume;                      //可变音效目标音量

    private float currentAudioEffectVolume;                     //可变音效当前音量

    private void Awake()
    {
        //初始化
        if (bgmPlayer == null)
            bgmPlayer=this.gameObject.GetComponent<AudioSource>();
        
        bgms = new Dictionary<string, Sound>();

        audioEffects = new Dictionary<string, AudioSource>();
    }

    private void Start()
    {
        //将所有音乐归类到对应库
        foreach (var sound in sounds)
        {
            if (sound.isBGM)
            {
                bgms[sound.name] = sound;
                //这个其实可以不需要，当前不存在这样的情况，先写上吧，万一呢
                if (sound.playOnAwake)
                {
                    SetAndPlayBgmPlayer(sound);
                    bgmPlayer.Play();
                }
            }
            else
            {
                //为每个音效生成单独的播放器，因为可能会重叠播放。
                var audioSource = this.gameObject.AddComponent<AudioSource>();
                SetAudioSource(sound, audioSource);
            }    
        }

        //初始化可变音效数据。
        mixer.GetFloat(changeableMixerGroupName, out currentAudioEffectVolume);

        targetAudioEffectVolume = currentAudioEffectVolume;

        targetAudioEffectVolume = Mathf.Clamp(targetAudioEffectVolume, minChangeableAudioEffectVolume, maxChangeableAudioEffectVolume);
    }

    private void Update()
    {
        //监控音效相关按键事件
        if (Input.GetKey(audioEffectCrescendo))
            CrescendoAudioEffect();
        else if (Input.GetKey(audioEffectDecrescendo))
            DerescendoAudioEffect();
        else if(Input.GetKeyDown(audioEffectStopAll))
            StopAllAudioEffect();

        //可变音效滑动更加自然
        targetAudioEffectVolume = Mathf.Clamp(targetAudioEffectVolume, minChangeableAudioEffectVolume, maxChangeableAudioEffectVolume);

        currentAudioEffectVolume = Mathf.Lerp(currentAudioEffectVolume, targetAudioEffectVolume, Time.deltaTime / smoothTime);

        mixer.SetFloat(changeableMixerGroupName, currentAudioEffectVolume);

        foreach (var sound in sounds)
        {
            //监控停止音效，要先按下右侧alt，再按下音效对应的按键。
            if (sound.isBGM && Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(sound.key))
            {
                StopAudioEffect(audioEffects[sound.name]);
                return;
            }
            
            //监控播放音乐！包括音效和bgm
            if (Input.GetKeyDown(sound.key))
            {
                if (sound.isBGM)
                    SetAndPlayBgmPlayer(sound);
                else
                    PlayAudioEffect(audioEffects[sound.name]);
                return;   
            }

        }

        //监控bgm控制相关
        if(Input.GetKeyDown(bgmPauseOrResume))
            PauseOrResumeBgmPlayer();
        else if(Input.GetKeyDown(bgmStop))
            StopBgmPlayer();
        else if(Input.GetKeyDown(bgmReplay))
            ReplayBgmPlayer();

    }

    /// <summary>
    /// 设置并且播放Bgm，公共方法，可以被外部调用。
    /// </summary>
    /// <param name="name">sound的名字</param>
    public void SetAndPlayBgmPlayer(string name)
    {
        if (bgms.ContainsKey(name))
            SetAndPlayBgmPlayer(bgms[name]);
    }

    /// <summary>
    /// 设置并播放bgm
    /// </summary>
    /// <param name="sound">sound</param>
    private void SetAndPlayBgmPlayer(Sound sound)
    {
        StopBgmPlayer();
        SetAudioSource(sound, bgmPlayer);
        bgmPlayer.Play();
    }

    /// <summary>
    /// 暂停或继续播放当前bgm
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
    /// 停止当前bgm
    /// </summary>
    public void StopBgmPlayer()
    {
        if (bgmPlayer.clip == null)
            return;
        bgmPlayer.Stop();
    }

    /// <summary>
    /// 重播当前bgm
    /// </summary>
    private void ReplayBgmPlayer()
    {
        if(bgmPlayer.clip==null)
            return;
        bgmPlayer.Stop();
        bgmPlayer.Play();
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name"></param>
    public void PlaypAudioEffect(string name)
    {
        if (audioEffects.ContainsKey(name))
            PlayAudioEffect(audioEffects[name]);
    }

    /// <summary>
    /// 播放音效，如果正在播放，则另外再重叠播放一个。
    /// </summary>
    /// <param name="audioSource"></param>
    private void PlayAudioEffect(AudioSource audioSource)
    {
        //这部分一开始设计为如果在播放则停止，但是考虑到农场场景下快速点击时的音效可能会造成bug，修改为下面形式。
        if (audioSource.isPlaying)
            audioSource.PlayOneShot(audioSource.clip);
        else
            audioSource.Play();
    }

    /// <summary>
    /// 停止音效 公共
    /// </summary>
    /// <param name="audioSoundName"></param>
    public void StopAudioEffect(string audioSoundName)
    {
        if(audioEffects.ContainsKey(audioSoundName))
            StopAudioEffect(audioEffects[audioSoundName]);
    }

    /// <summary>
    /// 停止音效
    /// </summary>
    /// <param name="audioSource"></param>
    private void StopAudioEffect(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    /// <summary>
    /// 音效渐强
    /// </summary>
    private void CrescendoAudioEffect()
    {
        targetAudioEffectVolume += volumeChangeSpeed * Time.deltaTime;
    }

    /// <summary>
    /// 音效减弱
    /// </summary>
    private void DerescendoAudioEffect()
    {
        targetAudioEffectVolume -= volumeChangeSpeed * Time.deltaTime; 
    }

    /// <summary>
    /// 停止所有音效
    /// </summary>
    public void StopAllAudioEffect()
    {
        foreach (var effect in audioEffects)
        {
            effect.Value.Stop();
        }
    }

    /// <summary>
    /// 设置AudioSource
    /// </summary>
    /// <param name="sound">sound信息对象</param>
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
