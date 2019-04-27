using UnityEngine;
using DataTable.Row;
using Generic.Pooling;
using Generic.Observer;
using Extensions.BinarySearch;
using System.Collections.Generic;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New Unit Table", menuName = "DataTable/JsonTable/Unit JSONTable", order = 4)]
    public sealed class JSONTable_Unit :
        JSONTable<UnitRow>,
        ISubject<Observer_Unit>
    {
        private List<Observer_Unit> observers;
        private Pooling<Observer_Unit> observerPooling;

        private List<Observer_Unit> Observers
        {
            get
            {
                return observers ?? (observers = new List<Observer_Unit>());
            }
        }
        private Pooling<Observer_Unit> ObserverPooling
        {
            get
            {
                return observerPooling ??
                    (observerPooling = new Pooling<Observer_Unit>(CreateObserver));
            }
        }
        private Observer_Unit CreateObserver(int id)
        {
            Observer_Unit observer = new Observer_Unit();
            observer.FirstSetup(id);
            return observer;
        }

        protected override bool UpdateOrAdd(UnitRow updateData)
        {
            bool isUpdate = base.UpdateOrAdd(updateData);

            if (isUpdate)
            {
                Observer_Unit tempObs = ObserverPooling.GetItem();
                tempObs.RefreshSubject(updateData);

                Observer_Unit notifyObserver = Observers.FirstOrDefault_R(tempObs);
                ObserverPooling.Release(tempObs);

                if (notifyObserver != null)
                    Notify(notifyObserver);
            }

            return isUpdate;
        }

        public override void UpdateTable(JSONObject jsonObj)
        {
            // base.UpdateTable(jsonObj);
            if (jsonObj != null)
            {
                UnitRow updateData = JsonUtility.FromJson<UnitRow>(jsonObj.ToString());
                UpdateOrAdd(updateData);
            }
        }

        public void Register(Observer_Unit observer)
        {
            Observers.UpdateOrInsert_R(observer);
        }

        public void Remove(Observer_Unit observer)
        {
            Observers.Remove_R(observer);
        }

        public void Notify(Observer_Unit observer)
        {
            Observer_Unit.Package package = new Observer_Unit.Package()
            {
                Unit = Rows.FirstOrDefault_R(observer.Subject),
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