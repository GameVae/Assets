using Entities.Navigation;
using EnumCollect;
using Generic.Pooling;
using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public sealed class AgentSpawnManager : MonoSingle<AgentSpawnManager>
{
    private Dictionary<int, Pooling<NavRemote>> agentsPool;

    private Dictionary<int, Object> agents;

    private AssetUtils AssetUtil;

    public Transform Container;

    protected override void Awake()
    {
        base.Awake();
        AssetUtil = Singleton.Instance<AssetUtils>();
        agentsPool = new Dictionary<int, Pooling<NavRemote>>();
        agents = new Dictionary<int, Object>();
        LoadAgents();
    }

    public NavRemote GetMilitary(ListUpgrade type)
    {
        if (agentsPool.TryGetValue(type.GetHashCode(), out Pooling<NavRemote> pool))
        {
            return pool.GetItem();
        }
        else
        {            
            return CreatePool(type).GetItem();
        }
    }

    public void Return(ListUpgrade type, NavRemote agent)
    {
        if (agentsPool.TryGetValue(type.GetHashCode(), out Pooling<NavRemote> pool))
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
            int hashCode = go.GetComponent<NavRemote>().Type.GetHashCode();
            agents[hashCode] = objs[i];
        }
    }

    private NavRemote Create(ListUpgrade type)
    {
        agents.TryGetValue(type.GetHashCode(), out Object res);
        if (res == null)
            return null;
        return Instantiate(res as GameObject, Container).GetComponent<NavRemote>();
    }

    private Pooling<NavRemote> CreatePool(ListUpgrade type)
    {
        int hashCode = type.GetHashCode();

        agentsPool[hashCode] = new Pooling<NavRemote>();

        agentsPool[hashCode].Initalize(delegate (int insId)
        {
            NavRemote remote = Create(type);
            remote.FirstSetup(insId);
            return remote;
        }
        );
        return agentsPool[hashCode];
    }
}
