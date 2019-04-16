using DataTable.Row;
using Generic.Observer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New Unit Table", menuName = "DataTable/JsonTable/Unit JSONTable", order = 4)]
    public sealed class JSONTable_Unit : JSONTable<UnitRow>, ISubject
    {
        private List<int> unitIds;


        private List<int> UnitIds
        {
            get { return unitIds ?? (unitIds = new List<int>()); }
        }

        private List<int> GenIDs()
        {
            UnitIds.Clear();
            for (int i = 0; i < Rows.Count; i++)
            {
                UnitIds.Add(Rows[i].ID);
            }
            return UnitIds;
        }

        public override UnitRow LoadRow(string json)
        {
            UnitRow r = ParseJson<UnitRow>(json);
            if (r != null)
            {
                int insertIndex = Rows.BinarySearch_L(0, Rows.Count, r);
                if (insertIndex >= 0) Rows.Insert(insertIndex, r);
                return r;
            }
            return null;
        }

        public UnitRow GetUnitById(int id)
        {
            List<int> ids = GenIDs();
            int index = ids.BinarySearch_L<int>(0, ids.Count, id);
            if (ids[index] == id)
                return Rows[index];
            return null;
        }

        public void UpdateTable(string json)
        {
            UnitRow r = LoadRow(json);
            if (r != null)
            {
                int updateIndex = Rows.BinarySearch_L(0, Count, r);
                if (Rows[updateIndex].ID == r.ID)
                    Rows[updateIndex] = r;

                for (int i = 0; i < Observers.Count; i++)
                {
                    if (((Observer_Unit)Observers[i]).UnitId == r.ID)
                    {
                        Notify(Observers[i]);
                        return;
                    }
                }
            }
        }

        private List<IObserver> observers;
        private List<IObserver> Observers
        {
            get
            {
                return observers ?? (observers = new List<IObserver>());
            }
        }

        public void Register(IObserver observer)
        {
            Observers.Add(observer);
        }

        public void Remove(IObserver observer)
        {
            int index = Observers.IndexOf(observer);
            if (index >= 0) Observers.RemoveAt(index);
        }

        public void Notify(IObserver observer)
        {
            Observer_Unit unitObserver = (Observer_Unit)observer;

            Observer_Unit.Package package = new Observer_Unit.Package()
            {
                Unit = GetUnitById(unitObserver.UnitId),
            };

            observer.SubjectUpdated(package);
        }

        public void NotifyAll()
        {
            for (int i = 0; i < Observers.Count; i++)
            {
                Notify(Observers[i]);
            }
        }
    }
}