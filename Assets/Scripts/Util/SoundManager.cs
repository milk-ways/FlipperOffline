using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : DontDestroySingleton<SoundManager>
{
    [SerializeField] AudioContainer container;
    [SerializeField] AudioSource bgmSource;
    List<AudioSource> effectSource = new();


    private float masterVolume = 1.0f;
    public float MasterVolume
    {
        get { return masterVolume; }
        private set
        {
            masterVolume = value;
            if(bgmSource != null) bgmSource.volume = MasterVolume * BgmVolume;
            foreach(var item in effectSource)
            {
                item.volume = MasterVolume * VfxVolume;
            }
        }
    }

    private float bgmVolume = 0.1f;
    public float BgmVolume
    {
        get { return bgmVolume; }
        private set
        {
            bgmVolume = value;
            if (bgmSource != null) bgmSource.volume = MasterVolume * BgmVolume;
        }
    }

    private float vfxVolume = 0.1f;
    public float VfxVolume
    {
        get { return vfxVolume; }
        private set
        {
            vfxVolume = value;
            foreach (var item in effectSource)
            {
                item.volume = MasterVolume * VfxVolume;
            }
        }
    }

    Camera m_Camera;
    AudioListener m_AudioListener;

    protected override void Awake()
    {
        base.Awake();
        m_Camera = Camera.main;
        m_AudioListener = m_Camera.GetComponent<AudioListener>();
        SceneManager.activeSceneChanged += (a, b) =>
        {
            if(b.name == "Title")
            {
                PlayBGM("titleBGM");
            }

            for(int i = effectSource.Count - 1; i >= 0; i--)
            {
                if (effectSource[i] == null)
                {
                    effectSource.RemoveAt(i);
                }
            }
        };
    }

    public static void PlayEffect(string key)
    {
        if (!Instance.container.audioClipKey.Contains(key))
        {
            //Debug.LogWarning($"Audio key \"{key}\" does not exist!");
            return;
        }

        AudioSource source = null;
        foreach(var src in Instance.effectSource)
        {
            if (!src.isPlaying)
            {
                source = src;
                break;
            }
        }
        if(source == null)
        {
            var temp = ObjectPool.Instance.GetGO("AudioSource").GetComponent<AudioSource>();
            temp.loop = false;

            temp.volume = Instance.MasterVolume * Instance.VfxVolume;

            Instance.effectSource.Add(temp);
            source = temp;
        }
        
        source.clip = Instance.container.audioClips[Instance.container.audioClipKey.IndexOf(key)];
        source.Play();
    }

    const string s_Audio = "AudioSource";
    public static void PlayBGM(string key)
    {
        if (!Instance.container.audioClipKey.Contains(key))
        {
            //Debug.LogWarning($"Audio key \"{key}\" does not exist!");
            return;
        }

        if(Instance.bgmSource == null)
        {
            Instance.bgmSource = ObjectPool.Instance.GetGO(s_Audio).GetComponent<AudioSource>();
            Instance.bgmSource.loop = true;
        }
        Instance.bgmSource.clip = Instance.container.audioClips[Instance.container.audioClipKey.IndexOf(key)];
        Instance.bgmSource.volume = Instance.MasterVolume * Instance.BgmVolume;
        Instance.bgmSource.Play();
    }

    public static void StopBGM()
    {
        if (Instance.bgmSource != null)
        {
            Instance.bgmSource.Stop();
        }
    }

    public static void StopAll()
    {
        if(Instance.bgmSource != null)
        {
            Instance.bgmSource.Stop();
        }

        for(int i = 0; i < Instance.effectSource.Count; i++)
        {
            Instance.effectSource[i]?.Stop();
        }
    }
}