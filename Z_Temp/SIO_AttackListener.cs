using DataTable;
using Entities.Navigation;
using SocketIO;

public class SIO_AttackListener : Listener
{
    public JSONTable_Unit UnitTable;

    private AttackInfoGenerator attackInfoGenerator;
    public AttackInfoGenerator AttackInfoGenerator
    {
        get
        {
            return attackInfoGenerator ?? (attackInfoGenerator = new AttackInfoGenerator());
        }
    }

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

    public void S_ATTACK(NavRemote owner, NavRemote other)
    {
        JSONObject attackInfo = AttackInfoGenerator.CreateAttackInfo(owner, other);
        Emit("S_ATTACK", attackInfo);
    }
}
