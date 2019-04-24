using UnityEngine;
using DataTable.Row;
using Entities.Navigation;
using System.Collections.Generic;

public class AttackInfoGenerator
{
    private Dictionary<string, string> infoDict;
    private Dictionary<string, string> InfoDict
    {
        get
        {
            return infoDict ?? (infoDict = new Dictionary<string, string>());
        }
    }

    public JSONObject CreateAttackInfo(AgentRemote ownerRemote, AgentRemote otherRemote)
    {
        InfoDict.Clear();
        UnitRow ownerUnit = ownerRemote.UnitInfo;
        UnitRow otherUnit = otherRemote.UnitInfo;

        InfoDict["Server_ID"] = ownerRemote.UserInfo.Server_ID;
        
        InfoDict["ID_Attack"] = ownerUnit.ID.ToString();
        InfoDict["ID_Unit_Attack"] = ((int)ownerUnit.ID_Unit).ToString();
        InfoDict["ID_User_Attack"] = ownerUnit.ID_User.ToString();
        
        InfoDict["ID_Defend"] = otherUnit.ID.ToString();
        InfoDict["ID_Unit_Defend"] = ((int)otherUnit.ID_Unit).ToString();
        InfoDict["ID_User_Defend"] = otherUnit.ID_User.ToString();
        
        InfoDict["Position_Cell_Attacker"] = ownerRemote.CurrentPosition.ToSerPosition().ToPositionString();

        JSONObject data = new JSONObject(infoDict);
        Debugger.Log(data);
        return data;
    }
}
