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

    }

    public void Done(string task)
    {
        if(progressSequence.ContainsKey(task))
        {
            UnityAction temp = progressSequence[task];
            progressSequence.Remove(task);
            temp?.Invoke();
        }
        Debug.Log("Complete: " + task);
    }

    public void AddTask(string tasks,UnityAction doneAct = null)
    {
        progressSequence[tasks] = doneAct;
    }
}
