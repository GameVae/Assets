using DataTable;
using DataTable.Row;
using Generic.Singleton;
using Network.Data;
using SocketIO;
using UnityEngine;

public class GuildSys : MonoBehaviour
{
    public JSONTable_GuildInfo GuildTable;

    private EventListenersController eventController;
    public EventListenersController EventController
    {
        get
        {
            return eventController ?? (eventController = Singleton.Instance<EventListenersController>());
        }
    }

    private PlayerInfo playerInfo;
    public PlayerInfo PlayerInfo
    {
        get
        {
            return playerInfo ?? (playerInfo = Singleton.Instance<PlayerInfo>());
        }
    }

    public GuildMemberRow Master
    {
        get
        {
            return GuildTable.Master;
        }
    }

    private void Start()
    {
        EventController.On("R_CREATE_GUILD", R_CREATE_GUILD);
    }

    private void R_CREATE_GUILD(SocketIOEvent obj)
    {
        Debugger.Log(obj);
        int success = -1;
        obj.data["R_CREATE_GUILD"].GetField(ref success, "Enum");
        if (success == 1)
        {
            int cost = 500;
            PlayerInfo.Info.Diamond -= cost;
        }
        else
        {
            string msg = "";
            obj.data["R_CREATE_GUILD"].GetField(ref msg, "Message");
            MessagePopup.Open(msg);
        }
    }    
}
