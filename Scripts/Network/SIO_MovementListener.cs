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
    private string moveJson;
    private JSONObject moveJSONObject;
    private AgentRemoteManager agentManager;
    private MyAgentRemoteManager ownerAgentManager;

    protected MyAgentRemoteManager MyAgentManager
    {
        get
        {
            return ownerAgentManager ?? (ownerAgentManager = Singleton.Instance<MyAgentRemoteManager>());
        }
    }
    public AgentRemoteManager AgentManager
    {
        get { return agentManager ?? (agentManager = Singleton.Instance<AgentRemoteManager>()); }
    }

    protected override void Start()
    {
        base.Start();
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
        AgentRemote ownerRemote,
        AgentRemote otherRemote)
    {
        InitMessage(path, separateTime, curCellPosition, ownerRemote, otherRemote);
        Emit("S_MOVE"); ;
    }

    private void InitMessage(
        List<Vector3Int> clientPath,
        List<float> separateTime,
        Vector3Int curPosition,
        AgentRemote ownerRemote,
        AgentRemote enemyRemote)
    {
        curPosition = curPosition.ToSerPosition();

        List<Vector3Int> serverPath = new List<Vector3Int>(clientPath);
        for (int i = 0; i < serverPath.Count; i++)
        {
            serverPath[i] = serverPath[i].ToSerPosition();
        }

        serverPath = serverPath.Invert();
        int ownerUnitType = (int)ownerRemote.Type;
        int ownerAgentId = ownerRemote.AgentID;

        string attack_unit_id = "NULL";

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
            "\"ListMove\":" + "{9}," +
            "\"Attack_Unit_ID\":" + "\"{10}\"" +
            "}}";

        UserInfoRow user = ownerRemote.UserInfo;
        Vector3Int endPosition = serverPath[serverPath.Count - 1];

        if (enemyRemote != null)
        {
            UserInfoRow otherUser = enemyRemote.UserInfo;

            attack_unit_id = string.Format("{0}_{1}_{2}_{3}",
                user.Server_ID,
                (int)enemyRemote.Type,
                otherUser.ID_User,
                enemyRemote.AgentID);
        }

        moveJson = string.Format(format,
            user.Server_ID,
            ownerAgentId,
            ownerUnitType,
            user.ID_User,
            curPosition.ToPositionString(),
            serverPath[0].ToPositionString(),
            endPosition.ToPositionString(),
            GMath.SecondToMilisecond(GMath.Round(separateTime[0], 3)),
            GMath.SecondToMilisecond(GMath.Round(separateTime.Sum(), 3)),
            GetJsonFrom1(serverPath, separateTime, serverPath[0]),
            attack_unit_id
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
        //Debugger.Log(obj.ToString().Substring(0,150));
        JSONObject r_move = obj.data["R_MOVE"];
        int id = -1;
        r_move.GetField(ref id, "ID");
        if (!MyAgentManager.IsOwnerAgent(id))
        {
            AgentRemote agent = AgentManager.GetAgentRemote(id);
            agent?.FixedMove.StartMove(r_move);
        }
    }

    public void R_TESTMOVE(SocketIOEvent obj)
    {
        JSONObject test = obj.data["R_TESTMOVE"];
        if (test != null)
        {
            Debugger.Log(test);
        }
    }
}

