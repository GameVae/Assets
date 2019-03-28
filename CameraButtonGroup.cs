using Generic.Singleton;
using UnityEngine;

public class CameraButtonGroup : MonoBehaviour
{
    public CameraController CameraController;
    
    private bool isMoveToAgent;
    
    public void OnMoveToAgentButton()
    {
        isMoveToAgent = !isMoveToAgent;        
    }

    public void CameraMoveToAgent(Vector3Int position)
    {
        if (isMoveToAgent &&
            Generic.Contants.Constants.IsValidCell(position.x, position.y))
        {
            CameraController.Set(position);
        }
    }
}
