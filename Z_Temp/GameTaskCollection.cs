using UnityGameTask;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameTaskCollection :MonoBehaviour, IGameTaskCollection
{
    private bool isDone;
    private float progress;
    private Queue<IGameTask> tasks;

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
    public float CurrentTaskProgress
    {
        get;
        private set;
    }

    private Queue<IGameTask> Tasks
    {
        get
        {
            return tasks ?? (tasks = new Queue<IGameTask>());
        }
    }

    public IEnumerator Action()
    {
        IsDone = false;
        Progress = 0.0f;

        int taskCount = Tasks.Count;
        IGameTask currentTask = null;
        while(Tasks.Count > 0)
        {
            currentTask = Tasks.Dequeue();
            StartCoroutine(currentTask.Action());
            while(!currentTask.IsDone)
            {
                CurrentTaskProgress = currentTask.Progress;
                Debugger.Log("Sync Task: " + Progress + " - Current task " + CurrentTaskProgress);
                yield return null;
            }
            Progress = 1.0f - (Tasks.Count * 1.0f / taskCount);      
            yield return null;
        }

        IsDone = true;
        Progress = 1.0f;
        yield break;
    }
    public void AddGameTask(IGameTask task)
    {
        Tasks.Enqueue(task);
    }
}
