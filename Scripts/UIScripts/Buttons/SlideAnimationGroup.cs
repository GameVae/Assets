
using UI.Animation;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;

public class SlideAnimationGroup : MonoBehaviour
{
    public GUIInteractableIcon Open;
    public ResizeAnimation ResizeAnim;
    public HideChildren HideAnim;

    private void Awake()
    {
        Open.OnClickEvents += delegate 
        {
            ResizeAnim.Action();            
        };

        Open.OnClickEvents += delegate { HideAnim.Shown(); };
        ResizeAnim.CloseDoneEvt += HideAnim.Hidden;
    }
}
