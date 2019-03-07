using EnumCollect;
using Generic.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public sealed class AgentSpawnManager : MonoSingle<AgentSpawnManager>
{
    private Dictionary<int,Queue<GameObject>> agentsPool;

    private Dictionary<int, Object> agents;

    public AssetUtils AssetUtil;

    protected override void Awake()
    {
        base.Awake();
        agentsPool = new Dictionary<int, Queue<GameObject>>();
        agents = new Dictionary<int, Object>();
        LoadAgents();

    }

    public GameObject GetMilitary(ListUpgrade type)
    {
        Queue<GameObject> pool;
        if(agentsPool.TryGetValue(type.GetHashCode(),out pool))
        {
            if (pool.Count > 0)
                return pool.Dequeue();
            else
                return Create(type);
        }
        else
        {
            agentsPool[type.GetHashCode()] = new Queue<GameObject>();
            return Create(type);
        }
    }

    public void ReturnSolider(ListUpgrade type,GameObject agent)
    {
        agent.SetActive(false);
        Queue<GameObject> pool;
        if(agentsPool.TryGetValue(type.GetHashCode(),out pool))
        {
            pool.Enqueue(agent);
        }
        else
        {
            agentsPool[type.GetHashCode()] = new Queue<GameObject>();
            agentsPool[type.GetHashCode()].Enqueue(agent);
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

    private GameObject Create(ListUpgrade type)
    {
        agents.TryGetValue(type.GetHashCode(), out Object res);
        if (res == null)
            return null;
        return Instantiate(res as GameObject);
    }
}
