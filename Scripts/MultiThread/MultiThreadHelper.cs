using Generic.Singleton;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThread
{
    public class MultiThreadHelper : MonoSingle<MultiThreadHelper>
    {
        public int MainTheadID { get; private set; }
        public Thread MainThread { get; private set; }

        private static object locker;
        private static Queue<Action> actions;

        public bool IsMainThreadRunning
        {
            get { return Thread.CurrentThread.ManagedThreadId == MainTheadID; }
        }

        protected override void Awake()
        {
            base.Awake();

            locker = new object();
            actions = new Queue<Action>();

            MainThread = Thread.CurrentThread;
            MainTheadID = Thread.CurrentThread.ManagedThreadId;

        }

        private void Update()
        {
            lock (locker)
            {
                while (actions.Count > 0)
                    actions.Dequeue()?.Invoke();
            }
        }

        public void Invoke(Action action)
        {
            if (IsMainThreadRunning)
                action?.Invoke();
            else
            {
                lock(locker)
                {
                    actions.Enqueue(action);
                }
            }
        }
    }
}