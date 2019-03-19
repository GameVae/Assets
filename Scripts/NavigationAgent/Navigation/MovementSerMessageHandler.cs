using ManualTable.Row;
using System.Collections.Generic;
using UnityEngine;

public sealed class MovementSerMessageHandler
{
    private List<MoveStep> moveSteps;
    
    public List<MoveStep> HandlerEvent(JSONObject r_move)
    {
        if (moveSteps == null) moveSteps = new List<MoveStep>();
        else moveSteps.Clear();

        MoveStep firstStep = new MoveStep()
        {
            Position_Cell = r_move.GetField("Position_Cell").str,
            Next_Cell = r_move.GetField("Next_Cell").str,
            TimeMoveNextCell = r_move.GetField("TimeMoveNextCell").n
        };

        JSONObject listMove = r_move.GetField("ListMove");

        //moveSteps = JsonUtility.FromJson<List<MoveStep>>(listMove.ToString());
        //moveSteps.Insert(0, firstStep);

        moveSteps.Add(firstStep);
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
            res.Add(moveSteps[i].NextPosition);
        }
        return res.Invert();
    }
}
