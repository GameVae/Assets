using DataTable;
using DataTable.Row;
using Generic.Singleton;
using Network.Data;
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

    public GuildRow FindGuildByName(string gName)
    {
        return guildInfos.FindByName(gName);
    }
    
}
