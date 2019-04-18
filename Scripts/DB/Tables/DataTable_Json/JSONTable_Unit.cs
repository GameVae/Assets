using UnityEngine;
using DataTable.Row;
using Generic.Observer;
using System.Collections.Generic;
using System.Linq;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New Unit Table", menuName = "DataTable/JsonTable/Unit JSONTable", order = 4)]
    public sealed class JSONTable_Unit : JSONTable<UnitRow>, ISubject
    {
        private List<int> unitIds;
        private List<IObserver> observers;

        private List<int> UnitIds
        {
            get { return unitIds ?? (unitIds = new List<int>()); }
        }
        private List<IObserver> Observers
        {
            get
            {
                return observers ?? (observers = new List<IObserver>());
            }
        }

        public UnitRow GetUnitById(int id)
        {
            Rows.RemoveNull();
            //List<int> ids = UnitIds;
            //int index = ids.BinarySearch_L<int>(0, ids.Count, id);
            //if (index < Count && ids[index] == id)
            //    return Rows[index];
            //return null;
            return Rows.FirstOrDefault(unit => unit.ID == id);
        }

        public override void LoadRow(string json)
        {
            UnitRow unit = ParseJson<UnitRow>(json);
            if (unit != null)
            {
                int insertIndex = Rows.BinarySearch_L(0, Rows.Count, unit);

                if (insertIndex >= 0)
                {
                    Rows.Insert(insertIndex, unit);
                    UnitIds.Insert(insertIndex, unit.ID);
                }
            }
        }

        public void UpdateTable(string json)
        {
            UnitRow updateData = ParseJson<UnitRow>(json);
            if (updateData != null)
            {
                //int updateIndex = Rows.BinarySearch_L(0, Count, updateData);
                //if (Rows[updateIndex].ID == updateData.ID)
                //    Rows[updateIndex] = updateData;

                int updateIndex = -1;
                for (int i = 0; i < Count; i++)
                {
                    if (Rows[i].ID == updateData.ID)
                    {
                        updateIndex = i;
                        Rows[i] = updateData;
                        break;
                    }
                }

                if (updateIndex >= 0)
                    for (int i = 0; i < Observers.Count; i++)
                    {
                        if (((Observer_Unit)Observers[i]).UnitId == updateData.ID)
                        {
                            Notify(Observers[i]);
                            return;
                        }
                    }
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