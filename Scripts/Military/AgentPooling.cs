using Entities.Navigation;
using EnumCollect;
using Generic.Pooling;
using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public sealed class AgentPooling : MonoSingle<AgentPooling>
{
    private Dictionary<int, Pooling<NavRemote>> agentsPool;

    private Dictionary<int, NavRemote> agentPrefabs;

    private AssetUtils assetUtil;

    public Transform Container;

    public AssetUtils AssetUtil
    {
        get
        {
            return assetUtil ?? (assetUtil = Singleton.Instance<AssetUtils>());
        }
    }
    public Dictionary<int, Pooling<NavRemote>>  AgentPools
    {
        get
        {
            return agentsPool ?? (agentsPool = new Dictionary<int, Pooling<NavRemote>>());
        }
    }
    private Dictionary<int, NavRemote> AgentPrefabs
    {
        get
        {
            return agentPrefabs ?? (agentPrefabs = new Dictionary<int, NavRemote>());
        }
    }
    
    protected override void Awake()
    {
        base.Awake();
        LoadAgents();
    }

    public NavRemote GetItem(ListUpgrade type)
    {
        if (AgentPools.TryGetValue(type.GetHashCode(), out Pooling<NavRemote> pool))
        {
            return pool.GetItem();
        }
        else
        {            
            return CreatePool(type).GetItem();
        }
    }

    public void Release(ListUpgrade type, NavRemote agent)
    {
        if (AgentPools.TryGetValue(type.GetHashCode(), out Pooling<NavRemote> pool))
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
            NavRemote remote = go.GetComponent<NavRemote>();
            int hashCode = remote.Type.GetHashCode();
            AgentPrefabs[hashCode] = remote;
        }
    }

    private NavRemote Create(ListUpgrade type)
    {
        AgentPrefabs.TryGetValue(type.GetHashCode(), out NavRemote res);
        if (res == null)
            return null;
        return Instantiate(res, Container);
    }

    private Pooling<NavRemote> CreatePool(ListUpgrade type)
    {
        int hashCode = type.GetHashCode();

        AgentPools[hashCode] = new Pooling<NavRemote>(
            delegate (int insId)
            {
                return CreatePoolingItem(insId, type);
            });

        return AgentPools[hashCode];
    }

    private NavRemote CreatePoolingItem(int id,ListUpgrade type)
    {
        NavRemote remote = Create(type);
        remote?.FirstSetup(id);
        return remote;
    }
}
