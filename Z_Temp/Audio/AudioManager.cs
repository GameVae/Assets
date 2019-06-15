using Generic.Singleton;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class AudioManager : MonoSingle<AudioManager>
{
    public const string MusicVolumeKey = "__musicVolume";
    public const string EffectVolumeKey = "__effectVolume";

    private float musicVolume = -1;
    private float effectVolume = -1;
    private List<EffectSource> effectSoundsCatcher;

    public float MusicVolume
    {
        get
        {
            return musicVolume;
        }
    }
    public float EffectVolume
    {
        get
        {
            return effectVolume;
        }
    }
    public List<EffectSource> EffectSoundsCatcher
    {
        get
        {
            return effectSoundsCatcher ?? (effectSoundsCatcher = new List<EffectSource>());
        }
    }

    public Sound[] Musics;
    public EffectSound[] EffectSounds;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < Musics.Length; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.clip = Musics[i].AudioClip;
            audioSource.pitch = Musics[i].Pitch;
            audioSource.loop = Musics[i].IsLoop;

            Musics[i].AudioSource = audioSource;
        }

        musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, -1.0f);
        effectVolume =  PlayerPrefs.GetFloat(EffectVolumeKey, -1.0f);

        if(effectVolume < 0)
        {
            effectVolume = 1.0f;
        }
        if (musicVolume < 0)
        {
            musicVolume = 1;
        }
        SetSoundVolume(musicVolume);
    }

    public void Play(string audioName)
    {
        Sound sound = Array.Find(Musics, s => s.Name == audioName);
        if (sound != null)
            sound.Play();
    }

    public void SetSoundVolume(float value)
    {
        value = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(MusicVolumeKey, value);

        for (int i = 0; i < Musics.Length; i++)
        {
            Musics[i].SetVolume(Musics[i].DefaultVolume * value);
        }
    }

    public void SetEffectVolume(float value)
    {
        value = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(EffectVolumeKey, value);

        for (int i = 0; i < EffectSoundsCatcher.Count; i++)
        {
            EffectSoundsCatcher[i].SetVolume(value);
        }
    }

    public void AddEffectSoundTo(GameObject go,string effectName)
    {
        EffectSound sound = Array.Find(EffectSounds, s => s.Name == effectName);

        if(sound != null)
        {
            EffectSource source = go.AddComponentNotExist<EffectSource>();

            source.SetSound(sound);
            source.SetVolume(EffectVolume);
            EffectSoundsCatcher.Add(source);
        }
    }
}
