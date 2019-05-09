using Generic.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;
using UnityGameTask;

public sealed class LoadingPanel : MonoSingle<LoadingPanel>
{
    public Image Background;
    public GameObject Panel;
    public GUIProgressSlider ProgressBar;
    public TextMeshProUGUI LoadingInfo;

    private Queue<IGameTask> gameTasks;
    private Queue<IGameTask> GameTasks
    {
        get
        {
            return gameTasks ?? (gameTasks = new Queue<IGameTask>());
        }
    }

    private Queue<string> descriptions;
    private Queue<string> Descriptions
    {
        get
        {
            return descriptions ?? (descriptions = new Queue<string>());
        }
    }

    public void Open()
    {
        Panel.SetActive(true);
        StartCoroutine(StartUIHandle());
    }

    public void Add(IGameTask task, string description = "")
    {
        GameTasks.Enqueue(task);
        Descriptions.Enqueue(description);
    }

    private IEnumerator StartUIHandle()
    {
        while (GameTasks.Count > 0)
        {
            IGameTask task = GameTasks.Dequeue();

            StartCoroutine(task.Action());
            LoadingInfo.text = Descriptions.Dequeue();

            while (!task.IsDone || task.Progress != ProgressBar.Value)
            {
                ProgressBar.Value
                    = Mathf.MoveTowards(ProgressBar.Value, task.Progress, Time.deltaTime);
                yield return null;
            }
            ProgressBar.Value = 0;
        }
        Panel.SetActive(false);
        yield break;
    }
}
