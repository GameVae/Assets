using Entities.Navigation;
using EnumCollect;
using Generic.Singleton;
using ManualTable.Row;
using SocketIO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SIO_MovementListener : Listener
{
    private NavAgentController agentCtrl;
    private NonControlAgentManager nCtrlAgentManager;
    private JSONObject moveJSONObject;
    private string moveJson;

    protected override void Start()
    {
        base.Start();
        agentCtrl = Singleton.Instance<NavAgentController>();
        nCtrlAgentManager = Singleton.Instance<NonControlAgentManager>();
        moveJSONObject = new JSONObject(JSONObject.Type.BAKED);
    }

    public override void RegisterCallback()
    {
        AddEmiter("S_MOVE", S_MOVE);

        On("R_MOVE", R_MOVE);
    }


    private JSONObject S_MOVE()
    {
        moveJSONObject.Clear();
        moveJSONObject.type = JSONObject.Type.BAKED;
        moveJSONObject.str = moveJson;

        Debug.Log(moveJSONObject);
        return moveJSONObject;
    }

    public void Move(List<Vector3Int> path,
        List<float> separateTime,
        Vector3Int curCellPosition,
        ListUpgrade unit)
    {
        InitMessage(path, separateTime, curCellPosition, unit);
        Emit("S_MOVE"); ;
    }

    private void InitMessage(List<Vector3Int> realPath, List<float> separateTime, Vector3Int curCellPosition, ListUpgrade unit)
    {
        Vector3Int substractBy = new Vector3Int(5, 5, 0);
        curCellPosition -= substractBy;

        List<Vector3Int> tempPath = new List<Vector3Int>(realPath);
        for (int i = 0; i < tempPath.Count; i++)
        {
            tempPath[i] -= substractBy;
        }
        tempPath = tempPath.Invert();

        const string format =
            "{{" +
            "\"Server_ID\":" + "{0}," +
            "\"ID\":" + "{1}," +
            "\"ID_Unit\":" + "{2}," +
            "\"ID_User\":" + "{3}," +
            "\"Position_Cell\":" + "\"{4}\"," +
            "\"Next_Cell\":" + "\"{5}\"," +
            "\"End_Cell\":" + "\"{6}\"," +
            "\"TimeMoveNextCell\":" + "{7}," +
            "\"TimeFinishMove\":" + "{8}," +
            "\"ListMove\":" + "{9}" +
            "}}";

        UserInfoRow user = SyncData.UserInfo.Rows[0];

        moveJson = string.Format(format,
            user.Server_ID,
            agentCtrl.CurrentAgent.ID,
            (int)unit,
            user.ID_User,
            curCellPosition.ToPositionString(),
            tempPath[0].ToPositionString(),
            tempPath[tempPath.Count - 1].ToPositionString(),
            GMath.SecondToMilisecond(GMath.Round(separateTime[0], 3)),
            GMath.SecondToMilisecond(GMath.Round(separateTime.Sum(), 3)),
            GetJsonFrom1(tempPath, separateTime, tempPath[0])
            );

        moveJson = string.Format("{{\"S_MOVE\":{0}}}", moveJson);
    }

    private string GetJsonFrom1(List<Vector3Int> path, List<float> times, Vector3Int curCell)
    {
        const string format =
            "{{" +
            "\"Position_Cell\":\"{0}\"," +
            "\"Next_Cell\":\"{1}\"," +
            "\"TimeMoveNextCell\":{2}" +
            "}}";

        int count = path.Count <= times.Count ? path.Count : times.Count;

        string result = "";
        string curPos = curCell.ToPositionString();
        float lastTime = times[0];

        for (int i = 1; i < count; i++)
        {
            result += string.Format(format, curPos, path[i].ToPositionString(), GMath.SecondToMilisecond(GMath.Round(times[i] + lastTime, 3)));
            if (i < count - 1) result += ",";
            curPos = path[i].ToPositionString();
            lastTime += times[i];
        }

        return string.Format("[{0}]", result);
    }

    private void R_MOVE(SocketIOEvent obj)
    {
        Debugger.Log(obj);
        nCtrlAgentManager.MoveAgent(obj.data["R_MOVE"]);
    }
}

