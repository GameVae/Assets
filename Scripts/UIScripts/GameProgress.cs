using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance { get; private set; }
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
    private Dictionary<string, UnityAction> progressSequence;
    public bool IsEmpty { get { return progressSequence == null || progressSequence.Count == 0; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
        progressSequence = new Dictionary<string, UnityAction>();
        //progressSequence.Add("get user info");
        //progressSequence.Add("get base info");
        //progressSequence.Add("get position");

    }

    public void Done(string task)
    {
        if(progressSequence.ContainsKey(task))
        {
            progressSequence[task]?.Invoke();
            progressSequence.Remove(task);
        }
        Debug.Log("Complete: " + task);
        if (progressSequence.Count == 0)
        {
            LoadingUICtrl.Instance.Done();
        }
        else
        {
            StartTask();
        }
    }

    public void AddTask(string tasks,UnityAction act = null)
    {
        progressSequence[tasks] = act;
    }

    public void StartTask()
    {
        LoadingUICtrl.Instance.StartProgress(1);
        LoadingUICtrl.Instance.Progress = 1;
    }
}
