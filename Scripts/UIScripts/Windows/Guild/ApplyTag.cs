using Generic.Pooling;
using TMPro;
using UI.Composites;
using UnityEngine;

public sealed class ApplyTag : MonoBehaviour,IPoolable
{
    public SelectableComp UserInfoBtn;
    public TextMeshProUGUI UserName;
    public SelectableComp RejectBtn;
    public SelectableComp AcceptBtn;

    public int ManagedId
    {
        get;
        private set;
    }

    public void Dispose()
    {
        UserInfoBtn.RemoveAllListener();
        RejectBtn.RemoveAllListener();
        AcceptBtn.RemoveAllListener();
        UserName.text = "";

        gameObject.SetActive(false);
    }

    public void FirstSetup(int insId)
    {
        ManagedId = insId;
    }
}
