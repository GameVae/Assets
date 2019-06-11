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
using System.Collections.Generic;

public class AgentRemoteManager : MonoSingle<AgentRemoteManager>
{

    [SerializeField] private Camera mainCamera;
    public MyAgentRemoteManager MyAgentRemoteManager;

    public Sync SyncData;
    public PlayerInfo PlayerInfo;
    public HexMap MapIns;

    [Header("Agent Health Bar")]
    public LightweightLabel LabelPrefab;
    public RectTransform LabelContainer;

    private AgentPooling agentPooling;
    private Pooling<LightweightLabel> labelPooling;
    private JSONTable_Unit UnitTable;
    private JSONTable_UserInfo Users;
    private EventListenersController Events;

    private Dictionary<int, AgentRemote> allAgents;

    public Camera MainCamera
    {
        get
        {
            return mainCamera ?? (mainCamera = Camera.main);
        }
    }
    public AgentPooling AgentPooling
    {
        get
        {
            return agentPooling ?? (agentPooling = Singleton.Instance<AgentPooling>());
        }
    }
    public Dictionary<int, AgentRemote> AllAgents
    {
        get
        {
            return allAgents ?? (allAgents = new Dictionary<int, AgentRemote>());
        }
    }

    protected override void Awake()
    {
        base.Awake();
        UnitTable = SyncData.UnitTable;
        Users = SyncData.UserInfos;

        PlayerInfo = Singleton.Instance<PlayerInfo>();
        MyAgentRemoteManager = Singleton.Instance<MyAgentRemoteManager>();
        Events = Singleton.Instance<EventListenersController>();

        labelPooling = new Pooling<LightweightLabel>(CreateLabel, 10);      
        Events.On("R_UNIT", R_UNIT_CREATE_UNIT);

    }

    private void Start()
    {
        Events.Emit("S_UNIT");
    }

    private void R_UNIT_CREATE_UNIT(SocketIOEvent evt)
    {
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
            agentRemote.Dispose();
            this.AddAgent(unitData.ID, agentRemote);

            LightweightLabel label = labelPooling.GetItem();
            agentRemote.transform.position = MapIns.CellToWorld(unitData.Position_Cell.Parse3Int().ToClientPosition());

            agentRemote.OnDead += delegate
            {
                AgentPooling.Release(agentRemote.Type, agentRemote);
                labelPooling.Release(label);

                MyAgentRemoteManager.Remove(agentRemote.AgentID);
                this.RemoveAgent(agentRemote.AgentID);
            };

            label.Initalize(agentRemote, MainCamera);
            bool isOwner = unitData.ID_User == PlayerInfo.Info.ID_User;

            agentRemote.Initalize(UnitTable, label, unitData, user, isOwner);

            if (isOwner)
            {
                if (agentRemote.NavAgent == null)
                {
                    agentRemote.gameObject.AddComponent<NavAgent>();
                }
                MyAgentRemoteManager.Add(agentRemote);
                agentRemote.name = "Owner " + unitData.ID;
            }
            else
            {
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
            unitData = UnitTable.ReadOnlyRows[i];
            user = Users.GetUserById(unitData.ID_User);
            Create(unitData, user);

            i++;
            yield return null;
        }
        yield break;

    }

    private bool RemoveAgent(int id)
    {
        return AllAgents.Remove(id);
    }

    private bool AddAgent(int id,AgentRemote remote)
    {
        if(!AllAgents.ContainsKey(id))
        {
            AllAgents.Add(id, remote);
            return true;
        }
        return false;
    }   

    public bool IsOwnerAgent(int id)
    {
        return MyAgentRemoteManager.IsOwnerAgent(id);
    }

    public AgentRemote GetAgentRemote(int id)
    {
        AllAgents.TryGetValue(id, out AgentRemote value);
        return value;
    }

}
