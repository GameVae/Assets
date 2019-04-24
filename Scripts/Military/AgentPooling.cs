using Entities.Navigation;
using EnumCollect;
using Generic.Pooling;
using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public sealed class AgentPooling : MonoSingle<AgentPooling>
{
    private Dictionary<int, Pooling<AgentRemote>> agentsPool;

    private Dictionary<int, AgentRemote> agentPrefabs;

    private AssetUtils assetUtil;

    public Transform Container;

    public AssetUtils AssetUtil
    {
        get
        {
            return assetUtil ?? (assetUtil = Singleton.Instance<AssetUtils>());
        }
    }
    public Dictionary<int, Pooling<AgentRemote>>  AgentPools
    {
        get
        {
            return agentsPool ?? (agentsPool = new Dictionary<int, Pooling<AgentRemote>>());
        }
    }
    private Dictionary<int, AgentRemote> AgentPrefabs
    {
        get
        {
            return agentPrefabs ?? (agentPrefabs = new Dictionary<int, AgentRemote>());
        }
    }
    
    protected override void Awake()
    {
        base.Awake();
        LoadAgents();
    }

    public AgentRemote GetItem(ListUpgrade type)
    {
        if (AgentPools.TryGetValue(type.GetHashCode(), out Pooling<AgentRemote> pool))
        {
            return pool.GetItem();
        }
        else
        {            
            return CreatePool(type).GetItem();
        }
    }

    public void Release(ListUpgrade type, AgentRemote agent)
    {
        if (AgentPools.TryGetValue(type.GetHashCode(), out Pooling<AgentRemote> pool))
        {
            pool.Release(agent);
        }
        else
        {
            CreatePool(type).Release(agent);
        }
    }

    private void LoadAgents()
    {
        Object[] objs = AssetUtil.Load(@"Prefabs\Agents");
        for (int i = 0; i < objs.Length; i++)
        {
            GameObject go = objs[i] as GameObject;
            AgentRemote remote = go.GetComponent<AgentRemote>();
            int hashCode = remote.Type.GetHashCode();
            AgentPrefabs[hashCode] = remote;
        }
    }

    private AgentRemote Create(ListUpgrade type)
    {
        AgentPrefabs.TryGetValue(type.GetHashCode(), out AgentRemote res);
        if (res == null)
            return null;
        return Instantiate(res, Container);
    }

    private Pooling<AgentRemote> CreatePool(ListUpgrade type)
    {
        int hashCode = type.GetHashCode();

        AgentPools[hashCode] = new Pooling<AgentRemote>(
            delegate (int insId)
            {
                return CreatePoolingItem(insId, type);
            });

        return AgentPools[hashCode];
    }

    private AgentRemote CreatePoolingItem(int id,ListUpgrade type)
    {
        AgentRemote remote = Create(type);
        remote?.FirstSetup(id);
        return remote;
    }
}
