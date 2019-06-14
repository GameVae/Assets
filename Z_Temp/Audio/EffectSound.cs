using UnityEngine;

[System.Serializable]
public sealed class EffectSound
{
    public string Name;
    public AudioClip AudioClip;

    public float Pitch;
    [Range(0, 1)]
    public float DefaultVolume;
}
