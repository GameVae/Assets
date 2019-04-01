using System.Collections.Generic;
using UnityEngine.Events;

public class GameProgress
{
    /*
     * - Startup
	    - Check game version
	       true : ignore
	       false: - Download Data
    - Login	
	    - Get User Info
	    - Get RSS
	    - Get Base Info
	    - Get Position
     */
    public class Task
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public System.Func<bool> IsDone { get; set; }
        public System.Func<float> GetProgress { get; set; }
        public UnityAction Start { get; set; }
    }

    public bool IsDone
    {
        get { return tasks.Count == 0; }
    }
    private Queue<Task> tasks;
    private Task currentTask;
    private UnityAction doneAction;

    public GameProgress(UnityAction doneAct, params Task[] t)
    {
        tasks = new Queue<Task>();
        doneAction = doneAct;
        for (int i = 0; i < t.Length; i++)
        {
            tasks.Enqueue(t[i]);
        }
    }

    public Task GetTask()
    {
        if (currentTask == null)
        {
            if (!IsDone)
            {
                currentTask = tasks.Dequeue();
            }
        }
        else
        {
            if (currentTask.IsDone())
            {
                // Debugger.Log(currentTask.Name + " done");
                if (!IsDone)
                {
                    currentTask = tasks.Dequeue();
                }
                else currentTask = null;
            }
        }
        return currentTask;
    }

    public void Done()
    {
        if (IsDone) doneAction?.Invoke();
    }
}
