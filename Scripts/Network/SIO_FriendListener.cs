using DataTable.Row;
using System.Collections.Generic;

public class SIO_FriendListener : Listener
{
    private FriendRow focusInfo;

    public override void RegisterCallback()
    {
        AddEmiter("S_REJECT_FRIEND", () => GetFocusData("S_REJECT_FRIEND"));
        AddEmiter("S_ACCEPT_FRIEND", () => GetFocusData("S_ACCEPT_FRIEND"));
        AddEmiter("S_ADD_FRIEND", () => GetFocusData("S_ADD_FRIEND"));
        AddEmiter("S_UNFRIEND", () => GetFocusData("S_UNFRIEND"));
    }

    private JSONObject GetFocusData(string ev)
    {
        Dictionary<string, string> keyValues = new Dictionary<string, string>()
        {
            {"ID_User"  ,SyncData.User_ID.ToString()},
            {"ID_Player",focusInfo.ID_Player.ToString()}
        };

        string json = string.Format(
            "{{" + 
            "\"ID_User\":{0}," +
            "\"ID_Player\":{1}"+
            "}}",
            SyncData.User_ID,
            focusInfo.ID_Player
            );

        JSONObject data = new JSONObject(JSONObject.Type.BAKED);
        data.str = string.Format("{{\"{1}\":{0}}}", json, ev);
        Debugger.Log(data);
        return data;
    }

    public void SetFocusFriendInfo(FriendRow info)
    {
        focusInfo = info;
    }
}
