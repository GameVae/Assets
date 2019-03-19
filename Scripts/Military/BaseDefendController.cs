using Generic.Singleton;
using ManualTable;
using ManualTable.Row;
using Network.Sync;
using UnityEngine;

public class BaseDefendController : MonoBehaviour
{
    public Connection Conn;
    public AgentSpawnManager Manager;
    public HexMap Map;
    public Sync SyncData;

    private BaseDefendJSONTable[] baseDefends;
    private BaseInfoJSONTable baseInfo;

    private void Start()
    {
        //Map = Singleton.Instance<HexMap>();
        //Conn = Singleton.Instance<Connection>();
        //SyncData = Conn.Sync;

        baseDefends = SyncData.BaseDefends;
        baseInfo = SyncData.BaseInfo;
    }

    private void InitBaseDefend()
    {
        Debugger.Log(baseDefends.Length + " " + baseInfo.Count);
        for (int i = 0; i < baseDefends.Length && i < baseInfo.Count; i++)
        {
            Vector3 basePos = Map.CellToWorld(SyncData.BaseInfo.Rows[i].Position.Parse3Int().ToClientPosition());
            for (int j = 0; j < baseDefends[i].Count; j++)
            {
                BaseDefendRow row = baseDefends[i].Rows[j];
                GameObject agent = Manager.GetMilitary(row.ID_Unit);
                if (agent != null)
                {
                    agent.transform.position = basePos;
                    agent.SetActive(true);
                }
            }
        }
    }
}
