using Generic.Singleton;
using SocketIO;
using System.Collections.Generic;
using UnityEngine;

namespace Network.Data
{
    public sealed class EventListenersController : MonoSingle<EventListenersController>
    {
        public Sync.Sync SyncData { get { return Conn?.Sync; } }

        private Connection Conn;

        private Dictionary<string, System.Func<JSONObject>> emitter;

        protected override void Awake()
        {
            base.Awake();
            Conn = Singleton.Instance<Connection>();
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
                if (emitter.ContainsKey(ev))
                    Conn.Emit(ev, emitter[ev]?.Invoke());
            }
            catch (System.Exception ex)
            {
#if UNITY_EDITOR
                Debug.Log(ex.ToString());
#endif
            }
        }

        public void Emit(string ev,JSONObject data)
        {
            Conn.Emit(ev, data);
        }

        public void On(string ev, System.Action<SocketIOEvent> callback)
        {
            Conn.On(ev, callback);
        }
    }
}