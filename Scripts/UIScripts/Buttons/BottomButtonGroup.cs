
using UI.Animation;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;

public class BottomButtonGroup : MonoBehaviour
{
    public GUIInteractableIcon Open;
    public ResizeAnimation ResizeAnim;

    private void Awake()
    {
        Open.OnClickEvents += delegate 
        {
            ResizeAnim.Action();            
        };
    }
}
