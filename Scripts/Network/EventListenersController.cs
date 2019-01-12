using System.Collections.Generic;
using UnityEngine;

namespace Network.Data
{
    public sealed class EventListenersController : MonoBehaviour
    {
        public static EventListenersController Instance { get; private set; }

        public Sync.Sync SyncData { get { return Conn?.Sync; } }
        public Connection Conn;

        private Dictionary<string, System.Func<JSONObject>> emitter;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else Destroy(gameObject);

            emitter = new Dictionary<string, System.Func<JSONObject>>();
        }

        public void AddEmiter(string ev, System.Func<JSONObject> getData)
        {
            emitter[ev] = getData;
        }

        public void Emit(string ev)
        {
            try
            {
                Conn.Emit(ev, emitter[ev]?.Invoke());
            }
            catch (System.Exception ex)
            {
#if UNITY_EDITOR
                Debug.Log(ex.ToString());
#endif
            }
        }
    }
}