using DataTable.Loader;
using UI;
using UnityEngine;

public class GameOnStarted : MonoBehaviour
{
    public SQLiteLocalLink SQLiteLocalLink;
    public ManualTableLoader TableLoader;
    public SIO_LoginListener LoginListener;
    public LoadingPanel LoadingPanel;
    public SceneLoader SceneLoader;

    private void Start()
    {
        LoadingPanel.Add(new Task_CheckDatabase(SQLiteLocalLink), "Checking data ...");
        LoadingPanel.Add(new Task_CheckVersion(TableLoader), "Checking version ...");
        LoadingPanel.Open();
    }

    public void LoginTask()
    {
        Task_SynchronousClient syncServerData = new Task_SynchronousClient(LoginListener);
        syncServerData.AddGameTask("R_BASE_INFO");
        syncServerData.AddGameTask("R_USER_INFO");
        syncServerData.AddGameTask("R_GET_POSITION");


        LoadingPanel.Add(syncServerData,"Synchronous ...");

        SceneLoader.SetLoadScene(1);
        LoadingPanel.Add(SceneLoader,"Entrancing game ...");

        LoadingPanel.Open();
    }
}
