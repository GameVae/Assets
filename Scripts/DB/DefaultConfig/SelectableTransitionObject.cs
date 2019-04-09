using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Selectable Transition Object", menuName = "Config/ColorBlock",order = 1)]
public class SelectableTransitionObject : ScriptableObject
{
    public ColorBlock Colors;
    public SpriteState SpriteState;
}
