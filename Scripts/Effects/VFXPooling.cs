using EnumCollect;
using Generic.Pooling;
using System;
using System.Collections.Generic;
using UnityEngine;

public class VFXPooling : MonoBehaviour
{
    [Serializable]
    class VFXPrefab
    {
        public ListUpgrade Type;
        public VFXArcher Prefab;
    }

    [SerializeField] private VFXPrefab[] vfx;
    private Dictionary<ListUpgrade, Pooling<VFXArcher>> pooling;

    private void Awake()
    {
        pooling = new Dictionary<ListUpgrade, Pooling<VFXArcher>>();
        if (vfx != null)
        {
            for (int i = 0; i < vfx.Length; i++)
            {
                int capture = i;
                if(!pooling.ContainsKey(vfx[capture].Type))
                {
                    pooling.Add(vfx[capture].Type, 
                        new Pooling<VFXArcher>((int id) => Create(id, vfx[capture].Prefab)));
                }
            }
        }
    }

    private VFXArcher Create(int insId,VFXArcher prefab)
    {
        VFXArcher obj = Instantiate(prefab);
        obj.FirstSetup(insId);

        return obj;
    }

    public VFXArcher GetItem(ListUpgrade type)
    {
        if(pooling.ContainsKey(type))
        {
            Debugger.Log("get vfx " + type);
            return pooling[type].GetItem();
        }
        Debugger.Log("type not found");
        return null;
    }

    public void Release(ListUpgrade type,VFXArcher obj)
    {
        if (pooling.ContainsKey(type))
        {
            pooling[type].Release(obj);
        }
    }
}
