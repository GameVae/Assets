using UnityEngine;
using UnityEngine.EventSystems;

public class ElementButton : MonoBehaviour,IPointerClickHandler
{
    protected ParticleSystem thisEffect;
    protected Transform groupTransform;
    protected ParticleSystem[] groupEffect;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (thisEffect.isPlaying) return;

        for (int i = 0; i < groupEffect.Length; i++)
        {
            groupEffect[i].Stop();
        }
        thisEffect.Play();

        //throw new System.NotImplementedException();
    }

    //protected override void Start()
    //{
    //    thisEffect = GetComponentInChildren<ParticleSystem>();
    //    groupTransform = transform.root;
    //    groupEffect = groupTransform.GetComponentsInChildren<ParticleSystem>();

    //    base.Start();
    //}
    //protected override void ButtonBehavior()
    //{
    //    if (thisEffect.isPlaying) return;

    //    for(int i = 0; i < groupEffect.Length; i++)
    //    {
    //        groupEffect[i].Stop();
    //    }
    //    thisEffect.Play();
    //}
    private void Start()
    {
        thisEffect = GetComponentInChildren<ParticleSystem>();
        groupTransform = transform.root;
        groupEffect = groupTransform.GetComponentsInChildren<ParticleSystem>();
    }
}
