using DataTable.Loader;
using UI;
using UnityEngine;

public class GameOnStarted : MonoBehaviour
{
    private GameTaskCollection checkversionTask;
    private GameTaskCollection loginTask;

    public SQLiteLocalLink SQLiteLocalLink;
    public ManualTableLoader TableLoader;
    public SIO_LoginListener LoginListener;
    public LoadingPanel LoadingPanel;
    public SceneLoader SceneLoader;

    private void Start()
    {      
        checkversionTask = new GameObject().AddComponent<GameTaskCollection>();
        checkversionTask.AddGameTask(new SqliteFileValidator(SQLiteLocalLink));
        checkversionTask.AddGameTask(new VersionGame(TableLoader));
        
        StartCoroutine(checkversionTask.Action());
    }

    public void LoginTask()
    {
        AsyncGameTaskCollection syncServerData = new GameObject().AddComponent<AsyncGameTaskCollection>();
        syncServerData.AddGameTask(new SyncServerData(LoginListener, "R_BASE_INFO"));
        syncServerData.AddGameTask(new SyncServerData(LoginListener, "R_USER_INFO"));
        syncServerData.AddGameTask(new SyncServerData(LoginListener, "R_GET_POSITION"));

        loginTask = new GameObject().AddComponent<GameTaskCollection>();
        loginTask.AddGameTask(syncServerData);
        loginTask.AddGameTask(new LoadScene(1, SceneLoader));

        StartCoroutine(loginTask.Action());
    }
}
