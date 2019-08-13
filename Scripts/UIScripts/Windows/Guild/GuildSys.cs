using DataTable;
using DataTable.Row;
using Generic.Singleton;
using Network.Data;
using SocketIO;
using UnityEngine;

public class GuildSys : MonoBehaviour
{
    [SerializeField] private JSONTable_GuildInfo guildInfos;

    private EventListenersController eventController;

    public EventListenersController EventController
    {
        get
        {
            return eventController ?? (eventController = Singleton.Instance<EventListenersController>());
        }
    }

    public GuildMemberRow FindGuildByName(string gName)
    {
        return guildInfos.FindByName(gName);
    }

    private void Start()
    {
        EventController.On("R_CREATE_GUILD", R_CREATE_GUILD);
    }

    private void R_CREATE_GUILD(SocketIOEvent obj)
    {
        Debugger.Log(obj);
    }
}
