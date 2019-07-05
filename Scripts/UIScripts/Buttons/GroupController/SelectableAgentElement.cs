using Generic.Pooling;
using UI.Composites;
using UnityEngine;

public class SelectableAgentElement : MonoBehaviour, IPoolable
{
    public SelectableComp SelectableComp;
    public PlaceholderComp PlaceholderComp;

    private RectTransform rectTrans;

    public RectTransform RectTransform
    {
        get
        {
            return rectTrans ?? (rectTrans = (RectTransform)transform);
        }
    }

    public int ManagedId
    {
        get;
        private set;
    }

    public void Dispose()
    {
        SelectableComp.RemoveAllListener();
        PlaceholderComp.Text = "";
        gameObject.SetActive(false);
    }

    public void FirstSetup(int insId)
    {
        ManagedId = insId;
    }
}
