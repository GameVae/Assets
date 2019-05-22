using Generic.Singleton;
using SocketIO;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Network.Data
{
    public sealed class EventListenersController : MonoSingle<EventListenersController>
    {
        public Sync.Sync SyncData { get { return Conn?.Sync; } }

        private Connection conn;
        private Dictionary<string, System.Func<JSONObject>> emitter;

        private Connection Conn
        {
            get { return conn ?? (conn = Singleton.Instance<Connection>()); }
        }

        private Dictionary<string, Func<JSONObject>> Emitter
        {
            get
            {
                return emitter ?? (emitter = new Dictionary<string, Func<JSONObject>>());
            }
        }

        public void AddEmiter(string ev, System.Func<JSONObject> getData)
        {
            Emitter[ev] = getData;
            Debugger.Log("add emmiter " + ev);
        }

        public void Emit(string ev)
        {
            try
            {
                if (Emitter.ContainsKey(ev))
                    conn.Emit(ev, Emitter[ev]?.Invoke());
            }
            catch (System.Exception ex)
            {
#if UNITY_EDITOR
                Debug.Log(ex.ToString());
#endif
            }
        }

        public void Emit(string ev, JSONObject data)
        {
            Conn.Emit(ev, data);
        }

        public void On(string ev, Action<SocketIOEvent> callback)
        {
            Conn.On(ev, callback);
        }

        public void Off(string ev, Action<SocketIOEvent> callback)
        {
            Conn.Off(ev, callback);
        }
    }
}