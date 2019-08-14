using System;
using DataTable.Row;
using Generic.Pooling;
using Generic.Observer;
using UnityEngine.Events;
using System.Collections.Generic;

public class Observer_Unit : IObserver, IComparable, IPoolable
{
    public class Package
    {
        public UnitRow Unit;
    }

    private bool inited;
    private UnitRow unit;
    private Package package;
    private List<UnityAction<Package>> actions;

    public int UnitId
    {
        get { return unit.ID; }
    }
    public int ManagedId
    {
        get; private set;
    }
    public UnitRow Subject
    {
        get { return unit; }
    }
    public event UnityAction<Package> OnSubjectUpdated
    {
        add
        {
            if (actions == null) actions = new List<UnityAction<Package>>();
            actions.Add(value);
        }
        remove
        {
            int index = actions.IndexOf(value);
            if (index >= 0)
                actions.RemoveAt(index);
        }
    }
      
    public Observer_Unit() { }
    public Observer_Unit(UnitRow subject)
    {
        unit = subject;
        inited = true;
    }

    public void Dispose()
    {
        actions?.Clear();
        package = null;
        unit = null;
        inited = false;
    }
    public int CompareTo(object obj)
    {
        Observer_Unit other = obj as Observer_Unit;
        return UnitId.CompareTo(other.UnitId);
    }
    public void FirstSetup(int insId)
    {
        ManagedId = insId;
    }
    public void RefreshSubject(UnitRow subject)
    {
        if (!inited)
        {
            unit = subject;
            inited = true;
        }
    }
    public void SubjectUpdated(object dataPacked)
    {
        package = dataPacked as Package;
        //Debugger.Log(package.Unit);
        if (unit.CompareTo(package.Unit) == 0)
        {
            unit = package.Unit;
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i].Invoke(package);
            }
        }
    }
}
