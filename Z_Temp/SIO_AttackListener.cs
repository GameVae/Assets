using DataTable;
using SocketIO;

public class SIO_AttackListener : Listener
{
    public JSONTable_Unit UnitTable;
    public override void RegisterCallback()
    {
        On("R_ATTACK", R_ATTACK);
        Debugger.Log("ON R_ATTACK");
    }

    private void R_ATTACK(SocketIOEvent obj)
    {
        Debugger.Log(obj);
        UnitTable.UpdateTable(obj.data["R_ATTACK"].ToString());
    }
}
