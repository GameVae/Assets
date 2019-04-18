using DataTable.Row;
using Generic.Observer;
using System.Collections.Generic;
using UnityEngine.Events;

public class Observer_Unit : IObserver
{
    public class Package
    {
        public UnitRow Unit;
    }

    public int UnitId
    {
        get { return unit.ID; }
    }

    private UnitRow unit;
    private Package package;
    private List<UnityAction<Package>> actions;

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

    public Observer_Unit(UnitRow subject)
    {
        unit = subject;
    }

    public void SubjectUpdated(object dataPacked)
    {
        package = dataPacked as Package;
        Debugger.Log(package.Unit);
        if (unit.CompareTo(package.Unit) == 0)
        {
            unit = package.Unit;
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i].Invoke(package);
            }
        }
    }

    public void Dispose()
    {
        actions?.Clear();
        package = null;
        unit = null;
    }
}
