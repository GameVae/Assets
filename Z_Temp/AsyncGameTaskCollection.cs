using UnityGameTask;
using Generic.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generic.Singleton;

public class AsyncGameTaskCollection :MonoBehaviour,
    IPoolable, IGameTaskCollection
{
    private bool isDone;
    private float progress;
    private List<IGameTask> tasks;

    public int ManagedId
    {
        get; private set;
    }
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
    private List<IGameTask> Tasks
    {
        get
        {
            return tasks ?? (tasks = new List<IGameTask>());
        }
    }
    
    public IEnumerator Action()
    {
        IsDone = false;
        Progress = 0.0f;

        int taskCount = Tasks.Count;
        StartAllTask();

        while (Tasks.Count > 0)
        {
            ClearCompleteTask();
            Progress = 1.0f - (Tasks.Count * 1.0f / taskCount);
            Debugger.Log("Async task: " + Progress);

            yield return null;
        }

        IsDone = true;
        Progress = 1.0f;
        yield break;
    }
    public void AddGameTask(IGameTask task)
    {
        Tasks.Add(task);
    }
    public void FirstSetup(int insId)
    {
        ManagedId = insId;
        IsDone = false;
        Progress = 0.0f;
    }
    public void Dispose()
    {
        IsDone = false;
        Progress = 0.0f;
        Tasks.Clear();
    }

    private void StartAllTask()
    {
        for (int i = 0; i < Tasks.Count; i++)
        {
            StartCoroutine(Tasks[i].Action());
        }
    }
    private void ClearCompleteTask()
    {
        for (int i = Tasks.Count - 1; i >= 0; i--)
        {
            if(Tasks[i].IsDone)
            {
                Tasks.RemoveAt(i);
            }
        }
    }
}
