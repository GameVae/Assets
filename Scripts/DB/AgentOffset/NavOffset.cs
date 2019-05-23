using UnityEngine;


[CreateAssetMenu(fileName = "Nav Offset", menuName = "Agents/Offset", order = 1)]
public sealed class NavOffset : ScriptableObject
{
#pragma warning disable IDE0044
    [SerializeField] private int maxMoveStep;
    [SerializeField] private int maxSearchLevel;
    [SerializeField] private float maxSpeed;
    [SerializeField] private int attackRange;
#pragma warning restore IDE0044

    public int MaxMoveStep
    {
        get { return maxMoveStep; }
    }
    public int MaxSearchLevel
    {
        get { return maxSearchLevel; }
    }
    public float MaxSpeed
    {
        get { return maxSpeed; }
    }
    public int AttackRange
    {
        get { return attackRange; }
    }
}