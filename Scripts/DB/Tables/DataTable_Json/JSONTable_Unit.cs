using UnityEngine;
using DataTable.Row;
using Generic.Observer;
using System.Collections.Generic;
using System.Linq;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New Unit Table", menuName = "DataTable/JsonTable/Unit JSONTable", order = 4)]
    public sealed class JSONTable_Unit : JSONTable<UnitRow>, ISubject, ISearchByObjectCompare<UnitRow>
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

        private UnitRow searchObject;
        private UnitRow SearchObject
        {
            get
            {
                return searchObject ?? (searchObject = new UnitRow());
            }
        }

        public UnitRow GetUnitById(int id)
        {
            List<int> ids = UnitIds;
            int index = ids.BinarySearch_R(id);
            if (ids[index] == id)
                return Rows[index];
            return null;
        }

        protected override void Add(UnitRow unit)
        {
            if (unit != null)
            {
                int unitID = unit.ID;
                int insertIndex = Rows.BinarySearch_R(GetSearchObject(unitID));

                if (insertIndex >= 0)
                {
                    Rows.Insert(insertIndex, unit);
                    UnitIds.Insert(insertIndex, unitID);
                }
            }
        }

        public override void UpdateTable(JSONObject json)
        {
            UnitRow updateData = Json.AJPHelper.ParseJson<UnitRow>(json.ToString());
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

        public UnitRow GetSearchObject(object obj)
        {
            SearchObject.ID = (int)obj;
            return SearchObject;
        }
    }
}