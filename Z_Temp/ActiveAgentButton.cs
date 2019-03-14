using Entities.Navigation;
using System.Collections;
using System.Collections.Generic;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;

public class ActiveAgentButton : MonoBehaviour
{
    public GUIInteractableIcon AcceptBtn;
    public InputField InputField;
    public OwnerNavAgentManager OwnerNavAgentManager;

    private void Awake()
    {
        AcceptBtn.OnClickEvents += OnAccept;
    }

    private void OnAccept()
    {
        int id;
        int.TryParse(InputField.text, out id);
        OwnerNavAgentManager.ActiveNav(id);
    }

}
