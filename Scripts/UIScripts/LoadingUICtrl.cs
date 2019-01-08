using UI;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUICtrl : MonoBehaviour
{
    public static LoadingUICtrl Instance { get; private set; }
    public Image Background;
    public GameObject Panel;
    public GUIProgressSlider ProgressBar;

    public float Progress { get; set; }
    public bool IsDone
    {
        get
        { return isDone && ProgressBar.Value == ProgressBar.MaxValue; }
    }

    private bool isLoadingScene;
    private bool isDone;

    private SceneLoader sceneLoader;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance.gameObject);

        sceneLoader = SceneLoader.Instance;
        Done();

    }

    private void Update()
    {
        if (Panel.activeInHierarchy)
        {
            if (isLoadingScene)
            {
                Progress = sceneLoader.Progress;
                ProgressBar.Value = Mathf.MoveTowards(ProgressBar.Value, Progress, Time.deltaTime);
                if (ProgressBar.Value == ProgressBar.MaxValue)
                {
                    sceneLoader.ActiveScene();
                    if (sceneLoader.IsActiveDone)
                    {
                        isLoadingScene = false;
                        Done();
                    }
                }
            }
            else
            {
                if (IsDone)
                {
                    Panel.SetActive(false);
                }
                ProgressBar.Value = Mathf.MoveTowards(ProgressBar.Value, Progress, Time.deltaTime);
            }
        }
    }

    public void LoadScene(int index)
    {
        isLoadingScene = true;

        Panel.SetActive(true);
        sceneLoader.LoadScene(index);
        StartProgress(1.0f);
    }

    public void StartProgress(float max)
    {
        ProgressBar.MaxValue = max;
        ProgressBar.Value = 0;
        Panel.SetActive(true);
    }

    public void Done()
    {
        isDone = true;
    }
}
