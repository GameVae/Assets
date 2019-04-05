using UI.Widget;
using UnityEngine;

public class PositionSelector : MonoBehaviour
{
    public GameObject Panel;
    public NumpadInput NumpadInput;
    public GUIInputField InputK;
    public GUIInputField InputX;
    public GUIInputField InputY;
    public GUIInteractableIcon AcceptButton;
    public CameraController CameraCtr;

    private Vector3Int Position;

    private void Awake()
    {
        InputK.OnSelectField += delegate { OnFieldSelected(InputK); };
        InputX.OnSelectField += delegate { OnFieldSelected(InputX); };
        InputY.OnSelectField += delegate { OnFieldSelected(InputY); };

        AcceptButton.OnClickEvents += OnAcceptButton;
    }

    private void OnAcceptButton()
    {
        int.TryParse(InputX.Text, out int x);
        int.TryParse(InputY.Text, out int y);

        Position.x = Mathf.Clamp(x, 0, 512);
        Position.y = Mathf.Clamp(y, 0, 512);

        CameraCtr.Set(Position.ToClientPosition());

        Close();
        NumpadInput.CloseNumpad();
    }

    private void OnFieldSelected(GUIInputField field)
    {
        NumpadInput.SetInputField(field);
        NumpadInput.OpenNumpad();

        int.TryParse(field.Text, out int v);
        NumpadInput.Numpad.InputInt = v;
    }

    private void Close()
    {
        Panel.SetActive(false);
    }

    public void Open()
    {
        if (Panel.activeInHierarchy)
        {
            NumpadInput.CloseNumpad();
            Panel.SetActive(false);
        }
        else
            Panel.SetActive(true);


    }
}
