using Generic.Pooling;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Composites;
using UnityEngine;

public sealed class MemberTag : MonoBehaviour,IPoolable
{
    public SelectableComp UserInfoBtn;
    public TextMeshProUGUI UserName;
    public TextMeshProUGUI Describe;
    public SelectableComp IncreaseGradeBtn;
    public SelectableComp DecreaseGradeBtn;
    public SelectableComp KickBtn;

    public int ManagedId
    {
        get;
        private set;
    }

    public void Dispose()
    {
        UserInfoBtn.RemoveAllListener();
        UserName.text = "";
        Describe.text = "";
        IncreaseGradeBtn.RemoveAllListener();
        DecreaseGradeBtn.RemoveAllListener();
        KickBtn.RemoveAllListener();

        IncreaseGradeBtn.gameObject.SetActive(true);
        DecreaseGradeBtn.gameObject.SetActive(true);
        KickBtn.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }

    public void FirstSetup(int insId)
    {
        ManagedId = insId;
    }
}
