using Entities.Navigation;
using Generic.Singleton;
using DataTable.Row;
using Network.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAttack : MonoBehaviour
{
    private NavRemote agentRemote;
   
    public NavRemote AgentRemote
    {
        get { return agentRemote ?? (agentRemote = GetComponent<NavRemote>()); }
    }

    private Dictionary<string, string> attackData;

    private void Start()
    {
        attackData = new Dictionary<string, string>();
    }

    public JSONObject S_ATTACK(NavRemote otherRemote)
    {

        attackData.Clear();
        UnitRow ownerUnit = AgentRemote.UnitInfo;
        UnitRow otherUnit = otherRemote.UnitInfo;

        attackData["Server_ID"] = "1";

        attackData["ID_Attack"] = ownerUnit.ID.ToString();
        attackData["ID_Unit_Attack"] = ((int)ownerUnit.ID_Unit).ToString();
        attackData["ID_User_Attack"] = ownerUnit.ID_User.ToString();

        attackData["ID_Defend"] = otherUnit.ID.ToString();
        attackData["ID_Unit_Defend"] = ((int)otherUnit.ID_Unit).ToString();
        attackData["ID_User_Defend"] = otherUnit.ID_User.ToString();

        attackData["Position_Cell_Attacker"] = AgentRemote.FixedMove.CurrentPosition.ToSerPosition().ToPositionString();

        JSONObject data = new JSONObject(attackData);
        Debugger.Log(data);
        return data;
    }

    public void Attack(NavRemote otherRemote)
    {
        attackData.Clear();
        UnitRow ownerUnit = AgentRemote.UnitInfo;
        UnitRow otherUnit = otherRemote.UnitInfo;

        attackData["Server_ID"] = "1";

        attackData["ID_Attack"] = ownerUnit.ID.ToString();
        attackData["ID_Unit_Attack"] = ((int)ownerUnit.ID_Unit).ToString();
        attackData["ID_User_Attack"] = ownerUnit.ID_User.ToString();

        attackData["ID_Defend"] = otherUnit.ID.ToString();
        attackData["ID_Unit_Defend"] = ((int)otherUnit.ID_Unit).ToString();
        attackData["ID_User_Defend"] = otherUnit.ID_User.ToString();

        attackData["Position_Cell_Attacker"] = AgentRemote.FixedMove.CurrentPosition.ToSerPosition().ToPositionString();
    }
}
