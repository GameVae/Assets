using Entities.Navigation;
using Generic.Pooling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LightweightLabel : MonoBehaviour,IPoolable
{
    [SerializeField] private TextMeshProUGUI nameInGame;
    [SerializeField] private TextMeshProUGUI qualityText;

    private bool active;
    private Camera canvasCamera;
    private AgentRemote navRemote;
    private RectTransform rectTrans;

    public Image HealthFill;

    public string NameInGame
    {
        get { return nameInGame.text; }
        set { nameInGame.text = value; }
    }
    public int Quality
    {
        get
        {
            int.TryParse(qualityText.text, out int v);
            return v;
        }
        set
        {
            qualityText.text = value.ToString();
        }
    }
    public RectTransform RectTransform
    {
        get { return rectTrans ?? (rectTrans = transform as RectTransform); }
    }

    public int ManagedId { get; private set; }

    public void Dispose()
    {
        SetHP(1, 1);
        NameInGame = "";
        Quality = 0;
        active = false;
        navRemote = null;
        canvasCamera = null;

        gameObject.SetActive(false);
    }

    public void FirstSetup(int insId)
    {
        ManagedId = insId;
    }

    public void SetHP(float cur,float max)
    {
        HealthFill.fillAmount = cur / max;
    }

    public void Update()
    {
        if(active && navRemote != null)
        {
            RectTransform.SetPositionAndRotation(
                canvasCamera.WorldToScreenPoint(navRemote.HeadPoint.position),
                Quaternion.identity);
        }
    }

    public void Initalize(AgentRemote remote,Camera cam)
    {
        canvasCamera = cam;
        navRemote = remote;
        active = true;
        gameObject.SetActive(true);
    }
}
