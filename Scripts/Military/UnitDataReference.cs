using Generic.Singleton;
using ManualTable;
using ManualTable.Row;
using Network.Sync;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitDataReference : MonoSingle<UnitDataReference>
{
    public AgentSpawnManager AgentSpawner;
    public Sync SyncData; 
    public Player Player;
    public HexMap HexMap;

    private UnitJSONTable UnitTable;
    private UserInfoJSONTable Users;
    protected override void Awake()
    {
        base.Awake();
        UnitTable = SyncData.UnitTable;
        Users = SyncData.UserInfo;

        Player = Singleton.Instance<Player>();
        UnitAgents();
    }

    private void UnitAgents()
    {
        int count = UnitTable.Count;
        UnitRow r;
        UserInfoRow user;

        for (int i = 0; i < count; i++)
        {
            r = UnitTable.Rows[i];
            user = Users.Rows.FirstOrDefault(u => u.ID_User == r.ID_User);
            Create(r, user);
        }
    }

    public void Create(UnitRow r,UserInfoRow user)
    {
        GameObject agent = AgentSpawner.GetMilitary(r.ID_Unit);
        if (agent == null || user == default(UserInfoRow))
            return;
        else
        {
            agent.transform.position = HexMap.CellToWorld(r.Position_Cell.Parse3Int() + new Vector3Int(5, 5, 0));
            agent.GetComponent<AgentController>().SetData(r);
            agent.GetComponent<AgentController>().SetCurrentUser(user);
            agent.SetActive(true);
        }
    }
}
