using System.Collections.Generic;
using UnityEngine;

public abstract class Range : MonoBehaviour
{
    [SerializeField] private Transform owner;

    protected Transform Owner
    {
        get { return owner ?? (owner = gameObject.transform); }
        private set { owner = value; }
    }

    protected Queue<CellInfo> cellInfors;

    public abstract Queue<CellInfo> GetInfo();
}
