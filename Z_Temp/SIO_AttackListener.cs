using SocketIO;

public class SIO_AttackListener : Listener
{
    public override void RegisterCallback()
    {
        On("R_ATTACK", R_ATTACK);
        Debugger.Log("ON R_ATTACK");
    }

    private void R_ATTACK(SocketIOEvent obj)
    {
        Debugger.Log(obj);
    }
}
