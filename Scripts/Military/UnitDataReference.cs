using Entities.Navigation;
using Generic.Singleton;
using DataTable;
using DataTable.Row;
using Network.Data;
using Network.Sync;
using SocketIO;
using System.Collections;
using UnityEngine;

public class UnitDataReference : MonoSingle<UnitDataReference>
{
    public AgentSpawnManager AgentSpawner;
    public NonControlAgentManager NCAgentManager;
    public OwnerNavAgentManager OwnerAgents;

    public Sync SyncData;
    public PlayerInfo PlayerInfo;
    public HexMap MapIns;

    private JSONTable_Unit UnitTable;
    private JSONTable_UserInfo Users;
    private EventListenersController Events;

    protected override void Awake()
    {
        base.Awake();
        UnitTable = SyncData.UnitTable;
        Users = SyncData.UserInfos;

        PlayerInfo = Singleton.Instance<PlayerInfo>();
        NCAgentManager = Singleton.Instance<NonControlAgentManager>();
        OwnerAgents = Singleton.Instance<OwnerNavAgentManager>();
        Events = Singleton.Instance<EventListenersController>();

        Events.On("R_UNIT", CreateAgents);
    }

    private void CreateAgents(SocketIOEvent evt)
    {
        //int count = UnitTable.Count;
        //UnitRow r;
        //UserInfoRow user;

        //for (int i = 0; i < count; i++)
        //{
        //    r = UnitTable.Rows[i];
        //    user = Users.GetUser(r.ID_User);
        //    Create(r, user);
        //}

        StartCoroutine(AsyncCreateAgents());
    }

    public void Create(UnitRow unitData, UserInfoRow user)
    {
        NavRemote agentRemote = AgentSpawner.GetMilitary(unitData.ID_Unit);
        if (agentRemote == null || user == null)
            return;
        else
        {
            agentRemote.transform.position = MapIns.CellToWorld(unitData.Position_Cell.Parse3Int().ToClientPosition());
            agentRemote.OnDead += delegate
            {
                AgentSpawner.Return(agentRemote.Type, agentRemote);
            };

            bool isOwner = unitData.ID_User == PlayerInfo.Info.ID_User;

            agentRemote.Initalize(UnitTable, unitData, user, isOwner);

            if (isOwner)
            {
                agentRemote.gameObject.AddComponentNotExist<NavAgent>();
                OwnerAgents.Add(agentRemote);
                agentRemote.name = "Owner " + unitData.ID;
            }
            else
            {
                FixedMovement nav = agentRemote.FixedMove;
                NCAgentManager.Add(unitData.ID, nav);
                agentRemote.name = "other " + unitData.ID;
            }

            agentRemote.gameObject.SetActive(true);
        }
    }

    private IEnumerator AsyncCreateAgents()
    {
        int i = 0;
        int count = UnitTable.Count;

        UnitRow unitData = null;
        UserInfoRow user = null;

        while (i < count)
        {
            unitData = UnitTable.Rows[i];
            user = Users.GetUser(unitData.ID_User);
            Create(unitData, user);

            i++;
            yield return null;
        }
        yield break;
    }
}
