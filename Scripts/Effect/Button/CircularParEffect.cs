using UnityEngine;

public class CircularParEffect : MonoBehaviour
{
    public float Radius;
    public float StartSize;
    public Color Color;
    public OnOffSwitch SwitchButton;

    private ParticleSystem particle;
    private ParticleSystem.MainModule main;
    private ParticleSystem.ShapeModule shape;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        main = particle.main;
        shape = particle.shape;

        shape.radius = Radius;
        main.startColor = Color;
        main.startSize = StartSize;

        SwitchButton.On += On;
        SwitchButton.Off += Off;
    }

    private void On(OnOffSwitch onOff)
    {
        particle.Play();
    }
    private void Off(OnOffSwitch onOff)
    {
        particle.Stop();
    }
}
