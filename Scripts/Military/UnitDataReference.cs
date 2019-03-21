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
    public Player Player;
    public HexMap HexMap;

    private UnitJSONTable UnitTable;
    private UserInfoJSONTable Users;
    private EventListenersController Events;
    protected override void Awake()
    {
        base.Awake();
        UnitTable = SyncData.UnitTable;
        Users = SyncData.UserInfo;

        Player = Singleton.Instance<Player>();
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
            user = Users.Rows.FirstOrDefault(u => u.ID_User == r.ID_User);

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
            agent.transform.position = HexMap.CellToWorld(r.Position_Cell.Parse3Int().ToClientPosition());

            NavRemote agentRemote = agent.GetComponent<NavRemote>();
            bool isOwner = r.ID_User == Player.Info.ID_User;

            agentRemote.SetUnitData(r, user, isOwner);
            if (!isOwner)
            {
                FixedMovement nav = agent.GetComponent<FixedMovement>();
                NCAgentManager.Add(r.ID, nav);
                agent.name = "Other " + r.Position_Cell.Parse3Int().ToClientPosition();
            }
            else
            {
                agent.AddComponentNotExist<NavAgent>();
                OwnerAgents.Add(agentRemote);
                agent.name = "Owner " + r.Position_Cell.Parse3Int().ToClientPosition();
            }

            agent.SetActive(true);
        }
    }
}
