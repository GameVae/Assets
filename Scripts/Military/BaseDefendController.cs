using Generic.Singleton;
using DataTable;
using DataTable.Row;
using Network.Sync;
using UnityEngine;
using Entities.Navigation;

public class BaseDefendController : MonoBehaviour
{
    public Connection Conn;
    public AgentPooling Manager;
    public HexMap Map;
    public Sync SyncData;

    private JSONTable_BaseDefend[] baseDefends;
    private JSONTable_BaseInfo baseInfo;

    private void Start()
    {
        //Map = Singleton.Instance<HexMap>();
        //Conn = Singleton.Instance<Connection>();
        //SyncData = Conn.Sync;

        baseDefends = SyncData.BaseDefends;
        baseInfo = SyncData.BaseInfos;
    }

    private void InitBaseDefend()
    {
        Debugger.Log(baseDefends.Length + " " + baseInfo.Count);
        for (int i = 0; i < baseDefends.Length && i < baseInfo.Count; i++)
        {
            Vector3 basePos = Map.CellToWorld(SyncData.BaseInfos.Rows[i].Position.Parse3Int().ToClientPosition());
            for (int j = 0; j < baseDefends[i].Count; j++)
            {
                BaseDefendRow row = baseDefends[i].Rows[j];
                NavRemote agent = Manager.GetItem(row.ID_Unit);
                if (agent != null)
                {
                    agent.transform.position = basePos;
                    agent.gameObject.SetActive(true);
                }
            }
        }
    }
}
