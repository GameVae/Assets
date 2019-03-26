using Entities.Navigation;
using Generic.Singleton;
using ManualTable;
using ManualTable.Row;
using Network.Data;
using Network.Sync;
using SocketIO;
using System.Linq;
using UnityEngine;

public class UnitDataReference : MonoSingle<UnitDataReference>
{
    public AgentSpawnManager AgentSpawner;
    public NonControlAgentManager NCAgentManager;
    public OwnerNavAgentManager OwnerAgents;

    public Sync SyncData;
    public Player PlayerInfo;
    public HexMap MapIns;

    private UnitJSONTable UnitTable;
    private UserInfoJSONTable Users;
    private EventListenersController Events;

    protected override void Awake()
    {
        base.Awake();
        UnitTable = SyncData.UnitTable;
        Users = SyncData.UserInfos;

        PlayerInfo = Singleton.Instance<Player>();
        NCAgentManager = Singleton.Instance<NonControlAgentManager>();
        OwnerAgents = Singleton.Instance<OwnerNavAgentManager>();
        Events = Singleton.Instance<EventListenersController>();

        Events.On("R_UNIT", CreateAgents);
    }

    private void CreateAgents(SocketIOEvent evt)
    {
        int count = UnitTable.Count;
        UnitRow r;
        UserInfoRow user;

        for (int i = 0; i < count; i++)
        {
            r = UnitTable.Rows[i];
            user = Users.GetUser(r.ID_User);
            Create(r, user);
        }
    }

    public void Create(UnitRow r, UserInfoRow user)
    {
        GameObject agent = AgentSpawner.GetMilitary(r.ID_Unit);
        if (agent == null || user == default(UserInfoRow))
            return;
        else
        {
            agent.transform.position = MapIns.CellToWorld(r.Position_Cell.Parse3Int().ToClientPosition());

            NavRemote agentRemote = agent.GetComponent<NavRemote>();
            bool isOwner = r.ID_User == PlayerInfo.Info.ID_User;

            agentRemote.SetUnitData(r, user, isOwner);
            if (!isOwner)
            {
                FixedMovement nav = agent.GetComponent<FixedMovement>();
                NCAgentManager.Add(r.ID, nav);
                agent.name = "Other " + r.ID;
            }
            else
            {
                agent.AddComponentNotExist<NavAgent>();
                OwnerAgents.Add(agentRemote);
                agent.name = "Owner " + r.ID;
            }

            agent.SetActive(true);
        }
    }
}
