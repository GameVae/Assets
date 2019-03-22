using Entities.Navigation;
using EnumCollect;
using Generic.Singleton;
using PathFinding;
using SocketIO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SIO_ServerHelperListener : Listener
{
    [SerializeField]
    private int maxDeep;
    [SerializeField]
    private float maxSpeed;

    private NonControlAgentManager nonCtrlAgents;

    private BreathFirstSearch breathFS;
    private AStartAlgorithm aStar;
    private HexMap mapIns;

    private bool isInited;

    private Vector3Int agentNextPosition;
    private FixedMovement agent;
    private JSONObject moveObject;
    private Connection Conn;

    public override void RegisterCallback()
    {
        // AddEmiter("S_NEW_POS", S_NEW_POS);

        On("R_NEW_POS", R_NEW_POS);
    }

    private JSONObject S_NEW_POS()
    {
        
        return moveObject;
    }

    public void R_NEW_POS(SocketIOEvent obj)
    {
        Debugger.Log(obj);
        if (!isInited)
        {
            Init();
            isInited = true;
        }
        int id = -1;
        obj.data["R_NEW_POS"].GetField(ref id, "ID");
        if (id != -1)
        {
            agentNextPosition = FindNextValidPosition(id);
            bool foundPath = aStar.FindPath(agent.CurrentPosition, agentNextPosition);

            if (foundPath &&
                agentNextPosition != Generic.Contants.Constants.InvalidPosition)
            {
                string json = InitMessage(aStar.Path, GetTimes(aStar.Path), agent.CurrentPosition, id);

                moveObject.Clear();
                moveObject.type = JSONObject.Type.BAKED;
                moveObject.str = json;

                Conn.Emit("S_MOVE", moveObject);
            }
            else Debugger.Log("Path not found");
        }
    }

    private void Init()
    {
        mapIns = Singleton.Instance<HexMap>();
        breathFS = Singleton.Instance<BreathFirstSearch>();
        aStar = new AStartAlgorithm(mapIns, maxDeep);
        moveObject = new JSONObject();
        nonCtrlAgents = Singleton.Instance<NonControlAgentManager>();
        Conn = Singleton.Instance<Connection>();
    }

    private Vector3Int FindNextValidPosition(int unitId)
    {
        agent = nonCtrlAgents.GetAgent(unitId);
        if (agent != null)
        {
            breathFS.GetNearestCell(agent.CurrentPosition, out Vector3Int res);
            return res;
        }
        return Generic.Contants.Constants.InvalidPosition;
    }

    public List<float> GetTimes(List<Vector3Int> path)
    {
        float distance = 0.0f;
        List<Vector3Int> currentPath = path;
        List<float> separateTime = new List<float>();

        Vector3 currentPosition = transform.position;

        for (int i = currentPath.Count; i > 0; i--)
        {
            Vector3 nextPosition = mapIns.CellToWorld(currentPath[i - 1]);
            distance = Vector3.Distance(currentPosition, nextPosition);
            separateTime.Add(distance / maxSpeed); // time
            currentPosition = nextPosition;
        }
        return separateTime;
    }

    private string InitMessage(List<Vector3Int> clientPath,
        List<float> separateTime, Vector3Int curCellPosition, int id)
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

        string moveJson = string.Format(format,
            "1",
            id,
            (int)agent.Remote.Type,
            agent.Remote.UnitData.ID_User,
            curCellPosition.ToPositionString(),
            tempPath[0].ToPositionString(),
            tempPath[tempPath.Count - 1].ToPositionString(),
            GMath.SecondToMilisecond(GMath.Round(separateTime[0], 3)),
            GMath.SecondToMilisecond(GMath.Round(separateTime.Sum(), 3)),
            GetJsonFrom1(tempPath, separateTime, tempPath[0])
            );

        return moveJson = string.Format("{{\"S_MOVE\":{0}}}", moveJson);
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
}
