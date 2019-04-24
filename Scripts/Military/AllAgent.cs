using Entities.Navigation;
using Generic.Singleton;
using DataTable;
using DataTable.Row;
using Network.Data;
using Network.Sync;
using SocketIO;
using System.Collections;
using UnityEngine;
using Generic.Pooling;
using Json;

public class AllAgent : MonoSingle<AllAgent>
{
    public AgentPooling AgentPooling;
    public NonControlAgentManager NCAgentManager;
    public OwnerNavAgentManager OwnerAgents;

    public Sync SyncData;
    public PlayerInfo PlayerInfo;
    public HexMap MapIns;

    [Header("Agent Health Bar")]
    public LightweightLabel LabelPrefab;
    public RectTransform LabelContainer;

    private Pooling<LightweightLabel> labelPooling;
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

        labelPooling = new Pooling<LightweightLabel>(CreateLabel, 10);
       
        Events.On("R_UNIT", CreateAgents);
    }

    private void Start()
    {
        Events.Emit("S_UNIT");
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

        StartCoroutine(StartCreateAgents());
    }

    private LightweightLabel CreateLabel(int id)
    {
        LightweightLabel label = Instantiate(LabelPrefab, LabelContainer);
        label.FirstSetup(id);
        return label;
    }

    public void Create(UnitRow unitData, UserInfoRow user)
    {
        AgentRemote agentRemote = AgentPooling.GetItem(unitData.ID_Unit);
        if (agentRemote == null || user == null)
            return;
        else
        {
            LightweightLabel label = labelPooling.GetItem();

            agentRemote.transform.position = MapIns.CellToWorld(unitData.Position_Cell.Parse3Int().ToClientPosition());

            agentRemote.OnDead += delegate
            {
                AgentPooling.Release(agentRemote.Type, agentRemote);
                labelPooling.Release(label);

                OwnerAgents.Remove(agentRemote.AgentID);
                NCAgentManager.Remove(agentRemote.AgentID);
            };

            label.Initalize(agentRemote, Camera.main);
            bool isOwner = unitData.ID_User == PlayerInfo.Info.ID_User;
            agentRemote.Initalize(UnitTable, label, unitData, user, isOwner);

            if (isOwner)
            {
                if(agentRemote.NavAgent == null)
                    agentRemote.gameObject.AddComponent<NavAgent>();

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

    private IEnumerator StartCreateAgents()
    {
        AJPHelper.Operation oper = UnitTable.Operation;
        while (!oper.IsDone)
            yield return null;

        int i = 0;
        int count = UnitTable.Count;

        UnitRow unitData = null;
        UserInfoRow user = null;

        while (i < count)
        {
            unitData = UnitTable.Rows[i];
            user = Users.GetUserById(unitData.ID_User);
            Create(unitData, user);

            i++;
            yield return null;
        }
        yield break;

    }
}
