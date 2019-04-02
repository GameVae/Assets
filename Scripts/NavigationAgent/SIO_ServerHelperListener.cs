using Entities.Navigation;
using Generic.Singleton;
using PathFinding;
using SocketIO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SIO_ServerHelperListener : Listener
{
    private int maxDeep;
    private float maxSpeed;

    private HexMap mapIns;
    private AStartAlgorithm aStar;
    private BreathFirstSearch breathFS;
    private NonControlAgentManager nonCtrlAgents;

    private bool isInited;
    private FixedMovement agent;
    private Vector3Int agentTargetPosition;

    public NavOffset SoldierOffset;

    public override void RegisterCallback()
    {
        On("R_NEW_POS", R_NEW_POS);
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
        int serId = -1;
        obj.data["R_NEW_POS"].GetField(ref id, "ID");
        obj.data["R_NEW_POS"].GetField(ref serId, "Server_ID");

        if (id != -1)
        {
            agentTargetPosition = FindNextValidPosition(id);
            bool foundPath = aStar.FindPath(agent.CurrentPosition, agentTargetPosition);

            if (foundPath &&
                mapIns.IsValidCell(agentTargetPosition.x,agentTargetPosition.y))
            {
                string data = ResponseMessage(
                    clientPath:         aStar.Path,
                    separateTime:       GetTimes(aStar.Path,agent.transform.position), 
                    curCellPosition:    agent.CurrentPosition,
                    id:                 id,
                    serId:              serId);

                JSONObject moveObject = new JSONObject(JSONObject.Type.BAKED)
                {
                    str = data
                };

                Emit("S_MOVE", moveObject);
            }
            else Debugger.Log("Path not found");
        }
    }

    private void Init()
    {
        InitalizeOffset();

        mapIns = Singleton.Instance<HexMap>();
        breathFS = Singleton.Instance<BreathFirstSearch>();
        aStar = new AStartAlgorithm(mapIns, maxDeep);
        nonCtrlAgents = Singleton.Instance<NonControlAgentManager>();
    }

    private void InitalizeOffset()
    {
        maxDeep = SoldierOffset.MaxSearchLevel;
        maxSpeed = SoldierOffset.MaxSpeed;
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

    public List<float> GetTimes(List<Vector3Int> path,Vector3 curPos)
    {
        float distance = 0.0f;
        List<Vector3Int> currentPath = path;
        List<float> separateTime = new List<float>();

        Vector3 currentPosition = curPos;

        for (int i = currentPath.Count; i > 0; i--)
        {
            Vector3 nextPosition = mapIns.CellToWorld(currentPath[i - 1]);
            distance = Vector3.Distance(currentPosition, nextPosition);
            separateTime.Add(distance / maxSpeed); // time
            currentPosition = nextPosition;
        }
        return separateTime;
    }

    private string ResponseMessage(List<Vector3Int> clientPath,
        List<float> separateTime, Vector3Int curCellPosition, int id, int serId)
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
            serId,
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
