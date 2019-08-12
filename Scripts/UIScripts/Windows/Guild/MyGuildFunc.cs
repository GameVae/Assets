using System;
using Generic.Singleton;
using Network.Data;
using UI.Composites;
using UnityEngine;

public class MyGuildFunc : ToggleWindow
{
    [SerializeField] private GameObject guildInfoPanel;
    [SerializeField] private GameObject defaultPage;
    [SerializeField] private CreateGuildForm createForm;

    // create guild panel options
    [SerializeField] private WindowToggleGroup windowToggle;
    [SerializeField] private SelectableComp createOption;
    [SerializeField] private SelectableComp joinOption;

    private EventListenersController eventController;
    private EventListenersController EventController
    {
        get
        {
            return eventController ?? (eventController = Singleton.Instance<EventListenersController>());
        }
    }


    private PlayerInfo playerInfo;
    private PlayerInfo PlayerInfo
    {
        get
        {
            return playerInfo ?? (playerInfo = Singleton.Instance<PlayerInfo>());
        }
    }

    protected override void Start()
    {
        base.Start();

        createOption.OnClickEvents += OnCreateOption;
        joinOption.OnClickEvents += OnJoinOption;
        createForm.CancelBtn.OnClickEvents += CreateGuildCancel;
    }

    public override void Load(params object[] input)
    {
        // check user has joined guild yet
        // true => show guild info
        // false => show defaultPage
        string gName = PlayerInfo.Info.Guild_Name;
        if(!string.IsNullOrEmpty(gName))
        {            
            guildInfoPanel.gameObject.SetActive(true);
        }
        else
        {
            defaultPage.gameObject.SetActive(true);          
        }
    }

    public override void Close()
    {
        base.Close();
        createForm.Close();
        defaultPage.gameObject.SetActive(false);
        guildInfoPanel.gameObject.SetActive(false);
    }
    public override void Open()
    {
        base.Open();
        Load();
    }

    protected override void Init()
    {
        
    }

    private void OnCreateOption()
    {
        defaultPage.gameObject.SetActive(false);
        createForm.Open();
    }

    private void OnJoinOption()
    {
        windowToggle.Active(0);
    }

    private void CreateGuildCancel()
    {
        defaultPage.gameObject.SetActive(true);
        createForm.Close();
    }

    public void S_CREATE_GUILD(JSONObject data)
    {
        Debugger.Log(data);

        EventController.Emit("S_CREATE_GUILD",data);
        //guildInfoPanel.gameObject.SetActive(true);
    }
}
