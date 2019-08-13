using DataTable.Row;
using System.Collections;
using System.Collections.Generic;
using UI.Composites;
using UnityEngine;

public class FindGuildFunc : ToggleWindow
{
    public GuildSys GuildSys;
    public SelectableComp FindBtn;
    public CustomInputField InputField;

    public GuildTag GuildTag;
    public GameObject NotFoundPanel;

    private void Awake()
    {
        FindBtn.OnClickEvents += OnFindGuidlButton;
        GuildTag.AcceptBtn.OnClickEvents += S_APPLY_GUILD;
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
        string gName = InputField.Text;
        if(!string.IsNullOrEmpty(gName))
        {
            GuildMemberRow g = GuildSys.FindGuildByName(gName);
            if (g != null)
            {
                GuildTag.GuildName.text = gName;

                GuildTag.gameObject.SetActive(true);
            }
            else
            {
                GuildTag.gameObject.SetActive(false);
                NotFoundPanel.gameObject.SetActive(true);
            }
        }
    }

    protected void S_APPLY_GUILD()
    {
        Dictionary<string, string> applyInfo = new Dictionary<string, string>()
        {

        };
        JSONObject data = new JSONObject(applyInfo);

        GuildSys.EventController.Emit("S_APPLY_GUILD", data);
    }
}
