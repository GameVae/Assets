using UI;
using UI.Widget;
using UnityEngine;
using UnityEngine.Events;
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
    private UnityAction doneAction;
    private SceneLoader sceneLoader;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance.gameObject);
    }

    private void Start()
    {
        sceneLoader = SceneLoader.Instance;
    }

    private void Update()
    {
        if (Panel.activeInHierarchy)
        {
            if (isLoadingScene)
            {
                Progress = sceneLoader.Progress;
                if (ProgressBar.Value == ProgressBar.MaxValue)
                {
                    sceneLoader.ActiveScene();
                    if (sceneLoader.IsActiveDone)
                    {
                        isLoadingScene = false;
                        ClosePanel();
                    }
                }
            }
            else
            {
                if(IsDone)
                {
                    doneAction?.Invoke();
                    doneAction = null;
                }
            }
            ProgressBar.Value = Mathf.MoveTowards(ProgressBar.Value, Progress, Time.deltaTime);
        }
    }

    public void LoadScene(int index)
    {
        if (!isLoadingScene)
        {
            isLoadingScene = true;
            Panel.SetActive(true);
            sceneLoader.LoadScene(index);
            ProgressBar.MaxValue = 1;
            ProgressBar.Value = 0;
        }
    }

    public void StartProgress(float max)
    {
        if (!isLoadingScene)
        {
            Panel.SetActive(true);
            isDone = false;
            ProgressBar.MaxValue = max;
            ProgressBar.Value = 0;
            Progress = max;
        }
    }

    public void Done(UnityAction doneAct = null)
    {       
        isDone = true;
        doneAction = doneAct ?? ClosePanel;
    }

    public void ClosePanel()
    {
        Panel.SetActive(false);
    }
}
