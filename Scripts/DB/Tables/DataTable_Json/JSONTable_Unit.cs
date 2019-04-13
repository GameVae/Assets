using DataTable.Row;
using Generic.Observer;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New Unit Table", menuName = "DataTable/JsonTable/Unit JSONTable", order = 4)]
    public sealed class JSONTable_Unit : JSONTable<UnitRow>, ISubject
    {
        private void Sort()
        {
            Rows?.BinarySort_L();
        }

        public UnitRow GetUnit(int id)
        {
            int index = Rows.BinarySearch_L(0, Rows.Count - 1, id);
            return index >= 0 ? Rows[index] : null;
        }

        public override void LoadTable(JSONObject data, bool clearPre = true)
        {
            base.LoadTable(data, clearPre);
            Sort();
        }

        private List<Observer_Unit> observers;
        private List<Observer_Unit> Observers
        {
            get
            {
                return observers ?? (observers = new List<Observer_Unit>());
            }
        }

        public void Register(IObserver observer)
        {
            Observers.Add((Observer_Unit)observer);
        }

        public void Remove(IObserver observer)
        {
            int index = Observers.IndexOf((Observer_Unit)observer);
            if (index >= 0) Observers.RemoveAt(index);
        }

        public void Notify(IObserver observer)
        {
            Observer_Unit unitObserver = (Observer_Unit)observer;

            Observer_Unit.Package package = new Observer_Unit.Package()
            {
                Unit = Rows[unitObserver.UnitId]
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

        public void UpdateRow(string json)
        {
            UnitRow r = ParseJson<UnitRow>(json);
            if (GetUnit(r.ID) == null)
            {
                LoadRow(json);
            }
            else
            {
                int index = Rows.BinarySearch_L(0, Rows.Count - 1, r.ID);
                Rows[index] = r;
                Observer_Unit obs = Observers.FirstOrDefault(x => x.UnitId == r.ID);
                if (obs != default(Observer_Unit))
                    obs.SubjectUpdated(r);
            }
        }
    }
}