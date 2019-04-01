using Generic.Singleton;
using System;
using System.Collections.Generic;
using TMPro;
using UI;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;
using static GameProgress;

public sealed class LoadingPanel : MonoSingle<LoadingPanel>
{
    public Image Background;
    public GameObject Panel;
    public GUIProgressSlider ProgressBar;
    public TextMeshProUGUI LoadingInfo;

    private bool isDone;
    private GameProgress gameTask;
    private Queue<GameProgress> progSequence;
    private Task curTask;

    private SceneLoader sceneLoader;

    protected override void Awake()
    {
        base.Awake();
        progSequence = new Queue<GameProgress>();
        isDone = true;
    }

    private void Start()
    {
        sceneLoader = Singleton.Instance<SceneLoader>();
    }

    private void Update()
    {
        if (!isDone)
        {
            if (curTask != null && (!curTask.IsDone() || ProgressBar.Value != curTask.GetProgress()))
            {
                ProgressBar.Value = Mathf.MoveTowards(ProgressBar.Value, curTask.GetProgress(), Time.deltaTime);
            }
            else
            {
                NextTask();
                if (curTask == null)
                {
                    NextProg();
                }
            }
        }
    }

    public void LoadScene(int index)
    {
        GameProgress loadProg = new GameProgress
            (
            doneAct: null,
            t: new Task()
            {
                IsDone = delegate { return sceneLoader.IsActiveDone; },
                GetProgress = delegate { return sceneLoader.Progress; },
                Name = "LoadScene",
                Title = "Entrancing game ...",
                Start = delegate
                {
                    sceneLoader.LoadScene(index);
                    sceneLoader.ActiveScene();
                }
            }
            );
        AddTask(loadProg);
    }

    public void AddTask(GameProgress progs)
    {
        progSequence.Enqueue(progs);
        if (isDone)
        {
            Panel.SetActive(true);
            isDone = false;
            NextProg();
        }
    }

    private void NextProg()
    {
        if (gameTask == null)
        {
            if (progSequence.Count > 0)
            {
                gameTask = progSequence.Dequeue();
                NextTask();
            }
        }
        else
        { 
            gameTask?.Done();
            if (progSequence.Count > 0)
            {
                gameTask = progSequence.Dequeue();
            }
            else
            {
                gameTask = null;
                isDone = true;
                Panel.SetActive(false);
                return;
            }
        }
       
    }

    private void NextTask()
    {
        ProgressBar.Value = 0;
        curTask = gameTask.GetTask();
        curTask?.Start?.Invoke();
        LoadingInfo.text = curTask?.Title;
    }
}
