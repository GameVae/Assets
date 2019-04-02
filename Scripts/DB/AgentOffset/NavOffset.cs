using UnityEngine;

[CreateAssetMenu(fileName = "Nav Offset", menuName = "Agents/Offset", order = 1)]
public sealed class NavOffset : ScriptableObject
{
    public int MaxMoveStep;
    public int MaxSearchLevel;
    public float MaxSpeed;
}