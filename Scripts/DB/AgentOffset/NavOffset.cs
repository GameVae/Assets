using UnityEngine;

[CreateAssetMenu(fileName = "Nav Offset", menuName = "Agents/Offset", order = 1)]
public sealed class NavOffset : ScriptableObject
{
    [SerializeField] private int maxMoveStep;
    [SerializeField] private int maxSearchLevel;
    [SerializeField] private float maxSpeed;
    [SerializeField] private int attackRange;

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