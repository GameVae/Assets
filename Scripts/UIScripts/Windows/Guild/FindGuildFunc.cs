using SocketIO;
using System.Collections.Generic;
using UI.Composites;
using UnityEngine;

public class FindGuildFunc : ToggleWindow
{
    public GuildSys GuildSys;
    public SelectableComp FindBtn;

    public CustomInputField GuildTagInput;
    public CustomInputField GuildNameInput;

    public GuildTag GuildTag;
    public GameObject NotFoundPanel;

    private void Awake()
    {
        FindBtn.OnClickEvents += OnFindGuidlButton;

        GuildTag.AcceptBtn.OnClickEvents += S_APPLY_GUILD;

        GuildSys.EventController.On("R_SEARCH_GUILD", R_SEARCH_GUILD);
    }

    public override void Load(params object[] input)
    {
        
    }

    protected override void Init()
    {
        GuildTag.gameObject.SetActive(false);
        NotFoundPanel.gameObject.SetActive(false);
    }

    protected void OnFindGuidlButton()
    {
        S_SEARCH_GUILD();
    }

    private void S_APPLY_GUILD()
    {
        Dictionary<string, string> applyInfo = new Dictionary<string, string>()
        {

        };
        JSONObject data = new JSONObject(applyInfo);

        GuildSys.EventController.Emit("S_APPLY_GUILD", data);
    }

    private void S_SEARCH_GUILD()
    {
        string gName = string.IsNullOrEmpty(GuildNameInput.Text) ? null : GuildNameInput.Text;
        string gTag = string.IsNullOrEmpty(GuildTagInput.Text) ? null : GuildTagInput.Text;

        if(gName != null || gTag != null)
        {
            gName = gName ?? "null";
            gTag = gTag ?? "null";

            Dictionary<string, string> searchInfo = new Dictionary<string, string>()
            {
                {"GuildTag" , gTag },
                {"GuildName", gName}
            };

            JSONObject data = new JSONObject(searchInfo);
            GuildSys.EventController.Emit("S_SEARCH_GUILD", data);
        }
    }

    private void S_GET_GUILD_INFO()
    {
        Dictionary<string, string> searchInfo = new Dictionary<string, string>()
            {
                {"Guild_ID" , "" },                
            };

        JSONObject data = new JSONObject(searchInfo);
        GuildSys.EventController.Emit("S_GET_GUILD_INFO", data);
    }

    private void R_GET_GUILD_INFO(SocketIOEvent obj)
    {
        Debugger.Log(obj);
    }

    private void R_SEARCH_GUILD(SocketIOEvent obj)
    {
        Debugger.Log(obj);
    }
}
