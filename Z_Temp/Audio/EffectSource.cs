using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public sealed class EffectSource : MonoBehaviour
{
    private AudioSource audioSource;
    private EffectSound sound;

    public AudioSource AudioSource
    {
        get
        {
            return audioSource ?? (audioSource = gameObject.AddComponentNotExist<AudioSource>());
        }
    }

    public void SetSound(EffectSound effSound)
    {
        sound = effSound;

        AudioSource.clip = sound.AudioClip;
        AudioSource.volume = sound.DefaultVolume;
        AudioSource.pitch = sound.Pitch;
    }

    public void SetVolume(float value)
    {
        AudioSource.volume = sound.DefaultVolume * value;
    }
}
