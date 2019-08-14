using Generic.Singleton;
using Network.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Composites;
using UnityEngine;

public class CreateGuildForm : BaseWindow
{
    [Serializable]
    public sealed class InputTag
    {
        public int CharacterCount;
        public CustomInputField Input;
        public GameObject WarningNotation;

        public void WarningNotationActive(bool v)
        {
            WarningNotation.gameObject.SetActive(v);
        }

        public void SetupEvent()
        {
            Input.OnValueChanged += InputChange;
        }

        public bool CheckInputText()
        {
            if (Input.Text.Length == CharacterCount)
            {
                WarningNotationActive(false);
                return true;
            }
            WarningNotationActive(true);
            return false;
        }

        private void InputChange(string value)
        {
            if (value.Length > CharacterCount)
            {
                string str = value.Substring(0, CharacterCount);
                Input.SetContent(str);
            }
        }
    }

    public InputTag GuildTag;
    public InputTag GuildName;
    public PlaceholderComp DiamonInfo;
    public SelectableComp CreateBtn;
    public SelectableComp CancelBtn;
    public MyGuildFunc MyGuildWindow;

    private PlayerInfo playerInfo;
    private PlayerInfo PlayerInfo
    {
        get
        {
            return playerInfo ?? (playerInfo = Singleton.Instance<PlayerInfo>());
        }
    }

    protected override void Init()
    {
        GuildTag.SetupEvent();
        GuildName.SetupEvent();
        CreateBtn.OnClickEvents += S_CREATE_GUILD;
    }

    public override void Load(params object[] input)
    {
        GuildTag.WarningNotationActive(false);
        GuildName.WarningNotationActive(false);

        int diamon = PlayerInfo.Info.Diamond;

        string color = diamon >= 500 ? "black" : "red";
        string costFormat = "<color={2}>{0}/{1}</color>";

        DiamonInfo.Text = string.Format(costFormat, diamon, 500, color);
    }

    public override void Open()
    {
        base.Open();
        Load();
    }

    private void S_CREATE_GUILD()
    {
        bool canCreate = GuildTag.CheckInputText() && GuildName.CheckInputText();
        if (canCreate)
        {
            Dictionary<string, string> createGuildInfo = new Dictionary<string, string>()
            {
                {"GuildTag"     ,GuildTag.Input.Text},
                {"GuildName"    ,GuildName.Input.Text},
                {"ID_User"      ,PlayerInfo.Info.ID_User.ToString()},
                {"Server_ID"    ,"1"}
            };

            JSONObject data = new JSONObject(createGuildInfo);

            MyGuildWindow.S_CREATE_GUILD(data);

            Close();
        }
    }

}
