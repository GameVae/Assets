using DataTable.Row;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class MovementSerMessageHandler
{
    private List<MoveStep> moveSteps;
    private List<Vector3Int> path;
    private HexMap mapIns;

    public MovementSerMessageHandler(HexMap hexMap)
    {
        mapIns = hexMap;
        path = new List<Vector3Int>();
        moveSteps = new List<MoveStep>();
    }

    public List<MoveStep> HandlerEvent(JSONObject r_move)
    {
        moveSteps.Clear();

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

        ConvertMoveStepsToPath(moveSteps);

        return moveSteps;
    }

    public List<Vector3Int> GetPath()
    {
        return path;
    }

    private void ConvertMoveStepsToPath(List<MoveStep> moveSteps)
    {
        path.Clear();
        for (int i = 0; i < moveSteps.Count; i++)
        {
            path.Add(moveSteps[i].NextPosition);
        }
        path = path.Invert();
    }

    public void Clear()
    {
        path?.Clear();
        moveSteps?.Clear();
    }

    /// <summary>
    /// Return speed and position for target
    /// </summary>
    /// <param name="target">world point target</param>
    /// <returns></returns>
    public float FirstStep(Vector3 current,out Vector3 target)
    {
        target = mapIns.CellToWorld(moveSteps[0].NextPosition);
        return CalculateSpeed(current, target, 0, moveSteps[0].TimeSecond);
    }

    public float NextStep(Vector3 current, out Vector3 target)
    {
        float lastTime = moveSteps[0].TimeSecond;

        path.RemoveAt(path.Count - 1);
        moveSteps.RemoveAt(0);

        if (moveSteps.Count > 0)
        {
            target = mapIns.CellToWorld(moveSteps[0].NextPosition);
            return CalculateSpeed(current, target, lastTime, moveSteps[0].TimeSecond);
        }
        target = Vector3.zero;
        return 0;
    }

    private float CalculateSpeed(Vector3 pos, Vector3 tar, float lastTime, float targetTime)
    {
        float deltaTime = targetTime - lastTime;
        if (deltaTime < 0)
        {
            path.Clear();
            return 0;
        }
        return Vector3.Distance(pos, tar) / deltaTime;
    }
}
