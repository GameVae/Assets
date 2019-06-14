using UnityEngine;

[System.Serializable]
public sealed class Sound
{
    public string Name;
    public AudioClip AudioClip;
    public AudioSource AudioSource;

    public float Pitch;
    [Range(0, 1)]
    public float DefaultVolume;
    public bool IsLoop;

    public void Play()
    {
        AudioSource.Play();
    }

    public void Stop()
    {
        AudioSource.Stop();
    }

    public void Mute(bool value)
    {
        AudioSource.mute = value;
    }

    public void SetVolume(float value)
    {
        AudioSource.volume = Mathf.Clamp01(value);
    }
}
