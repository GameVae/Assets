using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class MovementOffset
{
    private HexMap mapIns;

    public float MaxSpeed;
    public int MaxMoveStep;
    public int MaxSearchLevel;

    public MovementOffset() { }

    public List<float> GetTime(List<Vector3Int> path, Vector3 currentWorldPoint)
    {
        if (mapIns == null)
            mapIns = Singleton.Instance<HexMap>();

        float distance = 0.0f;
        List<Vector3Int> currentPath = path;
        List<float> separateTime = new List<float>();

        if (path.Count > MaxMoveStep)
            currentPath = TruncatePath(path);

        Vector3 currentPosition = currentWorldPoint;
        for (int i = currentPath.Count; i > 0; i--)
        {
            Vector3 nextPosition = mapIns.CellToWorld(currentPath[i - 1]);
            distance = Vector3.Distance(currentPosition, nextPosition);
            separateTime.Add(distance / MaxSpeed); // time
            currentPosition = nextPosition;
        }
        return separateTime;
    }

    public List<Vector3Int> TruncatePath(List<Vector3Int> path)
    {
        int startAt = path.Count - MaxMoveStep;
        if (startAt <= 0) return path;

        List<Vector3Int> result = new List<Vector3Int>();
        for (int i = startAt; i < path.Count; i++)
        {
            result.Add(path[i]);
        }
        return result;
    }
}