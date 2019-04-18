using Generic.Pooling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentLabelv2 : MonoBehaviour,IPoolable
{
    [SerializeField] private TextMeshProUGUI nameInGame;
    [SerializeField] private TextMeshProUGUI qualityText;

    public Image HealthFill;

    private void Awake()
    {
        SetHP(Random.Range(1,10), 10);
    }
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

    public int ManagedId { get; private set; }

    public void Dispose()
    {
        SetHP(1, 1);
        NameInGame = "";
        Quality = 0;
    }

    public void FirstSetup(int insId)
    {
        ManagedId = insId;
    }

    public void SetHP(float cur,float max)
    {
        HealthFill.fillAmount = cur / max;
    }
}
