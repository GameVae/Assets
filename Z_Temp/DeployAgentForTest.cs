using DataTable.Row;
using Entities.Navigation;
using EnumCollect;
using Generic.CustomInput;
using Generic.Singleton;
using Network.Data;
using Network.Sync;
using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployAgentForTest : MonoBehaviour
{
    public ListUpgrade type;
    public List<Vector3Int> positions;
    public Sync SyncData;

    public CursorController cursor;
    public bool isStartRecord;

    public void Awake()
    {
        if (positions == null)
            positions = new List<Vector3Int>();
        cursor.SelectedCallback += CursorSelectCallback;
    }

    private MyAgentRemoteManager myAgent;
    public MyAgentRemoteManager MyAgent
    {
        get
        {
            return myAgent ?? (myAgent = Singleton.Instance<MyAgentRemoteManager>());
        }
    }

    public void S_DEPLOY()
    {
        isStartRecord = false;
        if (positions.Count == 0) return;
        else
        {
            UserInfoRow user = SyncData.MainUser;
            BaseInfoRow baseInfo = SyncData.CurrentMainBase;
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "Server_ID"   ,user.Server_ID },
                { "ID_User"     ,user.ID_User.ToString()},
                { "ID_Unit"     ,((int)type).ToString() },
                { "Quality"     ,"6" },
                { "BaseNumber"  ,baseInfo.BaseNumber.ToString()}
            };
            JSONObject packet = new JSONObject(data);
            Debugger.Log(packet);

            Singleton.Instance<EventListenersController>().Emit("S_DEPLOY", packet);
            //return packet;
        }
    }

    public void StartRecord()
    {
        isStartRecord = true;
    }

    public void R_DEPLOY(SocketIOEvent obj)
    {
        //Debugger.Log(obj);
        string json = obj.data["R_DEPLOY"].ToString();
        UnitRow unit = JsonUtility.FromJson<UnitRow>(json);

        AgentRemote agent = MyAgent.GetNavRemote(unit.ID);
        int posCount = positions.Count;
        if (agent != null && posCount > 0)
        {
            agent.NavAgent.AsyncStartMove(agent.CurrentPosition, positions[posCount - 1], null);
            positions.RemoveAt(posCount - 1);
            if (positions.Count > 0)
            {
                Debugger.Log("start coroutine");
                StartCoroutine(DeployCoroutine());
            }
        }
        else
        {
            Debugger.Log("Agent null " + unit.ID);
        }
    }

    private void CursorSelectCallback(Vector3Int pos)
    {
        if (isStartRecord)
        {
            positions.Add(pos);
            Debugger.Log("Record " + pos);
        }
    }

    private IEnumerator DeployCoroutine()
    {
        yield return new WaitForSeconds(2.0f); ;
        S_DEPLOY();
    }
}
