using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgentLabel : MonoBehaviour
{
    private float currentHP;

    public float MaxHP;

    public Transform Head;
    public RectTransform Label;
    public Image HPFiller;
    public TextMeshProUGUI Quality;
    public TextMeshProUGUI NameInGame;
    public LookAt Lookat;

    private void Start()
    {        
        Enable();
        Lookat.Target = Camera.main.transform;
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

    public void SetHp(float cur)
    {
        currentHP = cur;
        HPFiller.fillAmount = (currentHP / MaxHP);
    }

    public void SetNameInGame(string vale)
    {
        NameInGame.text = vale;

    }

    public void SetQuality(int value)
    {
        Quality.text = value.ToString();
    }
}
