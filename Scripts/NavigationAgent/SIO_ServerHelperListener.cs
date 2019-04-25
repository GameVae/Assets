using Entities.Navigation;
using Generic.Singleton;
using MultiThread;
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
    private JSONObject r_new_pos;
    private BreathFirstSearch breathFS;
    private AStarAlgorithm aStarAlgorithm;
    private AgentRemoteManager agentManager;

    private bool isInited;
    private AgentRemote agentRemote;
    private Vector3Int agentTargetPosition;
    private MultiThreadHelper threadHelper;

    public AgentRemoteManager AgentManager
    {
        get
        {
            return agentManager ?? (agentManager = Singleton.Instance<AgentRemoteManager>());
        }
    }
    private MultiThreadHelper ThreadHelper
    {
        get { return threadHelper ?? (threadHelper = Singleton.Instance<MultiThreadHelper>()); }
    }
    public NavOffset SoldierOffset;

    #region REGISTER EVENTS
    public override void RegisterCallback()
    {
        On("R_NEW_POS", R_NEW_POS);
    }

    private void R_NEW_POS(SocketIOEvent obj)
    {
        Debugger.Log(obj);
        if (!isInited)
        {
            Init();
            isInited = true;
        }

        int id = -1;
        r_new_pos = obj.data["R_NEW_POS"];
        r_new_pos.GetField(ref id, "ID");
        if (id != -1 && id != 0)
        {
            agentTargetPosition = FindNextValidPosition(id);
            AStarAlgorithm.FindInfo info = new AStarAlgorithm.FindInfo()
            {
                StartPosition = agentRemote.CurrentPosition,
                EndPosition = agentTargetPosition,
                DoneCallback = FindPathDoneCallback
            };

            aStarAlgorithm.FindPath(info);
        }
    }
    #endregion

    #region INITALIZE
    private void Init()
    {
        InitalizeOffset();

        mapIns = Singleton.Instance<HexMap>();
        breathFS = Singleton.Instance<BreathFirstSearch>();
        aStarAlgorithm = new AStarAlgorithm(mapIns, maxDeep);
    }

    private void InitalizeOffset()
    {
        maxDeep = SoldierOffset.MaxSearchLevel;
        maxSpeed = SoldierOffset.MaxSpeed;
    }
    #endregion

    #region FORMAT DATA
    private Vector3Int FindNextValidPosition(int unitId)
    {
        bool isOwnerAgent = AgentManager.IsOwnerAgent(unitId);
        if (!isOwnerAgent)
        {
            agentRemote = AgentManager.GetAgentRemote(unitId);
            if (agentRemote != null)
            {
                breathFS.GetNearestCell(agentRemote.CurrentPosition, out Vector3Int res);
                return res;
            }
        }
        return Generic.Contants.Constants.InvalidPosition;
    }

    private List<float> GetTimes(List<Vector3Int> path, Vector3 curPos)
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
            (int)agentRemote.Type,
            agentRemote.UnitInfo.ID_User,
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
    #endregion

    #region MULTI-THREAD CALLBACK
    private void FindPathDoneCallback(AStarAlgorithm aStar, bool found)
    {
        ThreadHelper.MainThreadInvoke(() =>
        {
            FindPathDone(aStar, found);
        });
    }

    private void FindPathDone(AStarAlgorithm aStar, bool found)
    {
        int id = -1;
        int serId = -1;
        r_new_pos.GetField(ref id, "ID");
        r_new_pos.GetField(ref serId, "Server_ID");

        if (found)
        {
            string data = ResponseMessage(
                clientPath: aStar.Path,
                separateTime: GetTimes(aStar.Path, agentRemote.transform.position),
                curCellPosition: agentRemote.CurrentPosition,
                id: id,
                serId: serId);

            JSONObject moveObject = new JSONObject(JSONObject.Type.BAKED)
            {
                str = data
            };

            Emit("S_MOVE", moveObject);
        }
        else Debugger.Log("Path not found");
    }
    #endregion
}
