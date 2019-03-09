using ManualTable.Row;
using System.Collections.Generic;
using UnityEngine;

public sealed class MovementSerMessageHandler
{
    private List<MoveStep> moveSteps;
    
    public List<MoveStep> HandlerEvent(JSONObject listMove)
    {
        if (moveSteps == null) moveSteps = new List<MoveStep>();
        else moveSteps.Clear();

        for (int i = 0; i < listMove.Count; i++)
        {
            MoveStep step = JsonUtility.FromJson<MoveStep>(listMove[i].ToString());
            moveSteps.Add(step);
        }
        return moveSteps;
    }

    public List<Vector3Int> GetPath(List<MoveStep> moveSteps)
    {
        List<Vector3Int> res = new List<Vector3Int>();
        for (int i = 0; i < moveSteps.Count; i++)
        {
            if (i == 0) res.Add(moveSteps[i].Position);
            res.Add(moveSteps[i].NextPosition);
        }
        return res;
    }
}
