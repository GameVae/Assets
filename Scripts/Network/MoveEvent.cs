using Generic.Pool;
using Generic.Singleton;
using SocketIO;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveEvent : Listener
{
    private JSONObject positionArr;
    private List<JSONObject> positions;
    private JSONObject averageTimeObj;

    private float averageTime;
    private List<string> movePath;
    private AgentController agentCtrl;

    private PoolJSONObject jsonPool;

    private void Awake()
    {
        movePath = new List<string>();
        jsonPool = Singleton.Instance<PoolJSONObject>();
        positions = new List<JSONObject>();
        agentCtrl = GetComponent<AgentController>();

    }

    public override void RegisterCallback()
    {
        // AddEmiter("S_MOVE", S_MOVE);
        // Conn.On("R_MOVE", R_MOVE);
    }

    private void R_MOVE(SocketIOEvent obj)
    {
        // Debug.Log(obj);

        agentCtrl.MoveAgent(obj.data);
    }

    private JSONObject S_MOVE()
    {
        Dictionary<string, JSONObject> data = new Dictionary<string, JSONObject>();

        positionArr = jsonPool.Get(JSONObject.Type.ARRAY); ;
        for (int i = 0; i < movePath.Count; i++)
        {
            JSONObject value = jsonPool.Get(JSONObject.Type.STRING);
            value.str = movePath[i];
            positions.Add(value);
        }
        positionArr.list = positions;

        averageTimeObj = jsonPool.Get(JSONObject.Type.NUMBER);
        averageTimeObj.n = averageTime;

        data["PATH"] = positionArr;
        data["AVERAGE_TIME"] = averageTimeObj;

        JSONObject rs = new JSONObject(data);
        //Debug.Log(rs);
        return rs;
    }

    public void Move(List<Vector3Int> path, float _averageTime)
    {
        movePath.Clear();
        averageTime = _averageTime;

        JSONObject arr = new JSONObject();
        for (int i = 0; i < path.Count; i++)
        {
            movePath.Add(path[i].ToPositionString());
        }
        //Debug.Log(movePath.ToArray().ToJson());
        // Emit("S_MOVE");
        //S_MOVE();
        ReturnPool();
    }

    private void ReturnPool()
    {
        jsonPool.Return(positionArr);
        jsonPool.Return(positions);
        jsonPool.Return(averageTimeObj);

        positionArr = null;
        positions = null;
        averageTimeObj = null;
    }
}

[System.Serializable]
public sealed class MoveOffset
{
    private HexMap mapIns;

    public float AverageMoveTime;
    public int MaxMoveStep;

    public MoveOffset() { }

    public float GetSpeed(List<Vector3Int> path, Vector3 currentWorldPoint)
    {
        if (mapIns == null)
            mapIns = Singleton.Instance<HexMap>();

        float result = 0.0f;
        float distance = 0.0f;
        List<Vector3Int> currentPath = path;

        if (path.Count > MaxMoveStep)
            currentPath = TruncatePath(path);

        Vector3 currentPosition = currentWorldPoint;
        for (int i = currentPath.Count; i > 0; i--)
        {
            Vector3 nextPosition = mapIns.CellToWorld(currentPath[i - 1]);
            distance += Vector3.Distance(currentPosition, nextPosition);
            currentPosition = nextPosition;
        }
        result = distance / (currentPath.Count * AverageMoveTime);// van toc trung binh qua tat ca cac cell
		// khoang cach, AverageMoveTime => van toc => khi qua cell do 
        return result <= 0 ? 0 : result;
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