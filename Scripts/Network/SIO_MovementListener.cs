using Entities.Navigation;
using EnumCollect;
using Generic.Singleton;
using DataTable.Row;
using SocketIO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SIO_MovementListener : Listener
{
    private NonControlAgentManager nCtrlAgentManager;
    private OwnerNavAgentManager ownerAgentManager;
    private JSONObject moveJSONObject;
    
    private string moveJson;

    protected OwnerNavAgentManager OwnerAgentManager
    {
        get
        {
            return ownerAgentManager ?? (ownerAgentManager = Singleton.Instance<OwnerNavAgentManager>());
        }
    }

    protected override void Start()
    {
        base.Start();
        nCtrlAgentManager = Singleton.Instance<NonControlAgentManager>();
        moveJSONObject = new JSONObject(JSONObject.Type.BAKED);

        // Emit("S_UNIT");
    }

    public override void RegisterCallback()
    {
        AddEmiter("S_MOVE", S_MOVE);
        AddEmiter("S_UNIT", S_UNIT);

        //On("R_MOVE", R_MOVE);
    }

    private JSONObject S_UNIT()
    {
        // Debugger.Log("S_UNIT: " + Time.realtimeSinceStartup);

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"ID_User",SyncData.MainUser.ID_User.ToString() },
            {"Server_ID",SyncData.MainUser.Server_ID.ToString() },
        };
        return new JSONObject(data);
    }

    private JSONObject S_MOVE()
    {
        moveJSONObject.Clear();
        moveJSONObject.type = JSONObject.Type.BAKED;
        moveJSONObject.str = moveJson;

        Debugger.Log(moveJSONObject);
        return moveJSONObject;
    }

    public void Move(List<Vector3Int> path,
        List<float> separateTime,
        Vector3Int curCellPosition,
        ListUpgrade unit,
        int id)
    {
        InitMessage(path, separateTime, curCellPosition, unit,id);
        Emit("S_MOVE"); ;
    }

    private void InitMessage(List<Vector3Int> clientPath, 
        List<float> separateTime, Vector3Int curCellPosition, ListUpgrade unit,int id)
    {
        curCellPosition = curCellPosition.ToSerPosition();

        List<Vector3Int> tempPath = new List<Vector3Int>(clientPath);
        for (int i = 0; i < tempPath.Count; i++)
        {
            tempPath[i] = tempPath[i].ToSerPosition();
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

        UserInfoRow user = SyncData.MainUser;
        Vector3Int endPosition = tempPath[tempPath.Count - 1];
        


        moveJson = string.Format(format,
            user.Server_ID,
            id,
            (int)unit,
            user.ID_User,
            curCellPosition.ToPositionString(),
            tempPath[0].ToPositionString(),
            endPosition.ToPositionString(),
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

    public void R_MOVE(SocketIOEvent obj)
    {
        Debugger.Log("R_MOVE");
        // Debugger.Log(obj);
        JSONObject r_move = obj.data["R_MOVE"];
        bool isOther = nCtrlAgentManager.MoveAgent(r_move);
        //if (!isOther)
        //{
        //    int id = -1;
        //    r_move.GetField(ref id, "ID");
        //    OwnerAgentManager.GetNavRemote(id)?.FixedMove.StartMove(r_move);
        //}
    }
}

