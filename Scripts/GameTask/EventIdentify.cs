using SocketIO;
using System.Collections;
using UnityGameTask;

public partial class Task_SynchronousClient
{
    public partial class EventIdentify : IGameTask
    {
        private bool autoEmit;
        private string identify;

        private Listener listener;
        private Task_SynchronousClient manager;

        public bool IsDone
        {
            get;
            private set;
        }
        public float Progress
        {
            get;
            private set;
        }

        public EventIdentify(Task_SynchronousClient _manager, Listener _listener,
             string _identify, bool _autoEmit = false)
        {
            listener = _listener;
            manager = _manager;

            identify = _identify;
            autoEmit = _autoEmit;

            listener.On(_identify, Received);
        }

        ~EventIdentify()
        {
            listener?.Off(identify, Received);
        }

        public IEnumerator Action()
        {
            Progress = 0.0f;
            IsDone = false;

            if (autoEmit)
                Emit();
            yield break;
        }

        public void RemoveListener()
        {
            listener.Off(identify, Received);
        }

        private void Emit()
        {
            listener.Emit(identify);
        }

        private void Received(SocketIOEvent obj)
        {
            Progress = 1.0f;
            IsDone = true;

            manager.Complete(this);
            Debugger.Log(identify + " received");
            //listener.Off(identify, Received);
        }
    }
}