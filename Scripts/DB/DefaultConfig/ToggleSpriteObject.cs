using UnityEngine;

[CreateAssetMenu(fileName = "Toggle Sprite State",menuName = "Config/Toggle Sprite State",order = 3)]
public class ToggleSpriteObject : ScriptableObject
{
    public Sprite ActiveSprite;
    public Sprite UnActiveSprite;
}
