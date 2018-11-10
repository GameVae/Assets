using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Vector3Int CurrentCell
    {
        get
        {
            return HexMap.Instance.WorldToCell(transform.position).ZToZero();
        }
    }

    private void Start()
    {
        ObstacleManager.Instance.AddDynamicObstacle(obs: this);
    }

    public int GetIndex()
    {
        Vector3Int currentCell = CurrentCell;
        return HexMap.Instance.ConvertToIndex(currentCell.x, currentCell.y);
    }
}
