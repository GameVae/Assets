using Generic.Singleton;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{

    public Camera Cam;
    public Connection Conn;

    private void Awake()
    {
        Conn = Singleton.Instance<Connection>();
    }

    private void Start()
    {
        Vector3Int cellIndex = Conn.Sync.CurrentMainBase.Position.Parse3Int();
        cellIndex.z = cellIndex.y;
        cellIndex += new Vector3Int(5, 0, 5);
        Vector3 worldPoint = Singleton.Instance<HexMap>().CellToWorld(cellIndex);
        worldPoint.y = Cam.transform.position.y;         // current height
        Cam.transform.position = worldPoint;

        Debug.Log(cellIndex + " - " + worldPoint);
    }
}
