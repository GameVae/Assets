using TMPro;
using UnityEngine;
using UI.Composites;
using Generic.Pooling;

public class FriendTag : MonoBehaviour,IPoolable
{
    public TextMeshProUGUI UserName;
    public TextMeshProUGUI Describe;
    public SelectableComp Icon_InfoButton;
    public SelectableComp Add_AcceptButon;
    public SelectableComp SendMsgButton;
    public SelectableComp RemoveButton;

    public int ManagedId { get; private set; }

    public void Dispose()
    {
        gameObject.SetActive(false);

        UserName.text = "";
        Describe.text = "";
        Icon_InfoButton.RemoveAllListener();
        Add_AcceptButon.RemoveAllListener();
        SendMsgButton.RemoveAllListener();
        RemoveButton.RemoveAllListener();
    }

    public void FirstSetup(int insId)
    {
        ManagedId = insId;
        gameObject.SetActive(false);
    }
}
