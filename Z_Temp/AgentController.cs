using Generic.Singleton;
using ManualTable.Row;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    public AgentLabel Label;
    public UnitRow Data;

    private UserInfoRow currentUser;
    private void Start()
    {
        Init();
    }

    public void SetData(UnitRow data)
    {
        Data = data;
    }

    private void Init()
    {
        Label.MaxHP = Data.Health;
        Label.SetHp(Data.Hea_cur);
        Label.SetQuality(Data.Quality);

        Label.SetNameInGame(currentUser?.NameInGame);

    }

    public void SetCurrentUser(UserInfoRow user)
    {
        currentUser = user;
    }
}

