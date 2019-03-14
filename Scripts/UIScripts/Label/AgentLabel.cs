using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentLabel : MonoBehaviour
{
    private float currentHP;
    private float maxHP;

    private LookAt Lookat;

    public Transform Head;
    public RectTransform Label;
    public Image HPFiller;
    public TextMeshProUGUI Quality;
    public TextMeshProUGUI NameInGame;

    private void Start()
    {        
        Enable();        
        Lookat.GameObject = Head;
    }

    public void Disable()
    {
        Label.gameObject.SetActive(false);
    }

    public void Enable()
    {
        Label.gameObject.SetActive(true);
    }

    public void SetMaxHP(float value)
    {
        maxHP = value;
    }

    public void SetHp(float cur)
    {
        currentHP = cur;
        HPFiller.fillAmount = (currentHP / maxHP);
    }

    public void SetNameInGame(string vale)
    {
        NameInGame.text = vale;
    }

    public void SetQuality(int value)
    {
        Quality.text = value.ToString();
    }

    public void LookAt(Transform trans)
    {
        if (Lookat == null)
            Lookat = Head.GetComponent<LookAt>();
        Lookat.Target = trans;
    }
}
