using EnumCollect;
using Generic.Singleton;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public sealed class TowerSpawnManager : MonoSingle<TowerSpawnManager>
{
    private Dictionary<int, Queue<GameObject>> towerPool;

    private Dictionary<int, Object> towers;

    private AssetUtils AssetUtil;

    public Transform Container;

    protected override void Awake()
    {
        base.Awake();
        AssetUtil = Singleton.Instance<AssetUtils>();

        towerPool = new Dictionary<int, Queue<GameObject>>();
        towers = new Dictionary<int, Object>();
        LoadTowers();

    }

    private void LoadTowers()
    {
        Object[] objs = AssetUtil.Load(@"Prefabs\Towers");
        for (int i = 0; i < objs.Length; i++)
        {

            GameObject go = objs[i] as GameObject;
            int hashCode = go.GetComponent<BaseTower>().Type.GetHashCode();
            towers[hashCode] = objs[i];
        }
    }

    public GameObject GetTower(TowerType type)
    {
        if (towerPool.TryGetValue(type.GetHashCode(), out Queue<GameObject> pool))
        {
            if (pool.Count > 0)
                return pool.Dequeue();
            else
                return Create(type);
        }
        else
        {
            towerPool[type.GetHashCode()] = new Queue<GameObject>();
            return Create(type);
        }
    }

    public void ReturnTower(TowerType type, GameObject tower)
    {
        tower.SetActive(false);
        if (towerPool.TryGetValue(type.GetHashCode(), out Queue<GameObject> pool))
        {
            pool.Enqueue(tower);
        }
        else
        {
            towerPool[type.GetHashCode()] = new Queue<GameObject>();
            towerPool[type.GetHashCode()].Enqueue(tower);
        }
    }

    private GameObject Create(TowerType type)
    {
        towers.TryGetValue(type.GetHashCode(), out Object res);
        if (res == null)
            return null;
        return Instantiate(res as GameObject, Container);
    }

}
