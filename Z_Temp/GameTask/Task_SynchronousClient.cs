using UnityGameTask;
using System.Collections;
using System.Collections.Generic;

public partial class Task_SynchronousClient : IGameTask
{
    public partial class EventIdentify { }

    private bool isDone;
    private float progress;
    private int completed;

    private Listener listener;
    private List<EventIdentify> tasks;

    public bool IsDone
    {
        get
        {
            return isDone;
        }
        private set { isDone = value; }
    }
    public float Progress
    {
        get
        {
            return progress;
        }
        private set
        {
            progress = value;
        }
    }
    private List<EventIdentify> Tasks
    {
        get
        {
            return tasks ?? (tasks = new List<EventIdentify>());
        }
    }
   
    public Task_SynchronousClient(Listener _listener)
    {
        listener = _listener;
    }
    
    public IEnumerator Action()
    {
        IsDone = false;
        Progress = 0.0f;

        int taskCount = Tasks.Count;
        completed = 0;

        yield return StartAllTask();

        while (completed < taskCount)
        {
            Progress = completed * 1.0f / taskCount;
            yield return null;
        }

        Progress = 1.0f;
        IsDone = true;
        Clear();
    }

    public void Complete(EventIdentify task)
    {
        if (Tasks.Contains(task))
        {
            //Tasks.Remove(task);
            completed++;
        }
    }

    public void AddGameTask(string evt, bool autoEmit = false)
    {
        Tasks.Add(new EventIdentify(this, listener, evt, autoEmit));
    }

    private IEnumerator StartAllTask()
    {
        for (int i = 0; i < Tasks.Count; i++)
        {
            yield return Tasks[i].Action();
        }
    }

    private void Clear()
    {
        for (int i = 0; i < Tasks.Count; i++)
        {
            Tasks[i].RemoveListener();
        }
        Tasks.Clear();
    }
}
